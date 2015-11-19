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

	public enum CharacterState {Idle, Move, Pivot, Sprint, Crouch, Hang, Ladder, Sidle, Wait, Normal}
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

	private Interactable iObject;

    // Ladder Variables
    private bool onLadder = false;
    private Vector3 climbToPosition = Vector3.zero;

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
		_moveState = CharacterState.Idle;
		_stateModifier = CharacterState.Normal;

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

		switch (_stateModifier) 
		{
			case CharacterState.Ladder:
				ClimbLadder();
				break;

			default:
					ProcessStandardMotion();
				break;
			
		}
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

		if (_moveState != CharacterState.Idle)
			RotateCharacter (inputDirection);

		charCont.Move (curMoveVect * Time.deltaTime);
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
		if(!IsInActionState())
		{
			if (InputManager.input.isActionPressed ()) 
			{
				iObject = GetActionObject();

				if(iObject != null)
				{
					if(iObject.Type == Interactable.ObjectType.Ladder)
					{
						_stateModifier = CharacterState.Ladder;
					}
					else if(iObject.Type == Interactable.ObjectType.Sidle)
					{
						_stateModifier = CharacterState.Sidle;
					}
					else if(iObject.Type == Interactable.ObjectType.Door)
					{
						_stateModifier = CharacterState.Wait;
					}
					else 
					{
						Debug.Warning("player", "No player action for type "+iObject.Type);
						_stateModifier = CharacterState.Normal;
					}

					// We have an action to do, break out
					return;
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
	}

	Interactable GetActionObject()
	{
		GameObject actionableObj = null;
		RaycastHit[] hit = Physics.SphereCastAll(transform.position + charCont.center, 1f, Vector3.forward, 10);
		if(hit.Length > 0)
		{
			foreach(RaycastHit obj in hit)
			{
				if(obj.collider.gameObject.GetComponent<Interactable>())
				{
					if(actionableObj == null)
						actionableObj = obj.collider.gameObject;
					else 
					{
						Vector3 toActionObj = actionableObj.transform.position - transform.position;
						toActionObj = new Vector3(toActionObj.x, 0f, toActionObj.z);
						Vector3 toNewObj = obj.collider.transform.position - transform.position;
						toNewObj = new Vector3(toNewObj.x, 0f, toNewObj.z);
						
						if(Vector3.Angle(toActionObj, transform.forward) < 45f && 
						   Vector3.Angle(toNewObj, transform.forward) < 45f)
						{
							if(Vector3.Distance(actionableObj.transform.position, transform.position) >
							   Vector3.Distance(obj.transform.position, transform.position))
							{
								actionableObj = obj.collider.gameObject;
							}
						}
						else if(Vector3.Angle(toNewObj, transform.forward) <
						        Vector3.Angle(toActionObj, transform.forward))
						{
							actionableObj = obj.collider.gameObject;
						}
					}
				}
			}
		}
		if (actionableObj != null)
			return actionableObj.GetComponent<Interactable> ();
		else
			return null;
	}

	void Jump() 
	{
		if (charCont.isGrounded)
			verticalVelocity = jumpSpeed;
	}

	void ClimbLadder()
	{
		if (iObject == null || iObject.Type != Interactable.ObjectType.Ladder)
		{
			// For some reason we're trying to climb something thats not a ladder
			_stateModifier = CharacterState.Normal;
			return;
		}

		// Snap to ladder
		if(!onLadder)
		{
            // Just use an estimated distance check because absolute position checking is inaccurate
            if (Vector3.Distance(transform.position, iObject.ladderStart) < 0.1f)
            {
                onLadder = true;
            }
            else
            {
                // Ready to begin climbing
                transform.position = Vector3.Lerp(transform.position, iObject.ladderStart, 2 * Time.deltaTime);
                climbToPosition = transform.position;
            }
		}

		Vector3 bottom = iObject.ladderStart;
		Vector3 top = iObject.ladderEnd;
		Vector3 climbDir = Vector3.Normalize(top - bottom)/5;

		if(InputManager.input.MoveVerticalAxis() > 0)
		{
			// Move up the Ladder
			if(transform.position == top)
			{
				// At the top, dismount
			}
			else
			{
				//Move Character in climbDir
                if (Vector3.Distance(climbToPosition, transform.position) <= 0.15)
                {
                    climbToPosition = transform.position + climbDir;
                    transform.position = Vector3.Lerp(transform.position, transform.position + climbDir, 0.1f);
                }
			}
		}
		else if(InputManager.input.MoveVerticalAxis() < 0)
		{
			// Move down the ladder
			if(transform.position == bottom)
			{
				// We're at the bottom, dismount
			}
			else
			{
				// Move characters position in negative climbDir by some step
			}
		}
	}

	bool IsInActionState()
	{
		if (_stateModifier == CharacterState.Ladder ||
			_stateModifier == CharacterState.Sidle ||
			_stateModifier == CharacterState.Hang) 
		{
			return true;
		}
		else
		{
			return false;
		}
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
			Gizmos.DrawSphere (transform.position, 1f);
			/*Gizmos.DrawSphere (transform.position + moveDir, 0.1f);
			Gizmos.DrawRay (transform.position, testMoveVect);
			Gizmos.DrawRay (transform.position, moveVector);*/
		}
	}
}
