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
