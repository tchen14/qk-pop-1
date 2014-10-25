using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class CharacterController_2 : MonoBehaviour {

    // movement variables
    protected Vector3 targetDirection = Vector3.zero;
    [SerializeField] public float minMovementSpeed = 10f;
    [SerializeField] public float maxMovementSpeed = 20f;
    [SerializeField] public float acceleration = 7.5f;
    [SerializeField] protected float currentMovementSpeed;
    // jumping variables
    protected Vector3 jumpDirection = Vector3.zero;
    [SerializeField] public float maxJumpingHeight = 0.5f;
    [SerializeField] public float airMovementSpeedPercentage = 0.1f;
    // climbing variables
    [SerializeField] public float maxClimbingSpeed = 10f;
    [SerializeField] public float minClimbingSpeed = 5f;
    [SerializeField] protected float currentClimbingSpeed;

    // used only to see status in inspector; DO NOT USE!
    [SerializeField] public bool isMoving;
    // used only to see status in inspector; DO NOT USE!
    [SerializeField] public bool isGrounded;

    // character states
    public enum characterState { idleState, walkingState, runningState, jumpingState, movingJumpState, climbingState}
    public characterState currentCharacterState = characterState.idleState;

    // player rotation speed
    public float rotationSpeed = 3.5f;

    // check to see if player is grounded
    public bool groundedCheck (Collider collider) {
        if (Physics.Raycast (transform.position, -transform.up, collider.bounds.size.y/2)) {
            Debug.DrawRay (transform.position,-transform.up,Color.red);
            isGrounded = true;
            return true;
        } else {
            isGrounded = false;
            return false;
        }
    }

    // collision detection---NOT WORKING---NOT CURRENTLY NECESSARY
    public bool isColliding (Collider collider) {
        // --WARNING!-- last variable, calculation of length of ray cast does not match actual size of collider
        if (Physics.Raycast (transform.position, transform.forward, collider.bounds.size.y*1.67f)) {
            Debug.DrawRay (transform.position,transform.forward,Color.red);
            //Debug.DrawRay(transform.position,transform.up,Color.red);
            isMoving = false;
            return true;
        } else {
            isMoving = true;
            return false;
        }
    }

    // Use this for initialization
    void Start() {
        currentMovementSpeed = minMovementSpeed;
        currentClimbingSpeed = minClimbingSpeed;
    }

}
