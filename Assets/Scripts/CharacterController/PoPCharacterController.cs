using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

/*!
 *	Extended class from characterController.
 *	This controller is desgined to fulfil all necessities of Patriots of the Past
 */
public sealed class PoPCharacterController : CharacterController_2 {
#pragma warning disable 0114
	//Set in Start() function
	const float mass = 1f;
	const float drag = 1f;
	const float rigidbodyDrag = 1f; 		//Typical values for Drag are between .001 (solid block of metal) and 10 (feather).
	const float rigidbodyAngularDrag = Mathf.Infinity;

	public Transform cameraTransform;		//!< Moves in conjunction with camera's transform
	private float inputThreshold = 0.1f;	//!< Dead zone value to determine if input is applied
	GoTweenChain chain;						//!< Used for tweening player actions

	//! Unity Start function
	void Start() {
		base.Start();
		if(!cameraTransform) {
			if(Camera.main)
				cameraTransform = Camera.main.transform;
			else
				Debug.Error("player", "Cannot find this.cameraTransform. Please connect the camera to the component using the inspector.");
		}
		// Reduce drag for momentum to carry
		GetComponent<Rigidbody>().drag = rigidbodyDrag;
		GetComponent<Rigidbody>().angularDrag = rigidbodyAngularDrag;
		GetComponent<Rigidbody>().mass = mass;
		GetComponent<Rigidbody>().drag = drag;
		// Constrain player rotations. Player can only turn on the Y axis
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

		currentWalkingSpeed = minWalkingSpeed;
		currentClimbingSpeed = minClimbingSpeed;
	}

	void Update() {
		// Input used for running and crouching
		SetModifiers(Input.GetKey("left shift"), Input.GetKey("left ctrl"));

		// Input used for movement, rotation, jumping, eventInput
		PlayerInput(Input.GetAxisRaw("Horizontal"),
					Input.GetAxisRaw("Jump"),
					Input.GetAxisRaw("Vertical"),
					Input.GetKeyDown("f"));

		// action:event button used to get out of events
		if(Input.GetKeyDown(KeyCode.Q)) {
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().drag = 1.0f;
		}

		// temporary hotkey to kill player
		if(Input.GetKey("k"))
			Death();
	}

	private void SetModifiers(bool running, bool crouched) {
		//Running
		isRunning = running;

		//Crouching
		if(isCrouched != crouched) {
			if(isCrouched) {
				myCollider.center = new Vector3(myCollider.center.x, myCollider.center.y + myCollider.height * crouchingMod, myCollider.center.z);
				myCollider.height *= 1 / crouchingMod;
			} else {
				myCollider.height *= crouchingMod;
				myCollider.center = new Vector3(myCollider.center.x, myCollider.center.y - myCollider.height * crouchingMod, myCollider.center.z);
			}
			isCrouched = crouched;
		}
	}

	// Check if there is player input
	private void PlayerInput(float xPlaneInput, float yPlaneInput, float zPlaneInput, bool action) {
		if(action) {
			if(actionAvaiable && myActionState == actionState.prepState) { //Start Action
				//action setup crap
				//transform.position = SetPosition(actionComponent.transform.position);
				transform.rotation = actionComponent.transform.rotation;
				rigidbody.useGravity = false;

				List<Vector3> v = new List<Vector3>();
				v.Add(transform.position);
				v.Add(SetPosition(actionComponent.transform.position));
				for(int i = 0; i < actionComponent.path.Count; i++) {
					v.Add(SetPosition(actionComponent.path[i]));
				}
				var path = new GoSpline(v, true);
				GoTween tween = new GoTween(transform, 4f, new GoTweenConfig().positionPath(path));
				chain = new GoTweenChain();
				chain.append(tween);

				myActionState = actionState.loopState;
			} else if(myActionState == actionState.loopState) { //End Action
				//todo: end action state code
				//action ending crap
				rigidbody.useGravity = true;
				actionComponent = null;
				myActionState = actionState.prepState;
			} else {
				Debug.Log("player", "No action avaiable.");
			}
		} else if(myActionState == actionState.loopState) { //Loop Action
			// todo: determine what to loop to do
			switch(actionComponent.actionType) {
				case PlayerActionPath.PlayerAction.sidle:
					ActionLoop(xPlaneInput);
					break;
				case PlayerActionPath.PlayerAction.ladder:
					ActionLoop(zPlaneInput);
					break;
				default:
					Debug.Error("player", "Action loop called with no valid action.");
					break;
			}
		} else {
			NoActionLoop(xPlaneInput, yPlaneInput, zPlaneInput);
		}
	}

