using UnityEngine;
using System.Collections;

/*
 * Child Class of Parent Camera_2 for Patriots of the Past,
 * Inherits initial camera settings from Camera_2,
 * Uses mouse input to orbit camera in a 3D space and mouse wheel for zoom, input works with joysticks
 * Includes methods to handle player occlusion and object/environment clipping
 */

public class PoPCamera : Camera_2
{
	public static PoPCamera Instance;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		//find PoPPlayerController GameObject if one is not specified
		if (targetLookAt == null) {
			PoPPlayerController[] player = Object.FindObjectsOfType(typeof(PoPPlayerController)) as PoPPlayerController[];
			int single = 0;
			foreach (PoPPlayerController p in player) {
				if(single == 0){
					targetLookAt = p.transform;
				}else if(single == 1){
					Log.E("camera", "Multiple PoPPlayerController in scene");
				}
				single++;
			}
			if(single == 0)
				Log.E("camera", "No PoPPlayerController in scene");
		}
	
		distance = Mathf.Clamp(distance, distanceMin, distanceMax);
		cameraLatency = Mathf.Clamp(cameraLatency, 0.05f, 1f);
		Reset();
	}

	void FixedUpdate()
	{
		if (targetLookAt == null) {
			return;
		}

		HandleMouseInput();

		var count = 0;

		do {
			CalculateDesiredPosition();
			count++;
		} while(CheckifOccluded(count));

		UpdatePosition();
	}

	// Handle all player input and preapre for camera position calculation
	void HandleMouseInput()
	{
		var deadzone = 0.01f;

		//Take Mouse Input
		mouseX += Input.GetAxis("Mouse X") * xMouseSensitivity;
		mouseY -= Input.GetAxis("Mouse Y") * yMouseSensitivity;

		//Limit Y-axis input
		mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);

		//Get MouseWheel for zoom
		if (Input.GetAxis("Mouse ScrollWheel") < deadzone || Input.GetAxis("Mouse ScrollWheel") > deadzone) {
			desiredDistance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity, 
										  distanceMin, distanceMax);

			if (Input.GetAxis("Mouse ScrollWheel") != 0) {
				preOccludedDistance = desiredDistance;
			}
		}
	}

	// Checks if the target can see each point in the cameras near clippng plane
	// If it can target is not occluded, if not target is occluded
	bool CheckifOccluded(int count)
	{
		var isOccluded = false;

		var NearestDistance = CheckCameraPoints(targetLookAt.position, desiredPosition);

		if (NearestDistance != -1) {
			isOccluded = true;
			distance -= occlusionDistanceMove;

			desiredDistance = distance;
			distanceSmooth = distanceResumeSmooth;
		}

		return isOccluded;
	}

	// Creates clip plane points, checks if something is between target and points
	// Returns -1 if player is not occluded, nearest point to player if occluded
	float CheckCameraPoints(Vector3 from, Vector3 to)
	{
		var NearDistance = -1f;
		RaycastHit HitInfo;

		ClipPlanePoints PlanePoints = NearClipPlane(to);

		if (Physics.Linecast(from, PlanePoints.UpperLeft, out HitInfo) && HitInfo.collider.tag != "Player") {
			NearDistance = HitInfo.distance;
		}
		if (Physics.Linecast(from, PlanePoints.LowerLeft, out HitInfo) && HitInfo.collider.tag != "Player") {
			if (HitInfo.distance < NearDistance || NearDistance == -1) {
				NearDistance = HitInfo.distance;
			}
		}
		if (Physics.Linecast(from, PlanePoints.UpperRight, out HitInfo) && HitInfo.collider.tag != "Player") {
			if (HitInfo.distance < NearDistance || NearDistance == -1) {
				NearDistance = HitInfo.distance;
			}
		}
		if (Physics.Linecast(from, PlanePoints.LowerRight, out HitInfo) && HitInfo.collider.tag != "Player") {
			if (HitInfo.distance < NearDistance || NearDistance == -1) {
				NearDistance = HitInfo.distance;
			}
		}
		if (Physics.Linecast(from, to + transform.forward * -camera.nearClipPlane, out HitInfo) && HitInfo.collider.tag != "Player") {
			if (HitInfo.distance < NearDistance || NearDistance == -1) {
				NearDistance = HitInfo.distance;
			}
		}

		return NearDistance;
	}

	// Resets camera position to preoccluded position
	public override void ResetOccludedDistance()
	{
		if (desiredDistance < preOccludedDistance) {
			var pos = CalculatePosition(mouseY, mouseX, preOccludedDistance);

			var NearestDistance = CheckCameraPoints(targetLookAt.position, pos);

			if (NearestDistance == -1 || NearestDistance > preOccludedDistance) {
				desiredDistance = preOccludedDistance;
			}
		}
	}

	void UpdatePosition()
	{
		position = Vector3.Lerp(position, desiredPosition, cameraLatency);
		transform.position = position;

		transform.LookAt(targetLookAt); 
	}

	public void Reset()
	{
		mouseX = 0;
		mouseY = 10;
		desiredDistance = distance;
		preOccludedDistance = desiredDistance;
	}

	public struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
	}

	// Creates 4 Vector3 points to represent cameras near clipping plane
	public static ClipPlanePoints NearClipPlane(Vector3 position)
	{
		var PlanePoints = new ClipPlanePoints();

		if (Camera.main == null) {
			return PlanePoints;
		}

		var transform = Camera.main.transform;
		var halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
		var aspect = Camera.main.aspect;
		var distance = Camera.main.nearClipPlane;
		var height = distance * Mathf.Tan(halfFOV);
		var width = height * aspect;

		PlanePoints.LowerRight = position + transform.right * width;
		PlanePoints.LowerRight -= transform.up * height;
		PlanePoints.LowerRight += transform.forward * distance;

		PlanePoints.LowerLeft = position - transform.right * width;
		PlanePoints.LowerLeft -= transform.up * height;
		PlanePoints.LowerLeft += transform.forward * distance;
		
		PlanePoints.UpperRight = position + transform.right * width;
		PlanePoints.UpperRight += transform.up * height;
		PlanePoints.UpperRight += transform.forward * distance;
		
		PlanePoints.UpperLeft = position - transform.right * width;
		PlanePoints.UpperLeft += transform.up * height;
		PlanePoints.UpperLeft += transform.forward * distance;

		return PlanePoints;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		do {
			if (angle < -360) {
				angle %= 360;
			}
			if (angle > 360) {
				angle %= 360;
			}
		} while(angle < -360 || angle > 360);

		return Mathf.Clamp(angle, min, max);
	}
}