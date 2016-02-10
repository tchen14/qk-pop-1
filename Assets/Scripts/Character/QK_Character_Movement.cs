﻿#pragma warning disable 414     //Variable assigned and not used: slideSpeed, groundNormal, slideTheshold, MaxControllableSlideMagnitude, slideDirection, targetAngle

using UnityEngine;
using System.Collections;
using Debug = FFP.Debug;

public class QK_Character_Movement : MonoBehaviour {
	
	private static QK_Character_Movement _instance;
	public static QK_Character_Movement Instance 
	{
		get 
		{
			_instance = _instance ?? (_instance = GameObject.FindObjectOfType<QK_Character_Movement>());
			if(_instance == null) {
				Debug.Warning("player", "Character Controller is not in scene but a script is attempting to reference it.");
			}
			return _instance;
		}
	}
	
	public enum CharacterState {Idle, Move, Pivot, Sprint, Crouch, Hang, Ladder, Sidle, Wait, Normal}
	public CharacterState _moveState { get; private set; }
	public CharacterState _stateModifier { get; private set; }
	
	public static CharacterController charCont;
	
	[ReadOnly] public float curSpeed = 0f;
	private float acceleration = 0.3f;
	[ReadOnly] public float runSpeed = 8f;
	private float sprintSpeed = 12f;
	private float crouchSpeed = 4f;
	[ReadOnly] public float jumpSpeed = 8f;
	private float slideSpeed = 8f;
	private float gravity = 30f;
	[ReadOnly] public float verticalVelocity = 0f;
	private float terminalVelocity = 30f;
	private float turnRate = 5f;
	private bool jumping = false;
	
	private Vector3 moveVector = Vector3.zero;
	private Vector3 desiredMoveVector = Vector3.zero;
	private Vector3 inputDirection { get; set; }
	private Vector3 groundNormal = Vector3.zero;
	
	private Interactable iObject;
	private GameObject triggeredObj;
	
	// Ladder Variables
	private bool onLadder = false;
	private bool dismountTop = false;
	private bool dismountBottom = false;
	private Vector3 climbToPosition = Vector3.zero;
	private Vector3 ladderDismountPos = Vector3.zero;
	
	// This is for Slide if implemented
	private float slideTheshold = 0.6f;
	private float MaxControllableSlideMagnitude = 0.4f;
	private Vector3 slideDirection;
	private Quaternion targetAngle = Quaternion.identity;
	private PoPCamera cam;
	private bool canJump = true;
	private bool slide = false;
	
	//Parkor?
	
	
	void Awake()
	{
		_instance = null;
	}
	
	// Use this for initialization
	void Start () 
	{
		charCont = this.GetComponent<CharacterController>();
		cam = PoPCamera.instance;
		_moveState = CharacterState.Idle;
		_stateModifier = CharacterState.Normal;
		
		// Set CharacterController Defaults
		charCont.slopeLimit = 60f;
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
		
		if (Input.GetAxisRaw ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0) {
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
		
		//curSpeed *= desiredMoveVector.magnitude;
		
		// Apply Slide
		ApplySlide ();
		//print (slide);
		if (!slide) { //disable movement if the character is sliding
			moveVector = new Vector3 (desiredMoveVector.x, verticalVelocity, desiredMoveVector.z);
		}
		Vector3 curMoveVect = new Vector3 (moveVector.x * curSpeed, verticalVelocity, moveVector.z * curSpeed);
		
		if (_moveState != CharacterState.Idle)
			RotateCharacter (inputDirection);
		
		charCont.Move (curMoveVect * Time.deltaTime);
		
	}
	
	void CalculateMovementDirection ()
	{
		float inputHor = Input.GetAxisRaw("Horizontal");
		float inputVert = Input.GetAxisRaw("Vertical");
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
		} else if(charCont.isGrounded) {
			verticalVelocity = -1;
		}
	}
	
