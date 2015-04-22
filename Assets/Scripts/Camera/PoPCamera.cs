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

[RequireComponent(typeof(CheckTargets))]
public sealed class PoPCamera : Camera_2
{
	public static PoPCamera _instance;

	// Ensures PoPCamera is in the scene when attempting to access it
	public static PoPCamera instance
	{
		get
		{
			return _instance ?? (_instance = GameObject.FindObjectOfType<PoPCamera>());
		}
	}

	//@{
	/// Tracks mouse and player position between frames
	private Vector3 mousePosition;
	//@}

	//@{
	/// Hidden members used for camera events
	[HideInInspector] public PoPCameraEvent eventTrigger;
	[HideInInspector] public bool inEvent;
	//@}
	
	private float curRotateX = 0f;

	public float maxTimeSinceMouseMoved = 2.0f;			//!< Time frame until camera becomes follow cam b/c of mouse inactivity
	private bool occluded = false;						//!< Used to check if camera is compensating for occlusion
	public float targetingRange = 40f;					//!< Range allowed to target objects
	public float screenTargetArea = 20f;				//!< Area of screen object can be targeted
	[HideInInspector] public int targetindex = 0;	    //!< Index in list of target objects to look at
    private float targetResetTimer = 0f;                //!< Timeframe camera resets after losing track of target
	public List<GameObject> targetedObjects;			//!< List of objects player is targeting
	public List<GameObject> allTargetables;				//!< Master List of all available targets in scene

	void Awake()
	{
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

		distance = Mathf.Clamp(distance, distanceMin, distanceMax);
		cameraLatency = Mathf.Clamp (cameraLatency, 0.05f, 1f);
		Reset();
	}

	void Update() {
		if(inTargetLock && Input.GetMouseButtonDown(1))
			inTargetLock = false;
		else if(!inTargetLock && Input.GetMouseButtonDown(1) && AcquireTarget().Count != 0f)
			inTargetLock = true;

		if(inTargetLock && Input.GetAxis("Mouse ScrollWheel") > 0f) {
			if(targetindex <= 0)
				targetindex = targetedObjects.Count - 1;
			else
				targetindex--;
		}

		if(inTargetLock && Input.GetAxis("Mouse ScrollWheel") < 0f) {
			if(targetindex == targetedObjects.Count - 1)
				targetindex = 0;
			else
				targetindex++;
		}
	}

	/*!
	 * The bulk of PoP's 3rd person camera controllers implementation
	 * Determines whether camera should be in an event, behind player, or in targeting mode
	 * Tracks mouse and player position at the end of the frame for use during frame
	 */
	void FixedUpdate()
	{
		if(!inTargetLock) {
			targetedObjects.Clear();
			targetedObjects.TrimExcess();
			targetindex = 0;
		}

		#if UNITY_EDITOR
		/** Swap material color of targeted objects for debug targetting **/
		if(Debug.IsKeyActive("camera")) {
			foreach(GameObject go in allTargetables) {
				go.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
			}
		}
		#endif

		if(inEvent)
		{
			eventTrigger.Event ();
			transform.position = Vector3.Lerp (eventTrigger.cameraStartPosition, eventTrigger.eventCameraPosition, cameraLatency);
			transform.LookAt (eventTrigger.eventCameraFocus);
		}
		else if(inTargetLock && (AcquireTarget().Count != 0f || targetedObjects.Count != 0))
		{
            if (targetedObjects.Count == 0f) {
                targetedObjects = AcquireTarget();
            }

            RaycastHit hit;
            Physics.Raycast(transform.position, (targetedObjects[targetindex].transform.position - transform.position), out hit);
            if (hit.collider.name != targetedObjects[targetindex].GetComponent<Collider>().name) {
                targetResetTimer -= Time.deltaTime;
            } else {
                targetResetTimer = 3f;
            }

            if (targetResetTimer <= 0f) {
                Reset();
                return;
            }

			TargetLockCamera(targetindex);

			#if UNITY_EDITOR
			/** Color target green for debug purposes **/
			if(Debug.IsKeyActive("camera"))
				targetedObjects[targetindex].GetComponent<Renderer>().material.color = Color.green;
			#endif

			UpdatePosition ();
		}
		else
		{
			inTargetLock = false;
			targetLookAt = player.position;
			
			HandleMouseInput (false);
			
			var count = 0;
			
			do {
				CalculateDesiredPosition ();
				count++;
			} while(CheckifOccluded(count, false));
			
			UpdatePosition ();
		}

		mousePosition = Input.mousePosition;
	}

