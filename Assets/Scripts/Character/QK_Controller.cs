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
	}

	void GetLocomotionInput() {
		var deadZone = 0.1f;

		QK_Character_Movement.Instance.verticalVelocity = QK_Character_Movement.Instance.moveVector.y;
		QK_Character_Movement.Instance.moveVector = Vector3.zero;

        if (InputManager.input.MoveVerticalAxis() > deadZone || InputManager.input.MoveVerticalAxis() < deadZone)
        {
            QK_Character_Movement.Instance.moveVector += new Vector3(0, 0, InputManager.input.MoveVerticalAxis());
		}

        if (InputManager.input.MoveHorizontalAxis() > deadZone || InputManager.input.MoveHorizontalAxis() < deadZone)
        {
            QK_Character_Movement.Instance.moveVector += new Vector3(InputManager.input.MoveHorizontalAxis(), 0, 0);
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
