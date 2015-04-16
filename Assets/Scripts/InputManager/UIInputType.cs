using UnityEngine;
using System.Collections;

//! InputType responsible for UI interaction
public class UIInputType : InputType {
	
	// TO_DO set variables from json file; for mappable keys
	protected string cancel = "escape";
	protected string forward = "w";
	protected string backward = "s";
	protected string left = "a";
	protected string right = "d";
	protected string action = "f";
	protected string crouch = "left ctrl";
	protected string jump = "space";

	public override int VerticalValue(string keyPressed) {
		float axisValue = Input.GetAxis("Vertical");
		if(axisValue > 0) {
			return 1;
		} 
		else if(axisValue < 0) {
			return -1;
		} 
		else return 0;
	}

	public override int HorizontalValue(string keyPressed) {
		float axisValue = Input.GetAxis("Horizontal");
		if(axisValue > 0) {
			return 1;
		} else if(axisValue < 0) {
			return -1;
		} else return 0;
	}

	public override bool CancelPressed() {
		return Input.GetKey(cancel);
	}

	public override bool ActionPressed() {
		return Input.GetKey(action);
	}

	public override bool JumpPressed() {
		return Input.GetKey(jump);
	}

	public override bool CrouchPressed() {
		return Input.GetKey(crouch);
	}
}
