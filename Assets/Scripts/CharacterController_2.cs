using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
/*!
 *	Base class for a characterController. The _2 is appanded due to Unity's built in CharacterController class
 */
public abstract class CharacterController_2 : MonoBehaviour {
	
	//! Movement variables
	protected Vector3 targetDirection = Vector3.zero;
	[SerializeField] protected float minMovementSpeed = 10f;
	[SerializeField] protected float maxMovementSpeed = 20f;
	[SerializeField] protected float acceleration = 7.5f;
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

#if UNITY_EDITOR
	// Used only to see status in inspector
	[ReadOnly] public bool isMoving;
	[ReadOnly] public bool isGrounded;
#endif
	
	public bool inMovementEvent = false;

	// Character states
	public enum characterState { idleState, walkingState, runningState, jumpingState, movingJumpState, climbingState, endClimbingState}
	public characterState currentCharacterState = characterState.idleState;
	
	//! Check to see if character is grounded
	protected bool CheckGrounded(Collider collider)
	{
#if UNITY_EDITOR
		isGrounded = Physics.Raycast(transform.position, -transform.up, collider.bounds.size.y/2);
		if(isGrounded)
			Log.R("player",transform.position,-transform.up,Color.red); 
#endif
		return Physics.Raycast(transform.position, -transform.up, collider.bounds.size.y/2);
	}
	
	//! Check to see if the character is moving
	protected bool CheckMoving()
	{
#if UNITY_EDITOR
		isMoving = (currentMovementSpeed > minMovementSpeed);
#endif
		return (currentMovementSpeed > minMovementSpeed);
	}
}
