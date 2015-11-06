using UnityEngine;
using System.Collections;
using Debug = FFP.Debug;

public class QK_Character_Movement : MonoBehaviour {

	private static QK_Character_Movement _instance;
	public static QK_Character_Movement Instance 
	{
		get 
		{ 
			return _instance ?? (_instance = GameObject.FindObjectOfType<QK_Character_Movement> ()); }
	}

	public enum CharacterState {Idle, Move, Pivot, Sprint, Crouch, Climb, Normal}
	[ReadOnlyAttribute]
	public CharacterState _moveState;
	[ReadOnlyAttribute]
	public CharacterState _stateModifier;

	public static CharacterController charCont;

	[ReadOnlyAttribute]
	public float curSpeed = 0f;
	private float acceleration = 0.7f;
	[ReadOnlyAttribute]
	public float runSpeed = 8f;
	private float sprintSpeed = 11f;
	private float crouchSpeed = 5f;
	[ReadOnlyAttribute]
	public float jumpSpeed = 8f;
	private float slideSpeed = 8f;
	private float gravity = 38f;
	[ReadOnlyAttribute]
	public float verticalVelocity = 0f;
	private float terminalVelocity = 20f;
	private float turnRate = 5f;

	public Vector3 moveVector = Vector3.zero;
	private Vector3 desiredMoveVector = Vector3.zero;
    private Vector3 inputDirection { get; set; }


	// This is for Slide if implemented
	private float slideTheshold = 0.6f;
	private float MaxControllableSlideMagnitude = 0.4f;
	private Vector3 slideDirection;
	private Quaternion targetAngle = Quaternion.identity;
	private PoPCamera cam;

	//Testing vectors
	Vector3 testDir1 = Vector3.right - Vector3.zero;
	Vector3 testDir2 = new Vector3(-1f, 0f, -1f) - Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		charCont = GetComponent ("CharacterController") as CharacterController;
		if(PoPCamera.instance != null)
			cam = PoPCamera.instance;
	}

	void FixedUpdate()
	{
		if (cam == null)
			return;

		ApplyGravity ();

		HandleActionInput ();

		CalculateMovementDirection ();

		ProcessMotion ();
	}

	void ProcessMotion()
	{
		//Multiply move by MoveSpeed
		if (_moveState == CharacterState.Move) {
			curSpeed += acceleration;
		} else {
			curSpeed -= acceleration;
		}

		if (_stateModifier == CharacterState.Sprint) {
			curSpeed = Mathf.Clamp (curSpeed, 0f, sprintSpeed);
		} else if (_stateModifier == CharacterState.Crouch) {
			curSpeed = Mathf.Clamp (curSpeed, 0f, crouchSpeed);
		} else {
			curSpeed = Mathf.Clamp (curSpeed, 0f, runSpeed);
		}
		
		// Apply Slide
		ApplySlide ();

		moveVector = desiredMoveVector * curSpeed;
		moveVector = new Vector3 (moveVector.x, verticalVelocity, moveVector.z);

		// Rotate Character
		if (_moveState == CharacterState.Move) {
			RotateCharacter (inputDirection);
			QK_Character_Movement.charCont.Move (moveVector * Time.deltaTime);
		} else if (_moveState == CharacterState.Pivot) {
			RotateCharacter (inputDirection);
		} else if (_moveState == CharacterState.Idle) {
			QK_Character_Movement.charCont.Move (moveVector * Time.deltaTime);
		}
	}

	void CalculateMovementDirection ()
	{
		float inputHor = InputManager.input.MoveHorizontalAxis();
		float inputVert = InputManager.input.MoveVerticalAxis();
		Vector3 forward = transform.position + cam.transform.forward;
		forward = new Vector3(forward.x, transform.position.y, forward.z);
		forward = Vector3.Normalize(forward - transform.position);
		Vector3 right = new Vector3(forward.z, 0f, -forward.x);
		
		inputDirection = (inputHor * right) + (inputVert * forward);
		desiredMoveVector = Vector3.Lerp (transform.forward, inputDirection, 0.5f);

		if (inputHor != 0f || inputVert != 0) {
			if(Vector3.Angle(transform.forward, inputDirection) >= 90f) {
				_moveState = CharacterState.Pivot;
			} else {
				_moveState = CharacterState.Move;
			}
		} else {
			_moveState = CharacterState.Idle;
		}
	}

	void ApplyGravity () 
	{
        if (!charCont.isGrounded && moveVector.y > -terminalVelocity) {
			verticalVelocity -= gravity * Time.deltaTime;
		}

		if (charCont.isGrounded) {
			verticalVelocity = 0f;
		}
	}

	void ApplySlide ()
	{
		if (!QK_Character_Movement.charCont.isGrounded)
			return;

		slideDirection = Vector3.zero;

		RaycastHit hitInfo;

		if (Physics.Raycast (transform.position + Vector3.up, Vector3.down, out hitInfo, cam.PlayerLM)) {
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
		if (QK_Character_Movement.charCont.isGrounded)
			verticalVelocity = jumpSpeed;
	}

	void RotateCharacter(Vector3 toRotate)
	{
		Quaternion newRotation = Quaternion.LookRotation (toRotate, Vector3.up);
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, turnRate * Time.deltaTime);
	}
		
	void OnDrawGizmosSelected()
	{
		if (cam) {
			float inputHor = InputManager.input.MoveHorizontalAxis ();
			float inputVert = InputManager.input.MoveVerticalAxis ();
			Vector3 forward = transform.position + cam.transform.forward;
			forward = new Vector3 (forward.x, transform.position.y, forward.z);
			forward = Vector3.Normalize (forward - transform.position);
			Vector3 right = new Vector3 (forward.z, 0f, -forward.x);

			Vector3 moveDir = (inputHor * right) + (inputVert * forward);

			Vector3 testMoveVect = Vector3.Lerp(transform.forward, moveDir, 0.5f);
			Gizmos.DrawSphere (transform.position + moveDir, 0.1f);
			Gizmos.DrawRay (transform.position, testMoveVect);
			Gizmos.DrawRay (transform.position, moveDir);
			//Gizmos.DrawRay (transform.position, forward);
			//Gizmos.DrawRay (transform.position, right);
		}
	}
}
