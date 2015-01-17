using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
/*!
 *	Base class for a characterController. The _2 is appanded due to Unity's built in CharacterController class
 */
public abstract class CharacterController_2 : MonoBehaviour {
	
	//! Movement variables
	protected Vector3 targetDirection = Vector3.zero;
	[SerializeField] protected float minMovementSpeed = 10f;
	[SerializeField] protected float maxMovementSpeed = 20f;
	[SerializeField] protected float acceleration = 7.5f;
	[SerializeField] protected float deceleration = 20.0f;
	[SerializeField] public float currentMovementSpeed;
	public float rotationSpeed = 3.5f;
	
	// Jumping variables
	public Vector3 jumpDirection = Vector3.zero;
	[SerializeField] protected float maxJumpingHeight = 0.5f;
	[SerializeField] protected float airMovementSpeedPercentage = 0.1f;
	
	// Climbing variables
	[SerializeField] protected float minClimbingSpeed = 5f;
	[SerializeField] protected float maxClimbingSpeed = 10f;
	[SerializeField] public float currentClimbingSpeed;
	[SerializeField] protected float ladderClimbingSpeed = 200f;

	// Character states
	public enum characterState { idleState, movingState, walkingState, runningState, jumpingState, movingJumpState, climbingState, endClimbingState}
	public characterState currentCharacterState = characterState.idleState;
	
	//Only public for inspector. Should be protected
	[ReadOnly] public bool moving;
	[ReadOnly] public bool grounded;
	
	private int frameBuffer = 3;
	private List<float> _grounded;
	private List<Vector2> _moving;

	protected virtual void Start(){
		_grounded = new List<float>();
		_moving = new List<Vector2>();
	}

	//!Unity built in function, updates at a fixed interval. Used to calculated isGrounded and isMoving
	protected virtual void FixedUpdate(){
		//sets grounded
		if(_grounded.Count >= frameBuffer)
			_grounded.RemoveAt(0);
		_grounded.Add(Mathf.Round(transform.position.y * 100f) / 100f);
		
		grounded = false;
		for (int i = 0; i < _grounded.Count - 2; i++) {
			grounded ^= !Mathf.Approximately(_grounded [i],_grounded[i+1]);
		}
		grounded ^= true;
		
		//sets moving
		if(_moving.Count >= frameBuffer)
			_moving.RemoveAt(0);
		_moving.Add(new Vector2(Mathf.Round(transform.position.x * 100f) / 100f,
		                        Mathf.Round(transform.position.z * 100f) / 100f));
		
		moving = false;
		for (int i = 0; i < _moving.Count - 2; i++) {
			moving ^= !Mathf.Approximately(_moving [i].x,_moving[i+1].x);
			moving ^= !Mathf.Approximately(_moving [i].y,_moving[i+1].y);
		}
		//moving ^= true;
		//Debug.Log(grounded + "   " + moving);
	}
}
