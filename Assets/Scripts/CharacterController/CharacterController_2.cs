using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
/*!
 *	Base class for a characterController. The _2 is appanded due to Unity's built in CharacterController class
 */
public abstract class CharacterController_2 : MonoBehaviour {

	//! Overlord variables
	protected CapsuleCollider myCollider;

	// Character action states
	public enum actionState { prepState, loopState }
	[ReadOnly]
	public actionState myActionState = actionState.prepState;

	// Movement variables
	protected Vector3 targetDirection = Vector3.zero;
	protected float currentWalkingSpeed;
	public const float rotationSpeed = 3.5f;
	protected const float minWalkingSpeed = 20f;
	protected const float maxWalkingSpeed = 30f;
	protected const float runningSpeedMod = 1.2f;
	protected const float moveAcceleration = 7.5f;
	protected const int movePow = 3;
	protected const float moveDeceleration = 20.0f;

	// Jumping variables
	[ReadOnly]
	public bool grounded = false; //Only public for inspector. Should be protected
	protected const float maxJumpingHeight = 10.0f;
	protected const float airMovementSpeedPercentage = 0.1f;

	// Crouching variables
	protected const float crouchingMod = 0.5f;

	// Climbing variables
	[HideInInspector]
	public float currentClimbingSpeed;
	protected const float minClimbingSpeed = 5f;
	protected const float maxClimbingSpeed = 10f;

	// Action variables
	[ReadOnly]
	public bool actionAvaiable; //Only public for inspector. Should be protected
	public bool actionForward = true;
	protected PlayerActionPath actionComponent;
	new protected Rigidbody rigidbody;

	// Modifiers variables
	[ReadOnly]
	public bool isRunning = false; //Only public for inspector. Should be protected
	[ReadOnly]
	public bool isCrouched = false; //Only public for inspector. Should be protected

	public virtual void Start() {
		myCollider = GetComponent<CapsuleCollider>();
		rigidbody = GetComponent<Rigidbody>();

		currentWalkingSpeed = minWalkingSpeed;
		currentClimbingSpeed = minClimbingSpeed;
	}
#pragma warning disable 0219
	//! Detect if Character is grounded (true)
	void OnCollisionEnter(Collision collisionInfo) {
		foreach(ContactPoint d in collisionInfo.contacts) {
			if(d.thisCollider.GetType().ToString() == "UnityEngine.BoxCollider") {
				grounded = true;
				return;
			}
		}
	}

	//! Detect if Character is grounded (false)
	void OnCollisionExit(Collision collisionInfo) {
		foreach(ContactPoint d in collisionInfo.contacts) {
				grounded = false;
				return;
		}
	}
#pragma warning restore 0219

	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "PlayerAction") {
			actionAvaiable = true;
			actionComponent = collider.gameObject.GetComponent<PlayerActionPath>();

			//determine if player is closer to the sphere or the box collider
			bool forward = true;
			if (Vector3.SqrMagnitude(transform.position - collider.transform.position - collider.GetComponent<SphereCollider>().center) >
				Vector3.SqrMagnitude(transform.position - collider.transform.position - collider.GetComponent<BoxCollider>().center))
				forward = false;

			if (myActionState == actionState.prepState) {
				if (forward)
					actionForward = true;
				else
					actionForward = false;
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if(collider.tag == "PlayerAction") {
			actionAvaiable = false;
			//actionComponent = null; //Unset this in inherited class
		}
	}
}
