using UnityEngine;
using System.Collections;

//! InputType responsible for normal QK in game movement controls
public class GameInputType : InputType {

	// TO_DO set variables from json file; for mappable keys
	protected string forward = "w";
	protected string backward = "s";
	protected string left = "a";
	protected string right = "d";
	protected string action = "return";
	protected string sprint = "left shift";
	protected string crouch = "left ctrl";
	protected string jump = "space";

	public int VerticalAxis() {
		if(InputManager.activeInputType == "GameInputManager") {
			if(Input.GetKey(forward) && Input.GetKey(backward))
				return 0;
			if(Input.GetKey(forward))
				return 1;
			if(Input.GetKey(backward))
				return -1;
		}
		return 0;
	}
	public bool HorizontalAxis() {
		if(InputManager.activeInputType == "GameInputManager") {
			return true;
		}
		return false;
	}

	public bool isCrouched() {
		if(InputManager.activeInputType == "GameInputManager") {
			return true;
		} 
			return false;
	}
	public bool isSprinting() {
		if(InputManager.activeInputType == "GameInputManager") {
			return true;
		}
		return false;
	}
	public bool isJumping() {
		if(InputManager.activeInputType == "GameInputManager") {
			return true;
		}
		return false;
	}
	public bool isActionPressed() {
		if(InputManager.activeInputType == "GameInputManager") {
			return true;
		}
		return false;
	}
}
