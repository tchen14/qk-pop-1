using UnityEngine;
using System.Collections;
using UnityEditor;
using Debug = FFP.Debug;

/*!
 *	Extended class from characterController.
 *	This controller is desgined to fulfil all necessities of Patriots of the Past
 */
public class PoPCharacterController : CharacterController_2 {
	#pragma warning disable 0114
	//Set in Start() function
	const float rigidbodyDrag = 1f; 		//Typical values for Drag are between .001 (solid block of metal) and 10 (feather).
	const float rigidbodyAngularDrag = Mathf.Infinity;
	
	public Transform cameraTransform;		//!< Moves in conjunction with camera's transform
	private float inputThreshold = 0.1f;	//!< Dead zone value to determine if input is applied
	private Vector3 highestPoint;			//!< TBD

	//! Unity Start function
	void Start(){
		base.Start();
		if (!cameraTransform) {
			if(Camera.main)
				cameraTransform = Camera.main.transform;
			else
				Debug.Error ("player","Cannot find this.cameraTransform. Please connect the camera to the component using the inspector.");
		}
		// Reduce drag for momentum to carry
		rigidbody.drag = rigidbodyDrag;
		rigidbody.angularDrag = rigidbodyAngularDrag;
		// Constrain player rotations. Player can only turn on the Y axis
		rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		currentWalkingSpeed = minWalkingSpeed;
		currentClimbingSpeed = minClimbingSpeed;
	}

	void FixedUpdate(){
		// Input used for running and crouching
		SetModifiers(Input.GetKey("left shift"), Input.GetKey("left ctrl"));
		
		// Input used for movement, rotation, jumping, eventInput
		PlayerInput(Input.GetAxisRaw("Horizontal"),
		            Input.GetAxisRaw("Jump"),
		            Input.GetAxisRaw("Vertical"),
		            Input.GetKeyDown("f"));
		
		// action:event button used to get out of events
		if(Input.GetKeyDown(KeyCode.Q))
		{
			rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			rigidbody.useGravity = true;
			rigidbody.drag = 1.0f;
		}
		
		// temporary hotkey to kill player
		if(Input.GetKey("k"))
			Death();
	}
	
	private void SetModifiers(bool running, bool crouched){
		//Running
		isRunning = running;
		
		//Crouching
		if(isCrouched != crouched){
			if(isCrouched){
				myCollider.center = new Vector3(myCollider.center.x, myCollider.center.y + myCollider.height * crouchingMod, myCollider.center.z);
				myCollider.height *= 1/crouchingMod;
			}else{
				myCollider.height *= crouchingMod;
				myCollider.center = new Vector3(myCollider.center.x, myCollider.center.y - myCollider.height * crouchingMod, myCollider.center.z);
			}
			isCrouched = crouched;
		}
	}
	
	// Check if there is player input
	private void PlayerInput(float xPlaneInput, float yPlaneInput, float zPlaneInput, bool action){
		if(action){
			if(actionAvaiable && myActionState == actionState.prepState){ //Start Action
				switch(actionComponent.GetType().ToString()){
				case "SidlePoint":
					//sidle setup crap
					break;
				case "LadderPoint":
					//ladder setup crap
					break;
				default:
					Debug.Error("player","Action start called with no valid action.");
					break;
				}
				myActionState = actionState.loopState;
			}else if(myActionState == actionState.loopState){ //End Action
				//todo: end action state code
				switch(actionComponent.GetType().ToString()){
				case "SidlePoint":
					//sidle ending crap
					break;
				case "LadderPoint":
					//ladder ending crap
					break;
				default:
					Debug.Error("player","Action end called with no valid action.");
					break;
				}
				myActionState = actionState.prepState;
			}else{
				Debug.Error("player","Code should not reach here.");
			}
		}else if(myActionState == actionState.loopState){ //Loop Action
			// todo: determine what to loop to do
			switch(actionComponent.GetType().ToString()){
			case "SidlePoint":
				SidleLoop(xPlaneInput);
				break;
			case "LadderPoint":
				LadderLoop(yPlaneInput);
				break;
			default:
				Debug.Error("player","Action loop called with no valid action.");
				break;
			}
		}else{
			NoActionLoop(xPlaneInput, yPlaneInput, zPlaneInput);
		}
	}
	
	private void NoActionLoop(float xPlaneInput, float yPlaneInput, float zPlaneInput){
		if (Mathf.Abs (yPlaneInput) > inputThreshold) { //Jumping
			// increment player speed as long as movement is being applied
			if(currentWalkingSpeed < maxWalkingSpeed)
				currentWalkingSpeed = currentWalkingSpeed + Time.deltaTime * moveAcceleration;
			UpdateMovement(xPlaneInput,zPlaneInput);
			UpdateJump(yPlaneInput);
		}else if(Mathf.Abs(xPlaneInput) > inputThreshold || Mathf.Abs(zPlaneInput) > inputThreshold){ //Moving
			// increment player speed as long as movement is being applied
			if(currentWalkingSpeed < maxWalkingSpeed)
				currentWalkingSpeed = currentWalkingSpeed + Time.deltaTime * moveAcceleration;
			UpdateMovement(xPlaneInput,zPlaneInput);
		}else{ //Idle
			if(currentWalkingSpeed > minWalkingSpeed)
			{
				// decrement player speed as long as movement is being applied
				currentWalkingSpeed -= Time.deltaTime * moveDeceleration;
			}
		}
	}
	
