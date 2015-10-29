using UnityEngine;
using System.Collections;

public class QK_Character_Movement : MonoBehaviour {

	public static QK_Character_Movement Instance;

	public float moveSpeed = 10f;
	public float jumpSpeed = 6f;
	public float gravity = 21f;
	public float terminalVelocity = 20f;

	public Vector3 moveVector { get; set; }
	public float VerticalVelocity { get; set; }

	// This is for Slide if implemented
	public float slideTheshold = 0.6f;
	public float MaxControllableSlideMagnitude = 0.4f;
	private Vector3 slideDirection;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	
	// Update is called once per frame
	public void UpdateMotor () {
		SnapAlignCharacterWithCamera ();
		ProcessMotion ();
	}

	void ProcessMotion(){
		//Transform move into World Space
		moveVector = transform.TransformDirection (moveVector);

		//Normalize move if Magnitude > 1
		if (moveVector.magnitude > 1) {
			moveVector = Vector3.Normalize (moveVector);
		}

		//Apply Sliding if applicable
		ApplySlide ();

		//Multiply move by MoveSpeed
		moveVector *= moveSpeed;

		// Reapply Vertical Velocity to MoveVector.y
		moveVector = new Vector3 (moveVector.x, VerticalVelocity, moveVector.z);

		//Apply Gravity
		ApplyGravity ();

		//Move Quincy in World Space
		QK_Controller.CharacterController.Move (moveVector * Time.deltaTime);
	}

	void ApplyGravity () {
		if (moveVector.y > -terminalVelocity) {
			moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);
		}

		if (QK_Controller.CharacterController.isGrounded && moveVector.y < -1) {
			moveVector = new Vector3 (moveVector.x, -1, moveVector.z);
		}
	}

	void ApplySlide (){
		if (!QK_Controller.CharacterController.isGrounded)
			return;

		slideDirection = Vector3.zero;

		RaycastHit hitInfo;

		if (Physics.Raycast (transform.position + Vector3.up, Vector3.down, out hitInfo)) {
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

	public void Jump() {
		if (QK_Controller.CharacterController.isGrounded)
			VerticalVelocity = jumpSpeed;
	}

	void SnapAlignCharacterWithCamera () {
		if (moveVector.x != 0 || moveVector.z != 0) {
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 
			                                      Camera.main.transform.eulerAngles.y,
			                                      transform.eulerAngles.z);
		}
	}

}
