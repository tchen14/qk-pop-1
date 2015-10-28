using UnityEngine;
using System.Collections;
using Debug = FFP.Debug;

public class QK_Character_Movement : MonoBehaviour {

	private static QK_Character_Movement _instance;
	public static QK_Character_Movement Instance 
	{
		get { return _instance ?? (_instance = GameObject.FindObjectOfType<QK_Character_Movement> ()); }
	}

	public enum CharacterState {Idle, Move, Turn, Sprint, Crouch, Climb, Normal}
	private CharacterState _moveState;
	private CharacterState _stateModifier;

	public static CharacterController CharacterController;

	private float runSpeed = 5f;
	private float sprintSpeed = 8f;
	//public float backwardSpeed = 3f;
	//public float strafingSpeed = 6f;
	public float jumpSpeed = 6f;
	public float slideSpeed = 10f;
	public float gravity = 30f;
	public float terminalVelocity = 20f;
	public float turnRate = 3f;

	public Vector3 moveVector { get; set; }
	public float VerticalVelocity { get; set; }

	// This is for Slide if implemented
	public float slideTheshold = 0.6f;
	public float MaxControllableSlideMagnitude = 0.4f;
	private Vector3 slideDirection;
	private Quaternion targetAngle = Quaternion.identity;
	private PoPCamera cam;

	// Use this for initialization
	void Start () 
	{
		CharacterController = GetComponent ("CharacterController") as CharacterController;
		if(PoPCamera.instance != null)
			cam = PoPCamera.instance;
	}

	void FixedUpdate()
	{
		if (cam == null)
			return;
		
		HandleActionInput ();
		
		UpdateMotor ();
	}

	// Update is called once per frame
	public void UpdateMotor () 
	{
		//SnapAlignCharacterWithCamera ();
		ProcessMotion ();

		/*if (InputManager.input.MoveVerticalAxis() > 0) {
			targetAngle = Quaternion.Euler(transform.eulerAngles.x, 
			                               cam.transform.eulerAngles.y,
			                               transform.eulerAngles.z);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetAngle, turnRate * Time.deltaTime);
		}
		if (InputManager.input.MoveVerticalAxis() < 0) {
			targetAngle = Quaternion.Euler(transform.eulerAngles.x, 
			                               cam.transform.eulerAngles.y - 180,
			                               transform.eulerAngles.z);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetAngle, turnRate * Time.deltaTime);
		}
		if (InputManager.input.MoveHorizontalAxis() > 0) {
			targetAngle = Quaternion.Euler(transform.eulerAngles.x, 
			                               cam.transform.eulerAngles.y + 90f,
			                               transform.eulerAngles.z);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetAngle, turnRate * Time.deltaTime);
		}
		if (InputManager.input.MoveHorizontalAxis() < 0) {
			targetAngle = Quaternion.Euler(transform.eulerAngles.x, 
			                               cam.transform.eulerAngles.y - 90f,
			                               transform.eulerAngles.z);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetAngle, turnRate * Time.deltaTime);
		}*/

	}

	void ProcessMotion()
	{
		//Transform move into World Space
		//moveVector = transform.TransformDirection (moveVector);
		float inputHor = InputManager.input.MoveHorizontalAxis ();
		float inputVert = InputManager.input.MoveVerticalAxis ();
		Vector3 inputDir = new Vector3(inputHor, 0f, inputVert) - Vector3.zero;
		Vector3 inputVector = new Vector3 (transform.position.x + inputHor, 0f, transform.position.z + inputVert);
		Vector3 rotateVector = Vector3.Normalize (transform.position + inputDir);

		if (inputHor != 0f || inputVert != 0) {
			if (Vector3.Angle (rotateVector, transform.forward) < 15f)
				_moveState = CharacterState.Move;
			else
				_moveState = CharacterState.Turn;
		} else {
			_moveState = CharacterState.Idle;
		}

		switch (_moveState) 
		{
			case CharacterState.Move:
				//Multiply move by MoveSpeed
				moveVector = new Vector3(inputHor, 0f, inputVert);
				moveVector = transform.TransformDirection(moveVector);
				moveVector *= runSpeed;
				moveVector = new Vector3(moveVector.x, VerticalVelocity, moveVector.z);
				
				// Rotate Character
				RotateCharacter(rotateVector);
				// Apply Slide
				ApplySlide ();
			
				//Apply Gravity
				ApplyGravity ();

				QK_Character_Movement.CharacterController.Move (moveVector * Time.deltaTime);
			break;

			case CharacterState.Turn:
				RotateCharacter(rotateVector);
				break;
		}

		/*Normalize move if Magnitude > 1
		if (moveVector.magnitude > 1) {
			moveVector = Vector3.Normalize (moveVector);
		}*/

		//Apply Sliding if applicable
		ApplySlide ();


		//Apply Gravity
		ApplyGravity ();
	}

	void ApplyGravity () 
	{
		if (moveVector.y > -terminalVelocity) {
			moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);
		}

		if (QK_Controller.CharacterController.isGrounded && moveVector.y < -1) {
			moveVector = new Vector3 (moveVector.x, -1, moveVector.z);
		}
	}

	void ApplySlide ()
	{
		if (!QK_Character_Movement.CharacterController.isGrounded)
			return;

		slideDirection = Vector3.zero;

		RaycastHit hitInfo;

		if (Physics.Raycast (transform.position + Vector3.up, Vector3.down, out hitInfo)) {
			if (hitInfo.normal.y < slideTheshold) {
				slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
			}
		}

		if (slideDirection.magnitude < MaxControllableSlideMagnitude) {
			moveVector += slideDirection;
		}
		else {
			moveVector = slideDirection;
		}
	}
	
	void HandleActionInput () 
	{
		if (InputManager.input.isJumping()) {
			Jump ();
			return;
		}

		if (InputManager.input.isSprinting ()) {
			_stateModifier = CharacterState.Sprint;
		} else if (InputManager.input.isCrouched ()) {
			_stateModifier = CharacterState.Crouch;
		} else {
			_stateModifier = CharacterState.Normal;
		}
	}

	public void Jump() 
	{
		if (QK_Character_Movement.CharacterController.isGrounded)
			VerticalVelocity = jumpSpeed;
	}
	
	float MoveSpeed() 
	{
		float moveSpeed = 0f;

		if (slideDirection.magnitude > 0)
			moveSpeed = slideSpeed;

		return moveSpeed;
	}

	void RotateCharacter(Vector3 toRotate)
	{
		Quaternion newRotation = Quaternion.LookRotation (toRotate, Vector3.up);
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, turnRate * Time.deltaTime);
	}
		
	void OnDrawGizmosSelected()
	{
		float inputHor = InputManager.input.MoveHorizontalAxis ();
		float inputVert = InputManager.input.MoveVerticalAxis ();
		Vector3 inputVector = new Vector3 (transform.position.x + inputHor, 0f, transform.position.z + inputVert);
		Vector3 rotateVector = Vector3.Normalize (inputVector - transform.position);

		Gizmos.DrawSphere (inputVector, 0.1f);
		Gizmos.DrawRay (transform.position, rotateVector);
		Gizmos.DrawRay (transform.position, transform.forward);
	}
}