	/*! Updates movement for the character
	 *	This includes walking/running movement
	 *	This includes grounded/jumping movement
	 */
	private void UpdateMovement(float xPlaneMovement, float zPlaneMovement)
	{
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
		if(grounded){
			if(isRunning)
				rigidbody.AddForce(targetDirection.normalized * currentWalkingSpeed * runningSpeedMod);
			else
				rigidbody.AddForce(targetDirection.normalized * currentWalkingSpeed);
		}else{
			rigidbody.AddForce(targetDirection.normalized * currentWalkingSpeed * airMovementSpeedPercentage);
		}
		// Reset target direction
		targetDirection = Vector3.zero;
	}
	
	// y arguments taken to rotate player		
	private void UpdateJump(float yPlaneMovement)
	{
		if(grounded)
		{
			rigidbody.AddForce((yPlaneMovement * Vector3.up * currentWalkingSpeed * maxJumpingHeight), ForceMode.Impulse);
		}
		// Reset target direction
		targetDirection = Vector3.zero;
	}
	
	private void SidleLoop(float xPlaneInput){
		SidlePoint s = (SidlePoint)actionComponent;
		if(Vec3Approx(transform.position,s.rightDestination.transform.position))
			;
	}
	
	private void LadderLoop(float yPlaneInput){
		
	}
	
	/*public void ClimbUpdate(float climbingSpeed)
	{
		// If you are lower than the top of the ladder or you're moving down
		if((highestPoint.y >= transform.position.y || climbingSpeed < 0))
		{
			rigidbody.AddForce(cameraTransform.TransformDirection(Vector3.up) * climbingSpeed);
		}
	}*/

	/*
	//! Unity built in function. Used to detect continous collision with another GameObject
	//!	Something something climbing code
	void OnTriggerStay(Collider collider)
	{ 
		// if you're in a collider
		if(Input.GetKeyDown(KeyCode.Q))
		{
			eventInput = !eventInput;
		}
		// starting a ladder event from the bottom
		if(collider.gameObject.tag == "BottomStartRange")
		{
			// player has hit event button
			if(eventInput)
			{
				// not in a movement event already
				if(!inMovementEvent)
				{ 
					// what object we started the climb from
					last = "BottomStartRange";
					// limit movement to movementEvent movement
					inMovementEvent = true;
					// set climbing state
					currentCharacterState = characterState.climbingState;
					// stop movement
					rigidbody.velocity = Vector3.zero;
					// no gravity while climbing
					rigidbody.useGravity = false;
					// more drag so player doesnt slide
					rigidbody.drag = 5.0f;
					// limit rotations
					rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
					// snap player to ladder
					transform.position = collider.transform.parent.GetChild(1).position;
					// snap rotation to ladder
					transform.rotation = collider.gameObject.transform.rotation;
					// highest point of ladder
					highestPoint = collider.transform.parent.parent.GetChild(2).GetChild(0).position;
				}
			}
		}
		else
		{
			last = "";
		}
		// starting a ladder event from the top
		if(collider.gameObject.tag == "TopStartRange")
		{
			// player has hit event button
			if(eventInput)
			{
				// not in a movement event already
				if(!inMovementEvent)
				{
					// what object we started the climb from
					last = "TopStartRange";
					// limit movement to movementEvent movement
					inMovementEvent = true;
					// set climbing state
					currentCharacterState = characterState.climbingState;
					rigidbody.velocity = Vector3.zero;
					// no gravity while climbing
					rigidbody.useGravity = false;
					// more drag so player doesnt slide
					rigidbody.drag = 5.0f;
					rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
					// snap player to ladder
					transform.position = collider.transform.parent.GetChild(1).position;
					// snap rotation to ladder
					transform.rotation = collider.gameObject.transform.rotation;
					// highest point of ladder
					highestPoint = collider.transform.parent.GetChild(0).position;
				}
			}
			else
			{
				last = "";
			}
		}
	}
	
	//! Unity built in function. Used to detect collision
	//! More ladder shit
	void OnTriggerEnter(Collider collider)
	{
		// if we're in a movement even and we collide with the bottom of the ladder
		if(inMovementEvent && collider.gameObject.tag == "BottomEndPos" && last != "BottomStartRange")
		{
			// reset variables for normal movement
			eventInput = false;
			rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			rigidbody.useGravity = true;
			rigidbody.drag = 1.0f;
			inMovementEvent = false;
		}
		// if we're in a movement even and we collide with the top of the ladder
		if(inMovementEvent && collider.gameObject.tag == "TopEndPos" && last != "TopStartRange")
		{
			// reset variables for normal movement
			eventInput = false;
			rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			rigidbody.useGravity = true;
			rigidbody.drag = 1.0f;
			inMovementEvent = false;
			// put player on roof next to top of ladder
			PositionAfterEvent(collider.transform.parent.GetChild(2).position);
			
		}
	}
	
	//! This function is only called once currently; if you want a smooth transform, you need to create a function that is called in FixedUpdate
	public void PositionAfterEvent(Vector3 position)
	{
		while(Vector3.Distance(transform.position,position) > 0.1f)
		{
			transform.position = Vector3.Lerp(transform.position,position,0.01f);
		}
	}
	*/
	
	[EventField]
	//! Kills the character
	public void Death(){
		Debug.Log("player","Player has died.");
		Vector3 spawnpoint = CheckpointManager.instance.Respawn(transform.position);
		transform.position = new Vector3(spawnpoint.x, spawnpoint.y + myCollider.bounds.size.y / 2, spawnpoint.z);
	}
	
	private bool Vec3Approx(Vector3 a, Vector3 b){
		if(!Mathf.Approximately(a.x, b.x))
			return false;
		if(!Mathf.Approximately(a.y, b.y))
			return false;
		if(!Mathf.Approximately(a.z, b.z))
			return false;
		return true;
	}
}
