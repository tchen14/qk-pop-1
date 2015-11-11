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
	public CharacterState _moveState;
	public CharacterState _stateModifier;

	public static CharacterController CharacterController;

	public float curSpeed = 0f;
	private float acceleration = 1f;
	public float runSpeed = 10f;
	private float sprintSpeed = 14f;
	private float crouchSpeed = 7f;

	//public float backwardSpeed = 3f;
	//public float strafingSpeed = 6f;
	public float jumpSpeed = 10f;
	public float slideSpeed = 8f;
	public float gravity = 30f;
	public float terminalVelocity = 20f;
	public float turnRate = 5f;

	public Vector3 moveVector { get; set; }
    private Vector3 moveDir { get; set; }
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

	}

	void ProcessMotion()
	{
		//Transform move into World Space
		//moveVector = transform.TransformDirection (moveVector);
        float inputHor = InputManager.input.MoveHorizontalAxis();
        float inputVert = InputManager.input.MoveVerticalAxis();
        Vector3 forward = transform.position + cam.transform.forward;
        forward = new Vector3(forward.x, transform.position.y, forward.z);
        forward = Vector3.Normalize(forward - transform.position);
        Vector3 right = new Vector3(forward.z, 0f, -forward.x);

		if (inputHor != 0f || inputVert != 0) {
            _moveState = CharacterState.Move;
            moveDir = Vector3.Normalize((inputHor * right) + (inputVert * forward));
		} else {
			_moveState = CharacterState.Idle;
		}

		//Multiply move by MoveSpeed
        if (_moveState == CharacterState.Move)
            curSpeed += acceleration;
        else
            curSpeed -= acceleration;

		if (_stateModifier == CharacterState.Sprint)
			curSpeed = Mathf.Clamp (curSpeed, 0f, sprintSpeed);
		else if (_stateModifier == CharacterState.Crouch)
			curSpeed = Mathf.Clamp (curSpeed, 0f, crouchSpeed);
		else
			curSpeed = Mathf.Clamp(curSpeed, 0f, runSpeed);
		
		moveVector = moveDir * curSpeed;
		
		// Rotate Character
		if(_moveState == CharacterState.Move)
			RotateCharacter(moveDir);

        // Apply Slide
        ApplySlide();

        //Apply Gravity
        ApplyGravity();

        moveVector = new Vector3(moveVector.x, VerticalVelocity, moveVector.z);
        QK_Character_Movement.CharacterController.Move(moveVector * Time.deltaTime);
	}

	void ApplyGravity () 
	{
        if (moveVector.y > -terminalVelocity) {
			VerticalVelocity -= gravity * Time.deltaTime;
		}

		if (CharacterController.isGrounded && moveVector.y <= -1) {
			VerticalVelocity = 0;
		}
	}

	void ApplySlide ()
	{
		if (!QK_Character_Movement.CharacterController.isGrounded)
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
		if (QK_Character_Movement.CharacterController.isGrounded)
			VerticalVelocity = jumpSpeed;
	}

	void RotateCharacter(Vector3 toRotate)
	{
		Quaternion newRotation = Quaternion.LookRotation (toRotate, Vector3.up);
		transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, turnRate * Time.deltaTime);
	}
		
	void OnDrawGizmosSelected()
	{
		if (cam && Debug.IsKeyActive("player")) {
			float inputHor = InputManager.input.MoveHorizontalAxis ();
			float inputVert = InputManager.input.MoveVerticalAxis ();
			Vector3 forward = transform.position + cam.transform.forward;
			forward = new Vector3 (forward.x, transform.position.y, forward.z);
			forward = Vector3.Normalize (forward - transform.position);
			Vector3 right = new Vector3 (forward.z, 0f, -forward.x);

			Vector3 moveDir = (inputHor * right) + (inputVert * forward);
			Gizmos.DrawSphere (transform.position + moveDir, 0.1f);
			Gizmos.DrawRay (transform.position, moveDir);
			Gizmos.DrawRay (transform.position, forward);
			Gizmos.DrawRay (transform.position, right);
		}
	}
}
