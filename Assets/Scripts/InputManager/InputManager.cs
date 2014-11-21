using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour 
{
	private string currentInputState;
	private const int inputStateCount = 3;
	//private string[] inputStateArray;
	
	// TO_DO set variables from json file; for mappable keys
	protected string forward = "w";
	protected string backward = "s";
	protected string left = "a";
	protected string right = "d";
	protected string action = "return";
	protected string sprint = "left shift";
	protected string crouch = "left ctrl";
	protected string jump = "space";
	
	// Use this for initialization
	void Awake () 
	{
		//inputStateArray = new string[inputStateCount] {"MainMenuInputManager", "GameInputManager", "KeyboardInputManager"};
	}
	
	public bool switchState(string desiredState)
	{
		bool stateChanged = false;
		switch (desiredState)
		{
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
