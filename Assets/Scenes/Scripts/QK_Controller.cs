using UnityEngine;
using System.Collections;

public class QK_Controller : MonoBehaviour {

	public static CharacterController CharacterController;
	public static QK_Controller Instance;

	// Use this for initialization
	void Awake () {
		CharacterController = GetComponent ("CharacterController") as CharacterController;
		Instance = this;
		QK_Camera.UseExistingOrCreateNewMainCamera ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Camera.main == null)
			return;

		GetLocomotionInput ();

		HandleActionInput ();

		QK_Character_Movement.Instance.UpdateMotor ();
	}

	void GetLocomotionInput() {
		var deadZone = 0.1f;

		QK_Character_Movement.Instance.VerticalVelocity = QK_Character_Movement.Instance.moveVector.y;
		QK_Character_Movement.Instance.moveVector = Vector3.zero;

		if (Input.GetAxis ("Vertical") > deadZone || Input.GetAxis ("Vertical") < deadZone) {
			QK_Character_Movement.Instance.moveVector += new Vector3 (0, 0, Input.GetAxis("Vertical"));
		}

		if (Input.GetAxis ("Horizontal") > deadZone || Input.GetAxis ("Horizontal") < deadZone) {
			QK_Character_Movement.Instance.moveVector += new Vector3 (Input.GetAxis("Horizontal"), 0 , 0);
		}
	}

	void HandleActionInput () {
		if (Input.GetButton ("Jump")) {
			Jump ();
		}
	}

	void Jump() {
		QK_Character_Movement.Instance.Jump ();
	}
}
