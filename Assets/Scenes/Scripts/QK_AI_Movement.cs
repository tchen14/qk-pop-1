using UnityEngine;
using System.Collections;

public class QK_AI_Movement : MonoBehaviour {

	float moveSpeedMultiplier = 1;
	float stationaryTurnspeed = 180;
	float movingTurnspeed = 360;

	bool onGround;

	Animator anim;
	Rigidbody rigidBody;

	Vector3 moveInput; //movement vector
	float turnAmount; //calculated amount to make a turn
	float forwardAmount; //calculated amount to move forward
	Vector3 velocity;

	float jumpPower = 10;

	IComparer rayHitComparer;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		SetupAnimator ();
	}

	public void Move(Vector3 move) {
		if (move.magnitude > 1) {
			move.Normalize();
		}

		this.moveInput = move;

		velocity = rigidBody.velocity;

		ConvertMoveInput ();
		ApplyExtraTurnRotation ();
		GroundCheck ();
		UpdateAnimator ();

	}

	void SetupAnimator () {
		anim = GetComponent<Animator> ();

		foreach (Animator childAnimator in GetComponentsInChildren<Animator>()) {
			if (childAnimator != anim) {
				anim.avatar = childAnimator.avatar;
				Destroy(childAnimator);
				break;
			}
		}
	}

	void OnAnimatorMove() {
		if (onGround && Time.deltaTime > 0) {
			//check the position of the model each frame
			Vector3 v = (anim.deltaPosition * moveSpeedMultiplier)/Time.deltaTime;

			v.y = rigidBody.velocity.y;
			rigidBody.velocity = v;
		}
	}

	void ConvertMoveInput() {
		Vector3 localMove = transform.InverseTransformDirection (moveInput);
		
		turnAmount = Mathf.Atan2 (localMove.x, localMove.z);
		forwardAmount = localMove.z;
	}

	void UpdateAnimator() {
		anim.applyRootMotion = true;

		anim.SetFloat ("Forward", forwardAmount, 0.1f, Time.deltaTime);
		anim.SetFloat ("Turn", turnAmount, 0.1f, Time.deltaTime);
	}

	void ApplyExtraTurnRotation(){
		float turnSpeed = Mathf.Lerp (stationaryTurnspeed, movingTurnspeed, forwardAmount);
		transform.Rotate (0, turnAmount * turnSpeed * Time.deltaTime, 0);
	}

	void GroundCheck() {
		Ray ray = new Ray (transform.position + Vector3.up * .1f, -Vector3.up);

		RaycastHit[] hits = Physics.RaycastAll (ray, .5f);
		rayHitComparer = new RayHitComparer ();

		System.Array.Sort (hits, rayHitComparer);

		if (velocity.y < jumpPower * .5f) {
			onGround = false;
			rigidBody.useGravity = true;

			foreach(var hit in hits) {
				if(!hit.collider.isTrigger) {
					if(velocity.y <= 0) {
						rigidBody.position = Vector3.MoveTowards(rigidBody.position, hit.point, Time.deltaTime * 5);
					}

					onGround = true;
					rigidBody.useGravity = false;

					break;
				}
			}
		}
	}

	void Jumping(){
		anim.SetTrigger ("Jump");
		rigidBody.AddForce (Vector3.up * jumpPower, ForceMode.Impulse);
	}

	class RayHitComparer: IComparer {
		public int Compare(object x, object y) {
			return ((RaycastHit)x).distance.CompareTo (((RaycastHit)y).distance);
		}
	}
}
