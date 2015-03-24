using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

/*!
 * Child Class of Parent Camera_2 for Patriots of the Past,
 * Inherits initial camera settings from Camera_2,
 * Uses mouse input to orbit camera in a 3D space and mouse wheel for zoom, input works with joysticks
 * Camera follows character if no mouse input is detected
 * Includes methods to handle player occlusion and object/environment clipping
 */

public class PoPCamera : Camera_2
{
	public static PoPCamera Instance;

	// Tracks mouse and player position between frames
	private Vector3 mousePosition;
	private Vector3 targetPosition;

	// Hidden public event variables
	[HideInInspector]
	public PoPCameraEvent eventTrigger;
	[HideInInspector]
	public bool inEvent;

	public float maxTimeSinceMouseMoved = 2.0f;			//!<Time frame until camera becomes follow cam b/c of mouse inactivity
	private float currentTimeSinceMouseMoved = 0.0f;	//!<Private variable that stores mouse inactivity
	private bool occluded = false;						//!<Used to check if camera is compensating for occlusion
	public float targetingRange = 40f;					//!<Range allowed to target objects
	public float screenTargetArea = 20f;				//!<Area of screen object can be targeted
	private GameObject targetedObject;					//!<Object player is targeting
	public List<GameObject> allTargetables;				//!<Master List of all targets in scene


	void Awake()
	{
		Instance = this;
		player = target;
	}

	void Start()
	{
		if (!target) {
			if(GameObject.FindObjectOfType<PoPCharacterController>()){
				target = (Transform)GameObject.FindObjectOfType<PoPCharacterController>().transform;
				player = target;
			}else{
				Debug.Error ("camera","Cannot find this.target. Please connect the GameObject to the component using the inspector.");
				target = transform;
			}
		}

		targetPosition = target.position;
		distance = Mathf.Clamp(distance, distanceMin, distanceMax);
		cameraLatency = Mathf.Clamp (cameraLatency, 0.05f, 1f);
		Reset();
	}

	/*!
	 * The bulk of the camera controllers implementation
	 * Determines whether camera should be in an event, behind player, or targeting mode
	 * Tracks mouse and player position at the end of the frame for use during frame
	 */
	void FixedUpdate()
	{
		//**Used for target testing**
		if(targetedObject)
		{
			targetedObject.GetComponent<Renderer>().material.color = Color.red;
		}

		if(inEvent)
		{
			eventTrigger.Event ();
			transform.position = Vector3.Lerp (eventTrigger.cameraStartPosition, eventTrigger.eventCameraPosition, cameraLatency);
			transform.LookAt (eventTrigger.eventCameraFocus);
		}
		else if(Input.GetKeyDown ("f") && AcquireTarget())
		{
			if(Input.GetKey("f"))
			{
				targetedObject = AcquireTarget();

				targetLookAt = targetedObject.transform.position;
				
				//HandleMouseInput ();

				var count = 0;
				
				do {
					CalculateDesiredPosition ();
					count++;
				} while(CheckifOccluded(count));
				
				UpdatePosition ();
			}
		}
		else
		{
			targetLookAt = target.position;
			
			HandleMouseInput ();
			
			var count = 0;
			
			do {
				CalculateDesiredPosition ();
				count++;
			} while(CheckifOccluded(count));
			
			UpdatePosition ();
		}

		//**More Target Testing**
		if(Input.GetKey("f"))
		{
			targetedObject = AcquireTarget();

			if(targetedObject)
			{
				targetedObject.GetComponent<Renderer>().material.color = Color.green;
			}
		}

		mousePosition = Input.mousePosition;
		targetPosition = target.position;
	}

	// Handle all player mouse input and prepare for camera position calculation
	void HandleMouseInput()
	{
		var deadzone = 0.01f;

		// Takes mouse input if mouse is moving and increments camera rotation dependent variables
		// If mouse has been inactive for specified time period camera will become a followcam once player moves
		if(mousePosition != Input.mousePosition)
		{
			mouseX += Input.GetAxis("Mouse X") * xMouseSensitivity;
			mouseY -= Input.GetAxis("Mouse Y") * yMouseSensitivity;
			currentTimeSinceMouseMoved = 0.0f;
		}
		else if(currentTimeSinceMouseMoved < maxTimeSinceMouseMoved)
		{
			currentTimeSinceMouseMoved += Time.deltaTime;
		}
		else if(targetPosition != target.position)
		{
			mouseX = target.eulerAngles.y;
		}

		//Limit Y-axis input
		mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);

