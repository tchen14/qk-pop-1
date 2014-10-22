using UnityEngine;
using System.Collections;

public class PoPPlayerController : CharacterController_2
{

	// moves in conjunction with camera's transform
	public Transform cameraTransform;
	// dead zone value to determine if input is applied
	private float inputThreshold = 0.1f;
	
	// Use this for initialization
	void Start()
	{
		//find PoPCamera GameObject if one is not specified
		if (cameraTransform == null) {
			PoPCamera[] player = Object.FindObjectsOfType(typeof(PoPCamera)) as PoPCamera[];
			int single = 0;
			foreach (PoPCamera p in player) {
				if(single == 0){
					cameraTransform = p.transform;
				}else if(single == 1){
					Log.E("player", "Multiple PoPCamera in scene");
				}
				single++;
			}
			if(single == 0)
				Log.E("player", "No PoPCamera in scene");
		}
		
		// reduce drag for momentum to carry
		rigidbody.drag = 1.0f;
		// constrain player rotations
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	
	void FixedUpdate()
	{
		//testing purposes
		float xPlaneInput = Input.GetAxisRaw("Horizontal");
		float zPlaneInput = Input.GetAxisRaw("Vertical");
		float yPlaneInput = Input.GetAxisRaw("Jump");
		
		playerInput(xPlaneInput, zPlaneInput, yPlaneInput);
		groundedCheck(collider);
	}
	
	// check if there is player input
	// zPlane is JUMPING
	public void playerInput(float xPlaneInput, float zPlaneInput, float yPlaneInput)
	{
		// movement input
		if ((Mathf.Abs(xPlaneInput) > inputThreshold || Mathf.Abs(zPlaneInput) > inputThreshold)) {	
			// increment player speed as long as movement is being applied
			if (currentMovementSpeed < maxMovementSpeed) {
				currentMovementSpeed = currentMovementSpeed + Time.deltaTime * acceleration;
			}
			directionUpdate(xPlaneInput, zPlaneInput);
			movementUpdate();
		}
		// reset movement variables
		else {
			if (currentMovementSpeed > minMovementSpeed) {
				// decrement player speed as long as movement is being applied
				currentMovementSpeed = currentMovementSpeed - (Time.deltaTime * 20f);
			}
			isMoving = false;
			if (groundedCheck(collider)) {
				currentCharacterState = characterState.idleState;
			}
		}
		// jumping input
		// only jump if grounded
		if (groundedCheck(collider)) {
			if (yPlaneInput > inputThreshold) {
				currentCharacterState = characterState.jumpingState;
				// x and y inputs arguments for rotation in air
				jumpUpdate(yPlaneInput);
			}
			// reset jumping variables
			else {
				jumpDirection = Vector3.zero;
			}
		} else {
			// isn't jumping
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
		if (Mathf.Abs(currentMovementSpeed) > minMovementSpeed) {
			isMoving = true;
		}
		// movement velocity threshold
		if (isMoving) {
			if (groundedCheck(collider)) {
				rigidbody.AddForce(targetDirection.normalized * currentMovementSpeed);
				currentCharacterState = characterState.runningState;
			}
			// reduce ability to move if not grounded
			else {
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
		if (currentCharacterState == characterState.jumpingState) {
			jumpDirection = (yPlaneMovement * 1) * Vector3.up;
		}
		rigidbody.AddForce((jumpDirection * currentMovementSpeed * maxJumpingHeight), ForceMode.Impulse);
		currentCharacterState = characterState.jumpingState;

	}
}
