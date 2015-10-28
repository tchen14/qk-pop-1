using UnityEngine;
using System.Collections;

public class QK_Controller : MonoBehaviour {

	public static CharacterController CharacterController;
	public static QK_Controller Instance;

	public enum CharacterState {Idle, Walk, Run, Sprint, Jump, Crouch}
	private CharacterState _state;

	// Use this for initialization
	void Awake () {
		CharacterController = GetComponent ("CharacterController") as CharacterController;
		Instance = this;
		_state = CharacterState.Idle;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (PoPCamera.instance == null)
			return;

		GetLocomotionInput ();

		HandleActionInput ();

		QK_Character_Movement.Instance.UpdateMotor ();
	}

	void GetLocomotionInput() {
		var deadZone = 0.1f;

		QK_Character_Movement.Instance.VerticalVelocity = QK_Character_Movement.Instance.moveVector.y;
		QK_Character_Movement.Instance.moveVector = Vector3.zero;

		if (InputManager.input.MoveVerticalAxis() > deadZone || InputManager.input.MoveVerticalAxis() < deadZone) {
			if (InputManager.input.MoveVerticalAxis() > 0) {
			QK_Character_Movement.Instance.moveVector += new Vector3 (0, 0, InputManager.input.MoveVerticalAxis());
			}
			if (InputManager.input.MoveVerticalAxis() < 0) {
				QK_Character_Movement.Instance.moveVector += new Vector3 (0, 0, -InputManager.input.MoveVerticalAxis());
			}
		}

		if (InputManager.input.MoveHorizontalAxis() > deadZone || InputManager.input.MoveHorizontalAxis() < deadZone) {
			if (InputManager.input.MoveHorizontalAxis() > 0) {
				QK_Character_Movement.Instance.moveVector += new Vector3 (0, 0, InputManager.input.MoveHorizontalAxis());
			}
			if (InputManager.input.MoveHorizontalAxis() < 0) {
				QK_Character_Movement.Instance.moveVector += new Vector3 (0, 0, -InputManager.input.MoveHorizontalAxis());
			}
		}

		QK_Animator.Instance.DetermineCurrentMoveDirection ();
	}

	void HandleActionInput () {
		if (InputManager.input.isJumping()) {
			Jump ();
			_state = CharacterState.Jump;
		}
	}

	void Jump() {
		QK_Character_Movement.Instance.Jump ();
	}
}
