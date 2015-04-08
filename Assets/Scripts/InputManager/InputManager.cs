using UnityEngine;
using System.Collections.Generic;
using Debug = FFP.Debug;

//! Class manages different InputTypes and chooses one to be the active input type
public sealed class InputManager : MonoBehaviour
{
	//Singleton variable
	public static InputManager instance {
		get { return instance ?? (instance = GameObject.FindObjectOfType<InputManager>());} 
		private set{ }
	}
	public InputType activeInputType; //!< Active input type, todo: make enum

	public Dictionary<string, InputType> inputs = new Dictionary<string, InputType>();

	void Start() {
		inputs.Add("UIInputType", this.gameObject.AddComponent<UIInputType>());
		inputs.Add("GameInputType", this.gameObject.AddComponent<GameInputType>());
		inputs.Add("KeyboardInputType", this.gameObject.AddComponent<KeyboardInputType>());

		if(inputs.Count > 0)
			ChangeInputType("UIInputType");
		else
			Debug.Error("input", "InputManager.inputs is empty.");

		ChangeInputType("GameInputType");
	}

	public void ChangeInputType(string inputType) {
		if(inputs.ContainsKey(inputType))
			activeInputType = inputs[inputType];
	}
	
}
