using UnityEngine;
using System.Collections;

/*!
 *	Extended class from characterController.
 *	This controller is desgined to fulfil all necessities of Patriots of the Past
 */
public class PoPCharacterController : CharacterController_2 {
	//Set in Start() function
	const float rigidbodyDrag = 1.0f;
	
	public Transform cameraTransform;	/*!< Moves in conjunction with camera's transform */
	private float inputThreshold = 0.1f;	/*!< Dead zone value to determine if input is applied */
	public bool eventInput = false;	/*!<  */
	private Vector3 highestPoint;	/*!<  */
	string last = "";	/*!<  */
	
	//! Unity Start function
	void Start()
	{
		// Reduce drag for momentum to carry
		rigidbody.drag = rigidbodyDrag;
		// Constrain player rotations. Player can only turn on the Y axis
		rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		currentMovementSpeed = minMovementSpeed;
		currentClimbingSpeed = minClimbingSpeed;
	}
	
	void FixedUpdate()
	{
		// Input used for movement, rotation, jumping, eventInput
		PlayerInput(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Jump"),Input.GetAxisRaw("Vertical"));

		// action:event button used to get out of events
		if(Input.GetKeyDown(KeyCode.Q))
		{
			eventInput = false;
			rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			rigidbody.useGravity = true;
			rigidbody.drag = 1.0f;
			inMovementEvent = false;
		}
	}
	
	// Check if there is player input
	// zPlane is JUMPING
	public void PlayerInput(float xPlaneInput, float yPlaneInput, float zPlaneInput)
	{
		if(!inMovementEvent)
		{
			// Movement input
			if((Mathf.Abs(xPlaneInput) > inputThreshold || Mathf.Abs(zPlaneInput) > inputThreshold))
			{	
				// increment player speed as long as movement is being applied
				if(currentMovementSpeed < maxMovementSpeed)
				{
					currentMovementSpeed = currentMovementSpeed + Time.deltaTime * acceleration;
				}
				DirectionUpdate(xPlaneInput,zPlaneInput);
				MovementUpdate();
			}
			// Reset movement variables
			else
			{
				if(currentMovementSpeed > minMovementSpeed)
				{
					// decrement player speed as long as movement is being applied
					currentMovementSpeed = currentMovementSpeed - (Time.deltaTime*20f);
				}
				if(CheckGrounded(collider))
				{
					currentCharacterState = characterState.idleState;
				}
			}
			// jump input
			if(Mathf.Abs(yPlaneInput) > inputThreshold)
			{
				currentCharacterState = characterState.jumpingState;
				// x and y inputs arguments for rotation in air
				JumpUpdate(yPlaneInput);
			}
			// reset jumping variables
			else
			{
				jumpDirection = Vector3.zero;
			}
		} else {
			if(currentCharacterState == characterState.climbingState)
				ClimbUpdate(zPlaneInput*ladderClimbingSpeed);
		}
	}
	
	public void DirectionUpdate(float xPlaneMovement, float zPlaneMovement)
	{
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		
		targetDirection = ((xPlaneMovement * (right)) + (zPlaneMovement * (forward)));
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection.normalized), rotationSpeed);
	}
	
	public void MovementUpdate()
	{
		// Movement direction and rotation
		// Movement velocity threshold
		if(CheckMoving())
		{
			if(CheckGrounded(collider))
			{
				rigidbody.AddForce(targetDirection.normalized * currentMovementSpeed);
				currentCharacterState = characterState.runningState;
			}
			// Reduce ability to move if not grounded
			else
			{
				rigidbody.AddForce((targetDirection.normalized * currentMovementSpeed) * airMovementSpeedPercentage);
				currentCharacterState = characterState.movingJumpState;
			}
		}
		// Reset target direction
		targetDirection = Vector3.zero;
	}
	// x and y arguments taken to rotate player		
	public void JumpUpdate(float yPlaneMovement)
	{
		if(CheckGrounded(collider))
		{
			if(currentCharacterState == characterState.jumpingState)
			{
				jumpDirection = (yPlaneMovement) * Vector3.up;
			}
			rigidbody.AddForce((jumpDirection * currentMovementSpeed * maxJumpingHeight), ForceMode.Impulse);
		}
	}
	
	public void ClimbUpdate(float climbingSpeed)
	{
		// If you are lower than the top of the ladder or you're moving down
		if((highestPoint.y >= transform.position.y || climbingSpeed < 0) && currentCharacterState == characterState.climbingState)
		{
			rigidbody.AddForce(cameraTransform.TransformDirection(Vector3.up) * climbingSpeed);
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
	
	//! Unity built in function. Used to detect continous collision with another GameObject
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
}
