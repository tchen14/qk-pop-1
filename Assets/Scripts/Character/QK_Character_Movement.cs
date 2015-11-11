using UnityEngine;
using System.Collections;
using Debug = FFP.Debug;

public class QK_Character_Movement : MonoBehaviour {

	private static QK_Character_Movement _instance;
	public static QK_Character_Movement Instance 
	{
		get 
		{ 
			return _instance ?? (_instance = GameObject.FindObjectOfType<QK_Character_Movement> ()); 
		}
	}

	public enum CharacterState {Idle, Move, Pivot, Sprint, Crouch, Climb, Normal}
	public CharacterState _moveState { get; private set; }
	public CharacterState _stateModifier { get; private set; }

	public static CharacterController charCont;

	[ReadOnlyAttribute]
	public float curSpeed = 0f;
	private float acceleration = 0.3f;
	[ReadOnlyAttribute]
	public float runSpeed = 8f;
	private float sprintSpeed = 12f;
	private float crouchSpeed = 4f;
	[ReadOnlyAttribute]
	public float jumpSpeed = 8f;
	private float slideSpeed = 8f;
	private float gravity = 30f;
	[ReadOnlyAttribute]
	public float verticalVelocity = 0f;
	private float terminalVelocity = 20f;
	private float turnRate = 5f;

	private Vector3 moveVector = Vector3.zero;
	private Vector3 desiredMoveVector = Vector3.zero;
    private Vector3 inputDirection { get; set; }


	// This is for Slide if implemented
	private float slideTheshold = 0.6f;
	private float MaxControllableSlideMagnitude = 0.4f;
	private Vector3 slideDirection;
	private Quaternion targetAngle = Quaternion.identity;
	private PoPCamera cam;

	// Use this for initialization
	void Start () 
	{
		charCont = this.GetComponent<CharacterController>();
		cam = PoPCamera.instance;
	}

	void FixedUpdate()
	{
		if (cam == null)
			return;

		CalculateMovementDirection ();

		ApplyGravity ();

		HandleActionInput ();

		ProcessMotion ();
	}

	void ProcessMotion()
	{
		if (InputManager.input.MoveVerticalAxis () != 0 || InputManager.input.MoveHorizontalAxis () != 0) {
			curSpeed += acceleration;
			curSpeed *= inputDirection.magnitude;
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
		
		moveVector = new Vector3 (desiredMoveVector.x, verticalVelocity, desiredMoveVector.z);

		Vector3 curMoveVect = new Vector3(moveVector.x * curSpeed, verticalVelocity, moveVector.z * curSpeed);

		/*/ Rotate Character
		if (_moveState == CharacterState.Move) 
        {
			RotateCharacter (inputDirection);
		}
		else if (_moveState == CharacterState.Pivot) 
        {
			RotateCharacter (inputDirection);
			curMoveVect = new Vector3(curMoveVect.x, verticalVelocity, curMoveVect.z);
		}*/

		if (_moveState != CharacterState.Idle)
			RotateCharacter (inputDirection);

		charCont.Move (curMoveVect * Time.deltaTime);

        /*if(_moveState != CharacterState.Idle)
            RotateCharacter(inputDirection);*/

        //charCont.Move(curMoveVect * Time.deltaTime);
	}

	void CalculateMovementDirection ()
	{
		float inputHor = InputManager.input.MoveHorizontalAxis();
		float inputVert = InputManager.input.MoveVerticalAxis();
		Vector3 forward = transform.position + cam.transform.forward;
		forward = new Vector3(forward.x, transform.position.y, forward.z);
		forward = Vector3.Normalize(forward - transform.position);
		Vector3 right = new Vector3(forward.z, 0f, -forward.x);
		
		inputDirection = Vector3.Normalize((inputHor * right) + (inputVert * forward));
		desiredMoveVector = Vector3.Lerp (moveVector, inputDirection, 0.2f);
		desiredMoveVector = new Vector3 (desiredMoveVector.x, 0f, desiredMoveVector.z);

		if (inputHor != 0f || inputVert != 0) 
        {
            if (_moveState == CharacterState.Idle)
            {
                _moveState = CharacterState.Pivot;
            }
            else if (_moveState == CharacterState.Pivot)
            {
                if (Vector3.Dot(Vector3.Normalize(transform.forward), Vector3.Normalize(inputDirection)) >= 0.9)
                    _moveState = CharacterState.Move;
                else
                    _moveState = CharacterState.Pivot;
            }
            else
            {
                if (Vector3.Angle(Vector3.Normalize(transform.forward), Vector3.Normalize(inputDirection)) >= 90f)
                {
                    _moveState = CharacterState.Pivot;
                } else {
                    _moveState = CharacterState.Move;
                }
            }
		} 
        else 
        {
			_moveState = CharacterState.Idle;
		}
	}

	void ApplyGravity () 
	{
        if (!charCont.isGrounded && moveVector.y > -terminalVelocity) {
			verticalVelocity -= gravity * Time.deltaTime;
		}

		Debug.Log ("player", "" + charCont.isGrounded);
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
		if (charCont.isGrounded)
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

			Vector3 moveDir = Vector3.Normalize((inputHor * right) + (inputVert * forward));

			Vector3 testMoveVect = Vector3.Lerp(moveVector, moveDir, 0.2f);
			testMoveVect = new Vector3(testMoveVect.x, 0f, testMoveVect.z);
			Gizmos.DrawSphere (transform.position + moveDir, 0.1f);
			Gizmos.DrawRay (transform.position, testMoveVect);
			Gizmos.DrawRay (transform.position, moveVector);
		}
	}
}
