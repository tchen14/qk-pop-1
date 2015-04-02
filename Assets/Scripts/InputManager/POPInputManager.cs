using UnityEngine;
using System.Collections.Generic;

//Edited by KD

//Sealed because needs to be accessed from other input managers, and change variables

public sealed class POPInputManager : MonoBehaviour
{
	//Singleton variable
	public static POPInputManager instance {
		get { return instance ?? (instance = GameObject.FindObjectOfType<POPInputManager>()); } 
		private set{
		} }
	#pragma warning disable 0414
	public static string currentInputState;
	#pragma warning restore 0414
	private const int inputStateCount = 3;
	//private string[] inputStateArray;
	
	// Use this for initialization
	void Awake()
	{
		
		currentInputState = "MainMenuInputManager";
		//inputStateArray = new string[inputStateCount] {"MainMenuInputManager", "GameInputManager", "KeyboardInputManager"};
	}

	public bool switchState(string desiredState) {
		bool stateChanged = false;
		switch(desiredState) {
			case "MainMenuInputManager":
				currentInputState = "MainMenuInputManager";
				stateChanged = true;
				//			Camera.main.GetComponent<MainMenuInputManager>().enabled = true;
				//			Camera.main.GetComponent<GameInputManager>().enabled = false;
				Camera.main.GetComponent<KeyboardInputManager>().enabled = false;
				break;
			case "gameInputManager":
				currentInputState = "GameInputManager";
				stateChanged = true;
				//			Camera.main.GetComponent<MainMenuInputManager>().enabled = false;			
				//			Camera.main.GetComponent<GameInputManager>().enabled = true;
				Camera.main.GetComponent<KeyboardInputManager>().enabled = false;
				break;
			case "keyboardInputManager":
				currentInputState = "KeyboardInputManager";
				stateChanged = true;
				//			Camera.main.GetComponent<MainMenuInputManager>().enabled = false;			
				//			Camera.main.GetComponent<GameInputManager>().enabled = false;
				Camera.main.GetComponent<KeyboardInputManager>().enabled = true;
				break;
		}
		return stateChanged;
	}
	
}