	private void NoActionLoop(float xPlaneInput, float yPlaneInput, float zPlaneInput) {
		if(Mathf.Abs(yPlaneInput) > inputThreshold) { //Jumping
			// increment player speed as long as movement is being applied
			if(currentWalkingSpeed < maxWalkingSpeed)
				currentWalkingSpeed = Mathf.Clamp(currentWalkingSpeed + Time.deltaTime * Mathf.Pow(moveAcceleration, (float)movePow),minWalkingSpeed,maxWalkingSpeed);
			UpdateMovement(xPlaneInput, zPlaneInput);
			UpdateJump(yPlaneInput);
		} else if(Mathf.Abs(xPlaneInput) > inputThreshold || Mathf.Abs(zPlaneInput) > inputThreshold) { //Moving
			// increment player speed as long as movement is being applied
			if(currentWalkingSpeed < maxWalkingSpeed)
				currentWalkingSpeed = Mathf.Clamp(currentWalkingSpeed + Time.deltaTime * Mathf.Pow(moveAcceleration, (float)movePow), minWalkingSpeed, maxWalkingSpeed);
			UpdateMovement(xPlaneInput, zPlaneInput);
		} else { //Idle
			if(currentWalkingSpeed > minWalkingSpeed) {
				// decrement player speed as long as movement is being applied
				currentWalkingSpeed -= Time.deltaTime * moveDeceleration;
			}
		}
	}

	/*! Updates movement for the character
	 *	This includes walking/running movement
	 *	This includes grounded/jumping movement
	 */
	private void UpdateMovement(float xPlaneMovement, float zPlaneMovement) {
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;

		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		targetDirection = ((xPlaneMovement * (right)) + (zPlaneMovement * (forward)));
		if(!Mathf.Approximately(zPlaneMovement, 0.0f))
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection.normalized), rotationSpeed);

		// Movement direction and rotation
		// Movement velocity threshold
		if(grounded) {
			if(isRunning)
				GetComponent<Rigidbody>().AddForce(targetDirection.normalized * currentWalkingSpeed * runningSpeedMod);
			else
				GetComponent<Rigidbody>().AddForce(targetDirection.normalized * currentWalkingSpeed);
		} else {
			GetComponent<Rigidbody>().AddForce(targetDirection.normalized * currentWalkingSpeed * airMovementSpeedPercentage);
		}
		// Reset target direction
		targetDirection = Vector3.zero;
	}

	// y arguments taken to rotate player		
	private void UpdateJump(float yPlaneMovement) {
		if(grounded) {
			grounded = false;
			GetComponent<Rigidbody>().AddForce((yPlaneMovement * Vector3.up * maxJumpingHeight), ForceMode.Impulse);
		}
		// Reset target direction
		targetDirection = Vector3.zero;
	}

	private void ActionLoop(float input) {
		if(input > 0) {
			if(chain.state == GoTweenState.Paused || chain.isReversed == true)
				chain.playForward();
		} else if(input < 0) {
			if(chain.state == GoTweenState.Paused || chain.isReversed == false)
				chain.playBackwards();
		} else {
			chain.pause();
			Debug.Log("player", "stopped");
		}
	}

	[EventField]
	//! Kills the character
	public void Death() {
		Debug.Log("player", "Player has died.");
#if UNITY_EDITOR
		if(CheckpointManager.instance.checkpointTree == null) {
			Debug.Error("player", "While in Unity Editor, please load checkpoints before using death.");
			return;
		}
#endif
		Vector3 spawnpoint = CheckpointManager.instance.Respawn(transform.position);
		transform.position = SetPosition(spawnpoint);
	}

	private Vector3 SetPosition(Vector3 pos) {
		return new Vector3(pos.x, pos.y + myCollider.bounds.size.y / 2, pos.z);
	}

	private bool Vec3Approx(Vector3 a, Vector3 b) {
		if(!Mathf.Approximately(a.x, b.x))
			return false;
		if(!Mathf.Approximately(a.y, b.y))
			return false;
		if(!Mathf.Approximately(a.z, b.z))
			return false;
		return true;
	}
}