		// Get MouseWheel for zoom
		// If compensating for occlusion don't clamp distance
		if(Input.GetAxis("Mouse ScrollWheel") < deadzone || Input.GetAxis("Mouse ScrollWheel") > deadzone)
		{
			desiredDistance = distance - Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity;

			if(!occluded)
				desiredDistance = Mathf.Clamp(desiredDistance, distanceMin, distanceMax);

			if(Input.GetAxis("Mouse ScrollWheel") != 0)
				preOccludedDistance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity, 
				                                  distanceMin, distanceMax);
		}
	}
	
	// Checks if the target can see each point in the cameras near clippng plane
	// If it can target is not occluded, if not target is occluded
	bool CheckifOccluded(int count)
	{
		var isOccluded = false;

		var nearestDistance = CheckCameraPoints (target.position, desiredPosition);

		if (nearestDistance != -1) 
		{
			if(count < 10)
			{
				isOccluded = occluded = true;
				distance -= occlusionDistanceMove;

				if(distance < 0.25f)
					distance = 0.25f;
			}
			else
				distance = nearestDistance - Camera.main.nearClipPlane;

			desiredDistance = distance;
			distanceSmooth = distanceResumeSmooth;
			occluded = true;
		}

		return isOccluded;
	}

	// Creates clip plane points, checks if something is between target and points
	// Returns -1 if player is not occluded, returns nearest point to player if occluded
	float CheckCameraPoints(Vector3 from, Vector3 to)
	{
		var NearDistance = -1f;
		RaycastHit HitInfo;

		ClipPlanePoints PlanePoints = NearClipPlane (to);

		if (Physics.Linecast (from, PlanePoints.UpperLeft, out HitInfo) && HitInfo.collider.tag != "Player")
			NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, PlanePoints.LowerLeft, out HitInfo) && HitInfo.collider.tag != "Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, PlanePoints.UpperRight, out HitInfo) && HitInfo.collider.tag != "Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, PlanePoints.LowerRight, out HitInfo) && HitInfo.collider.tag != "Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, to + transform.forward * -GetComponent<Camera>().nearClipPlane, out HitInfo) && HitInfo.collider.tag != "Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;

		return NearDistance;
	}

	 // Resets camera position to preoccluded position
	 public override void ResetOccludedDistance()
	 {
		 if(desiredDistance < preOccludedDistance)
		 {
			var pos = CalculatePosition(mouseY, mouseX, preOccludedDistance);

			var nearestDistance = CheckCameraPoints(target.position, pos);

			if(nearestDistance == -1 || nearestDistance > preOccludedDistance)
			{
				desiredDistance = preOccludedDistance;
				occluded = false;
			}
			else
				desiredDistance = nearestDistance - Camera.main.nearClipPlane;
		 }
	 }

	//Finds closest targetable object from list and returns it
	public GameObject AcquireTarget()
	{
		GameObject potentialTarget = null;

		foreach(GameObject target in allTargetables)
		{
			if(target.GetComponent<Targetable>().isTargetable)
			{
				if(Vector3.Angle(this.transform.forward, target.transform.position - this.transform.position) <= screenTargetArea)
				{
					if(!potentialTarget)
					{
						potentialTarget = target;
					}
					else
					{
						if(Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(potentialTarget.transform.position, transform.position))
							potentialTarget = target;
					}
				}
			}
		}
		
		return potentialTarget;
	}

	// Resets camera variables to default values
	public void Reset()
	{
		target = player;
		mouseX = 0;
		mouseY = 10;
		cameraLatency = 0.2f;
		distance = 5f;
		desiredDistance = distance;
		preOccludedDistance = desiredDistance;
		inEvent = false;
	}

	public struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
	}

	// Creates 4 Vector3 points to represent cameras near clipping plane
	// Used to determine player occlusion
	public static ClipPlanePoints NearClipPlane(Vector3 position)
	{
		var PlanePoints = new ClipPlanePoints ();

		if (Camera.main == null)
			return PlanePoints;

		var transform = Camera.main.transform;
		var halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
		var aspect = Camera.main.aspect;
		var distance = Camera.main.nearClipPlane;
		var height = distance * Mathf.Tan (halfFOV);
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

	// Clamps a given angle to 360, then clamps that to min/max 
	public static float ClampAngle(float angle, float min, float max)
	{
		do
		{
			if(angle < -360)
				angle %= 360;
			if(angle > 360)
				angle %= 360;
		} while(angle < -360 || angle > 360);

		return Mathf.Clamp(angle, min, max);
	}

	// Draws Gizmos when Camera is selected in scene editor to assist in targetable object placing
	void OnDrawGizmosSelected()
	{
		foreach(GameObject target in allTargetables)
		{
			if(Vector3.Distance(this.target.position, target.transform.position) <= targetingRange)
			{
				if(target.GetComponent<Targetable>().isTargetable)
				{
					if(Vector3.Angle(this.transform.forward, target.transform.position - this.transform.position) <= screenTargetArea)
					{
						Gizmos.color = Color.green;
						Gizmos.DrawRay (target.transform.position, (this.transform.position - target.transform.position));
					}
					else
					{
						Gizmos.color = Color.red;
						Gizmos.DrawRay (target.transform.position, (this.transform.position - target.transform.position));
					}
				}
				else
				{
					Gizmos.color = Color.red;
					Gizmos.DrawRay (target.transform.position, (this.transform.position - target.transform.position));
				}
			}
		}

		Gizmos.color = new Color(1,1,1,0.5f);
		Gizmos.DrawSphere(this.target.position, 20f);
	}
}
