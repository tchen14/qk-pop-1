using UnityEngine;
using System.Collections;

public class PoPPlayerController : CharacterController_2 {
	
	//! moves in conjunction with camera's transform
	public Transform cameraTransform;
	//! dead zone value to determine if input is applied
	private float inputThreshold = 0.1f;
	public bool eventInput = false;
	private Vector3 highestPoint;
	//! Unity Start function
	void Start()
	{
		if (!cameraTransform)
			Log.E ("camera", "Variable cameraTransform is missing a value.");
		// reduce drag for momentum to carry
		rigidbody.drag = 1.0f;
		// constrain player rotations
		rigidbody.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		currentMovementSpeed = minMovementSpeed;
		currentClimbingSpeed = minClimbingSpeed;
	}
	
	void FixedUpdate()
	{
		//testing purposes
		// input used for movement, rotation, jumping, eventInput
		float xPlaneInput = Input.GetAxisRaw("Horizontal");
		float zPlaneInput = Input.GetAxisRaw("Vertical");
		float yPlaneInput = Input.GetAxisRaw("Jump");
		playerInput(xPlaneInput,zPlaneInput,yPlaneInput);

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
	
	// check if there is player input
	// zPlane is JUMPING
	public void playerInput(float xPlaneInput, float zPlaneInput, float yPlaneInput)
	{
		if(!inMovementEvent)
		{
			// movement input
			if((Mathf.Abs(xPlaneInput) > inputThreshold || Mathf.Abs(zPlaneInput) > inputThreshold))
			{	
				// increment player speed as long as movement is being applied
				if(currentMovementSpeed < maxMovementSpeed)
				{
					currentMovementSpeed = currentMovementSpeed + Time.deltaTime * acceleration;
				}
				directionUpdate(xPlaneInput,zPlaneInput);
				movementUpdate();
			}
			// reset movement variables
			else
			{
				if(currentMovementSpeed > minMovementSpeed)
				{
					// decrement player speed as long as movement is being applied
					currentMovementSpeed = currentMovementSpeed - (Time.deltaTime*20f);
				}
				if(groundedCheck(collider))
				{
					currentCharacterState = characterState.idleState;
				}
			}
			// jump input
			if(Mathf.Abs(yPlaneInput) > inputThreshold)
			{
				currentCharacterState = characterState.jumpingState;
				// x and y inputs arguments for rotation in air
				jumpUpdate(yPlaneInput);
			}
			// reset jumping variables
			else
			{
				jumpDirection = Vector3.zero;
			}
		}
		// inMovementEvent
		else
		{
			if(currentCharacterState == characterState.climbingState)
				climbUpdate(zPlaneInput*ladderClimbingSpeed);
		}
	}
	
	public void directionUpdate(float xPlaneMovement, float zPlaneMovement)
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
	
	public void movementUpdate()
	{
		// movement direction and rotation
		// movement velocity threshold
		if(moveCheck())
		{
			if(groundedCheck(collider))
			{
				rigidbody.AddForce(targetDirection.normalized * currentMovementSpeed);
				currentCharacterState = characterState.runningState;
			}
			// reduce ability to move if not grounded
			else
			{
				rigidbody.AddForce((targetDirection.normalized * currentMovementSpeed) * airMovementSpeedPercentage);
				currentCharacterState = characterState.movingJumpState;
			}
		}
		// reset target direction
		targetDirection = Vector3.zero;
	}
	// x and y arguments taken to rotate player		
	public void jumpUpdate(float yPlaneMovement)
	{
		if(groundedCheck(collider))
		{
			if(currentCharacterState == characterState.jumpingState)
			{
				jumpDirection = (yPlaneMovement) * Vector3.up;
			}
			rigidbody.AddForce((jumpDirection * currentMovementSpeed * maxJumpingHeight), ForceMode.Impulse);
		}
	}
	
	public void climbUpdate(float climbingSpeed)
	{
		// if you are lower than the top of the ladder or you're moving down
		if((highestPoint.y >= transform.position.y || climbingSpeed < 0) && currentCharacterState == characterState.climbingState)
		{
			rigidbody.AddForce(Vector3.up * climbingSpeed);
		}
	}
	// this function is only called once currently; if you want a smooth transform, you need to create a function that is called in FixedUpdate
	public void positionAfterEvent(Vector3 position)
	{
		while(Vector3.Distance(transform.position,position) > 0.1f)
		{
			transform.position = Vector3.Lerp(transform.position,position,0.01f);
		}
	}
	string last = "";
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
			positionAfterEvent(collider.transform.parent.GetChild(2).position);

		}
	}
}