	//Handle sliding on steep surfaces
	void ApplySlide ()
	{
		if (!QK_Character_Movement.charCont.isGrounded) {
			jumping = true;
			
			return;
		}
		jumping = false;
		
		
		
		slideDirection = Vector3.zero;
		
		RaycastHit hitInfo;
		//Raycast onto the object below you
		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 2f, cam.PlayerLM)) {
			
			groundNormal = hitInfo.normal;
			
			//if the angle is under slope limit, return
			if((Vector3.Angle (hitInfo.normal, Vector3.up)) <= (charCont.slopeLimit)){
				
				slide = false;
				canJump = true;
				return;
			}
			//if the angle is over the slope limit, continue, and disable jumping
			if (Vector3.Angle (hitInfo.normal, Vector3.up) > (charCont.slopeLimit)) {
				canJump = false;
			}
			else{
				canJump = true;
			}
			
			//this section catches the angle of the x, -x, z, and negative -z from the raycast
			//if it is "greater" than the slide threshhold (or less for the negative angle) then
			//feed into slideDirection the opposite angle
			
			if(hitInfo.normal.x> slideTheshold){
				slideDirection = new Vector3(-hitInfo.normal.x, hitInfo.normal.y,-hitInfo.normal.z);
				slide = true;
			}
			
			if(hitInfo.normal.x< slideTheshold*(-1f)){
				slideDirection = new Vector3(-hitInfo.normal.x, hitInfo.normal.y,-hitInfo.normal.z);
				slide = true;
			}
			
			
			if(hitInfo.normal.z> slideTheshold){
				slideDirection = new Vector3(-hitInfo.normal.x, hitInfo.normal.y, -hitInfo.normal.z);
				slide = true;
			}
			
			if(hitInfo.normal.z < slideTheshold*(-1)){
				slideDirection = new Vector3(-hitInfo.normal.x, hitInfo.normal.y, -hitInfo.normal.z);
				slide = true;
			}
			
		}
		//change our movement vector to the slide direction assigned above, continue to
		//slide until the character reaches an angle below the slide threshhold. 
		
		
		if (slide) {
			charCont.Move (slideDirection * Time.deltaTime * -slideSpeed);
		} else {
			moveVector = slideDirection;
		}
	}
	
	void DetermineCharacterState () 
	{
		
		if(!IsInActionState())
		{
			if (Input.GetKey(KeyCode.E)) 
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
			
			if (Input.GetButton("Jump")) {
				Jump ();
				return;
			}
			
			if (false) {
				_stateModifier = CharacterState.Sprint;
				
			} else if (false) {
				_stateModifier = CharacterState.Crouch;
			} else {
				_stateModifier = CharacterState.Normal;
				
			}
		}
	}
	
	Interactable GetActionObject()
	{
		GameObject actionableObj = null;
		RaycastHit[] hit = Physics.SphereCastAll(transform.position + charCont.center, 1.5f, Vector3.forward, 10);
		if(hit.Length > 0)
		{
			foreach(RaycastHit obj in hit)
			{
				if(GetComponentInHeirarchy<Interactable>(obj.collider.gameObject))
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
		if(actionableObj != null) {
			triggeredObj = actionableObj;
			return GetComponentInHeirarchy<Interactable>(actionableObj);
		} else
			return null;
	}
	
	
	
	
	void Jump() 
	{
		//slide bool is handed in ApplySlide, makes you jump backwards if you try to jump while sliding
		if ((charCont.isGrounded) && !slide) {
			verticalVelocity = jumpSpeed;
		}
		if ((charCont.isGrounded) && slide) {
			verticalVelocity = jumpSpeed;
			moveVector *= -0.5f;
		}
		
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
			if (Vector3.Distance(transform.position, triggeredObj.transform.position) < 0.01f) {
				// Ready to begin climbing
				onLadder = true;
				climbToPosition = triggeredObj.transform.position;
			} else {
				transform.position = Vector3.Lerp(transform.position, triggeredObj.transform.position, 2 * Time.deltaTime);
				transform.LookAt(transform.position - iObject.transform.forward);
			}
			
			return;
		}
		
		Vector3 bottom = iObject.ladderBottom;
		Vector3 top = iObject.ladderTop;
		Vector3 climbDir = Vector3.zero;
		
		// Check if we're at top or bottom
		if(dismountTop)
		{
			// Do dismount top
			transform.position = Vector3.Lerp(transform.position, top + transform.forward / 1.5f, Time.deltaTime * 2);
			
			if(Vector3.Distance(transform.position, top + transform.forward / 1.5f) <= 0.01f)
				Reset();
			return;
		}
		else if(dismountBottom) 
		{
			// Do dismount bottom
			transform.position = Vector3.Lerp(transform.position, bottom - transform.forward / 1.5f, Time.deltaTime * 2);
			
			if(Vector3.Distance(transform.position, bottom - transform.forward / 1.5f) <= 0.01f)
				Reset();
			return;
		}
		
		if(Vector3.Distance(climbToPosition, transform.position) > 0.1f)
		{
			transform.position = Vector3.Lerp(transform.position, climbToPosition, 0.1f);
			return;
		}
		
		if(Input.GetAxisRaw("Vertical") > 0)
		{
			if(Vector3.Distance(transform.position, top) <= 0.5f)
			{
				if(Vector3.Distance(transform.position, top) <= 0.01f)
					dismountTop = true;
				else
					transform.position = Vector3.Lerp(transform.position, top, Time.deltaTime * 2);
				return;
			}
			// Set position to move up
			climbDir = Vector3.Normalize(top - transform.position)/1.5f;
		}
		else if(Input.GetAxisRaw("Vertical") < 0)
		{
			if(Vector3.Distance(transform.position, bottom) <= 0.5f)
			{
				if(Vector3.Distance(transform.position, bottom) <= 0.01f)
					dismountBottom = true;
				else
					transform.position = Vector3.Lerp(transform.position, bottom, Time.deltaTime * 2);
				return;
			}
			// Set position to move down
			climbDir = Vector3.Normalize(bottom - transform.position)/1.5f;
		}
		
		climbToPosition = transform.position + climbDir;
	}
	
	bool IsInActionState()
	{
		if (_stateModifier == CharacterState.Ladder ||
		    _stateModifier == CharacterState.Sidle ||
		    _stateModifier == CharacterState.Hang ||
		    !charCont.isGrounded)
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
	
	void Reset()
	{
		_moveState = CharacterState.Idle;
		_stateModifier = CharacterState.Idle;
		moveVector = Vector3.zero;
		
		iObject = null;
		triggeredObj = null;
		
		dismountBottom = false;
		dismountTop = false;
		onLadder = false;
		climbToPosition = Vector3.zero;
	}
	
	public T GetComponentInHeirarchy<T>(GameObject obj) 
	{
		if(obj.GetComponent<T>() != null) {
			return obj.GetComponent<T>();
		} else if(obj.transform.parent == null) {
			return default(T);
		} else {
			return GetComponentInHeirarchy<T>(obj.transform.parent.gameObject);
		}
	}
	
	void OnDrawGizmosSelected()
	{
		#if UNITY_EDITOR
		if (cam && Debug.IsKeyActive("player")) {
			/*float inputHor = InputManager.input.MoveHorizontalAxis ();
			float inputVert = InputManager.input.MoveVerticalAxis ();
			Vector3 forward = transform.position + cam.transform.forward;
			forward = new Vector3 (forward.x, transform.position.y, forward.z);
			forward = Vector3.Normalize (forward - transform.position);
			Vector3 right = new Vector3 (forward.z, 0f, -forward.x);
			Vector3 moveDir = Vector3.Normalize((inputHor * right) + (inputVert * forward));
			Vector3 testMoveVect = Vector3.Lerp(moveVector, moveDir, 0.2f);
			testMoveVect = new Vector3(testMoveVect.x, 0f, testMoveVect.z);*/
			if(Input.GetKey(KeyCode.E)) 
			{
				Gizmos.DrawSphere(transform.position + charCont.center, 1.5f);
			}
		}
		#endif
	}
}