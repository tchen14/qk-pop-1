using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
/*!
 *	Base class for a characterController. The _2 is appanded due to Unity's built in CharacterController class
 */
public abstract class CharacterController_2 : MonoBehaviour {
	
	//! Overlord variables
	protected CapsuleCollider myCollider;
	
	// Movement variables
	protected Vector3 targetDirection = Vector3.zero;
	protected float currentWalkingSpeed;
	public const float rotationSpeed = 3.5f;
	protected const float minWalkingSpeed = 10f;
	protected const float maxWalkingSpeed = 20f;
	protected const float runningSpeedMod = 1.2f;
	protected const float moveAcceleration = 7.5f;
	protected const float moveDeceleration = 20.0f;
	
	// Jumping variables
	[ReadOnly] public bool grounded = false; //Only public for inspector. Should be protected
	protected const float maxJumpingHeight = 0.5f;
	protected const float airMovementSpeedPercentage = 0.1f;
	
	// Crouching variables
	protected const float crouchingMod = 0.5f;
	
	// Climbing variables
	[ReadOnly] public float currentClimbingSpeed;
	protected const float minClimbingSpeed = 5f;
	protected const float maxClimbingSpeed = 10f;
	
	// Modifiers
	[ReadOnly] public bool isRunning = false; //Only public for inspector. Should be protected
	[ReadOnly] public bool isCrouched = false; //Only public for inspector. Should be protected

	// Character states
	public enum characterState { idleState, movingState, jumpingState, movingJumpState, climbingState, endClimbingState}
	public characterState currentCharacterState = characterState.idleState;
	
	public virtual void Start(){
		myCollider = GetComponent<CapsuleCollider>();
	}
	
	//! Detect if Character is grounded (true)
	void OnCollisionEnter(Collision collisionInfo){
		if(collisionInfo.collider.tag == "Navigation")
			grounded = true;
	}
	
	//! Detect if Character is grounded (false)
	void OnCollisionExit(Collision collisionInfo){
		if(collisionInfo.collider.tag == "Navigation")
			grounded = false;
	}
}
