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

	public enum CharacterState {Idle, Move, Pivot, Sprint, Crouch, Hang, Ladder, Sidle, Normal}
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

	private Vector3 groundNormal = Vector3.zero;

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

		// Set CharacterController Defaults
		charCont.slopeLimit = 30f;
	}

	void FixedUpdate()
	{
		if (cam == null)
			return;

		CalculateMovementDirection ();

		ApplyGravity ();

		DetermineCharacterState ();

		ProcessStandardMotion ();
	}

	void ProcessStandardMotion()
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
		} else {
			verticalVelocity = -1;
		}
	}

	void ApplySlide ()
	{
		if (!QK_Character_Movement.charCont.isGrounded)
			return;

		slideDirection = Vector3.zero;

		RaycastHit hitInfo;

		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 5f, cam.PlayerLM)) {
			groundNormal = hitInfo.normal;
			if (Vector3.Angle (hitInfo.normal, Vector3.up) < charCont.slopeLimit) {
				slideDirection = new Vector3 (hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
			}
		}

		/*if (slideDirection.magnitude < MaxControllableSlideMagnitude) {
			moveVector += slideDirection;
		}
		else {
			moveVector = slideDirection;
		}*/
	}
	
	void DetermineCharacterState () 
	{
		if (InputManager.input.isActionPressed ()) 
		{
			GameObject actionableObj = null;
			RaycastHit[] hit = Physics.SphereCastAll(transform.position + charCont.center, 2f, Vector3.forward, 10);
			if(hit.Length > 0)
			{
				if(hit.Length > 1)
				{
					foreach(RaycastHit obj in hit)
					{
						if(obj.collider.gameObject.GetComponent<Interactable>())
						{
							if(actionableObj == null)
								actionableObj = obj.collider.gameObject;
							else if(Vector3.Distance(actionableObj.transform.position, transform.position) < 
									Vector3.Distance(obj.collider.transform.position, transform.position))
							{
								actionableObj = obj.collider.gameObject;
							}
							Debug.Log("player", ""+actionableObj.name);
						}
					}
				}
			}
		}

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
			/*Gizmos.DrawSphere (transform.position + moveDir, 0.1f);
			Gizmos.DrawRay (transform.position, testMoveVect);
			Gizmos.DrawRay (transform.position, moveVector);*/
		}
	}
}