	void CalculateDesiredPosition(float rotateY, float rotateX) {
		ResetOccludedDistance();

		// interpolate from current distance to desired
		distance = Mathf.Lerp(distance, desiredDistance, distanceSmooth);

		// Pass mouse input into function to calculate new position
		desiredPosition = CalculatePosition(rotateY, rotateX, distance);
	}

	// Handle all player mouse input and prepare for camera position calculation
	void HandleMouseInput(bool targetting)
	{
		var deadzone = 0.01f;

		// Takes mouse input if mouse is moving and increments camera rotation dependent variables
		// If mouse has been inactive for specified time period camera will become a followcam once player moves
		if(mousePosition != Input.mousePosition)
		{
			if(!targetting) {
				mouseX += Input.GetAxis("Mouse X") * xMouseSensitivity;
				mouseY -= Input.GetAxis("Mouse Y") * yMouseSensitivity;
			} else {
				curRotateX += Input.GetAxis("Mouse X") * xMouseSensitivity;
				mouseY -= Input.GetAxis("Mouse Y") * yMouseSensitivity;
				ClampAngle(curRotateX, curRotateX - 10f, curRotateX + 10f);
				Mathf.Clamp(mouseY, mouseY + 5f, mouseY - 5f);
			}
		}
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.BackQuote))
			mouseX = target.eulerAngles.y;
		
		/* Unused Code for followcam if mouse isn't moved
		 * Keeping in case we decide to put it back in
		else if(currentTimeSinceMouseMoved < maxTimeSinceMouseMoved)
		{
			currentTimeSinceMouseMoved += Time.deltaTime;
		}
		else if(targetPosition != target.position)
		{
			mouseX = target.eulerAngles.y;
		}*/

		//Limit Y-axis input
		mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);

		// Get MouseWheel for zoom
		// If compensating for occlusion don't clamp distance
		if(Input.GetAxis("Mouse ScrollWheel")< deadzone || Input.GetAxis("Mouse ScrollWheel") > deadzone)
		{
			Debug.Log("camera", Input.GetAxis("Mouse ScrollWheel").ToString());
			desiredDistance = distance - Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity;

			if(!occluded)
				desiredDistance = Mathf.Clamp(desiredDistance, distanceMin, distanceMax);

			if(Input.GetAxis("Mouse ScrollWheel") != 0)
				preOccludedDistance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * mouseWheelSensitivity, 
				                                  distanceMin, distanceMax);
		}
	}

	#region Targetting
    // Offsets camera position slightly to the right of the player during targeting, focuses target
	void TargetLockCamera(int i)
	{
		Vector3 player2DPos = new Vector3 (player.transform.position.x, 0, player.transform.position.z); 

		Vector3 toTarget = new Vector3(targetedObjects[i].transform.position.x, 0, targetedObjects[i].transform.position.z)
								- player2DPos;
		
		Vector3 toCamera = -Vector3.Normalize(toTarget);
		
		float angleFrom0 = Vector3.Angle ((player2DPos + Vector3.back) - player2DPos, toCamera);
		float angleFrom90 = Vector3.Angle ((player2DPos + Vector3.left) - player2DPos, toCamera);
		float angleFrom180 = Vector3.Angle ((player2DPos + Vector3.forward) - player2DPos, toCamera);
		float angleFrom270 = Vector3.Angle ((player2DPos + Vector3.right) - player2DPos, toCamera);

		if(angleFrom90 <= angleFrom270) {
			curRotateX = angleFrom0 - 20f;
		} else {
			curRotateX = angleFrom180 + 180f - 20f;
		}

		cameraLatency = 0.4f;
		targetLookAt = targetedObjects[targetindex].transform.position;

		int count = 0;
		do {
			CalculateDesiredPosition(mouseY, curRotateX);
			count++;
		} while(CheckifOccluded(count, true));

		inTargetLock = true;
	}

	// Finds closest targetable object from list and returns it
	public static List<GameObject> AcquireTarget() 
	{
		List<GameObject> targets = new List<GameObject>();

		foreach(GameObject go in PoPCamera.instance.allTargetables) {
			if(go.GetComponent<Targetable>().isTargetable) {
				if(Vector3.Angle(_instance.transform.forward, go.transform.position - _instance.transform.position) <= _instance.screenTargetArea) {
					if(targets.Count == 0) {
						targets.Add(go);
					} else {
						int i = 0;

						while(i < targets.Count && (Vector3.Distance(go.transform.position, _instance.transform.position) > Vector3.Distance(targets[i].transform.position, _instance.transform.position))) {
							i++;
						}

						targets.Insert(i, go);
					}
				}
			}
		}

        return targets;
	}
	#endregion

	#region Occlusion Checking
	/// Checks if the target can see each point in the cameras near clipping plane
	/// If it can target is not occluded, if not target is occluded
	bool CheckifOccluded(int count, bool targetting)
	{
		var isOccluded = false;
		var nearestDistance = 0f;

		if(targetting == false) {
			nearestDistance = CheckCameraPoints(target.position, desiredPosition);
		} else {
			nearestDistance = CheckCameraPoints(target.position, desiredPosition);
		}

			if(nearestDistance != -1) {
				if(count < 10) {
					isOccluded = occluded = true;
					distance -= occlusionDistanceMove;
					if(targetting)
						curRotateX += 0.01f;

					if(distance < 0.25f)
						distance = 0.25f;
				} else
					distance = nearestDistance - Camera.main.nearClipPlane;

				desiredDistance = distance - 0.5f;
				distanceSmooth = distanceResumeSmooth;
			}

		return isOccluded;
	}

	/// Creates near clip plane points, checks if something is between target and points
	/// Returns -1 if target is not occluded, returns nearest point to player if occluded
	float CheckCameraPoints(Vector3 from, Vector3 to)
	{
		var NearDistance = -1f;
		RaycastHit HitInfo;

		ClipPlanePoints PlanePoints = NearClipPlane (to);

		if (Physics.Linecast (from, PlanePoints.UpperLeft, out HitInfo) && HitInfo.collider.gameObject.name != "_Player")
			NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, PlanePoints.LowerLeft, out HitInfo) && HitInfo.collider.gameObject.name != "_Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, PlanePoints.UpperRight, out HitInfo) && HitInfo.collider.gameObject.name != "_Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, PlanePoints.LowerRight, out HitInfo) && HitInfo.collider.gameObject.name != "_Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;
		if (Physics.Linecast (from, to + transform.forward * -GetComponent<Camera>().nearClipPlane, out HitInfo) && HitInfo.collider.gameObject.name != "_Player")
			if(HitInfo.distance < NearDistance || NearDistance == -1)
				NearDistance = HitInfo.distance;

		return NearDistance;
	}

	 // Resets camera position to preoccluded position
	 public override void ResetOccludedDistance()
	 {
		 Vector3 pos;
		 if(desiredDistance < preOccludedDistance)
		 {
			 if(inTargetLock)
				pos = CalculatePosition(mouseY, curRotateX, preOccludedDistance);
			 else
				pos = CalculatePosition(mouseY, mouseX, preOccludedDistance);

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

	 public struct ClipPlanePoints {
		 public Vector3 UpperLeft;
		 public Vector3 UpperRight;
		 public Vector3 LowerLeft;
		 public Vector3 LowerRight;
	 }

	 /// Creates 4 Vector3 points to represent cameras near clipping plane
	 /// Used to determine player occlusion
	 public static ClipPlanePoints NearClipPlane(Vector3 position) {
		 var PlanePoints = new ClipPlanePoints();

		 if(Camera.main == null)
			 return PlanePoints;

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
	#endregion

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
		inTargetLock = false;
		targetedObjects.Clear();
		targetedObjects.TrimExcess();
        targetResetTimer = 3f;
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
			if(Vector3.Distance(this.target.position, target.transform.position) <= target.GetComponent<Targetable>().range)
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
		Gizmos.DrawSphere(this.target.position, targetingRange);
	}
}
