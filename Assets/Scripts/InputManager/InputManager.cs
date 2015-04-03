using UnityEngine;
using System.Collections.Generic;

//! Class manages different InputTypes and chooses one to be the active input type
public sealed class InputManager : MonoBehaviour
{
	//Singleton variable
	public static InputManager instance {
		get { return instance ?? (instance = GameObject.FindObjectOfType<InputManager>());} 
		private set{ }
	}
	public static string activeInputType = "MenuInput"; //!< Active inptytype, todo: make enum
	
	//!This might need to be reworked or something. don't know if the code is actually needed/useful
	public bool switchState(string desiredState) {
		bool stateChanged = false;
		switch(desiredState) {
			case "MenuInput":
				activeInputType = "MenuInput";
				stateChanged = true;
				//			Camera.main.GetComponent<MainMenuInputManager>().enabled = true;
				//			Camera.main.GetComponent<GameInputManager>().enabled = false;
				Camera.main.GetComponent<KeyboardInputType>().enabled = false;
				break;
			case "GameInput":
				activeInputType = "GameInput";
				stateChanged = true;
				//			Camera.main.GetComponent<MainMenuInputManager>().enabled = false;			
				//			Camera.main.GetComponent<GameInputManager>().enabled = true;
				Camera.main.GetComponent<KeyboardInputType>().enabled = false;
				break;
			case "KeyboardInput":
				activeInputType = "KeyboardInput";
				stateChanged = true;
				//			Camera.main.GetComponent<MainMenuInputManager>().enabled = false;			
				//			Camera.main.GetComponent<GameInputManager>().enabled = false;
				Camera.main.GetComponent<KeyboardInputType>().enabled = true;
				break;
		}
		return stateChanged;
	}
	
}
