using UnityEngine;
using System.Collections;

//! InputType responsible for normal QK in game movement controls
public class GameInputType : InputType {

	// TO_DO set variables from json file; for mappable keys
	protected string forward = "w";
	protected string backward = "s";
	protected string left = "a";
	protected string right = "d";
	protected string action = "f";
	protected string sprint = "left shift";
	protected string crouch = "left ctrl";
	protected string jump = "space";
	protected string target = "Mouse1";
	protected string cameraReset = "`";
	protected string qAbility1 = "q";
	protected string qAbility2 = "e";


	public override int VerticalAxis() {
			if(Input.GetKey(forward) && Input.GetKey(backward))
				return 0;
			else if(Input.GetKey(forward)){
				return 1;
			}	
			else if(Input.GetKey(backward))
				return -1;
			else if(Input.GetAxis("Vertical")>0) {
				return 1;
			} 
			else if(Input.GetAxis("Vertical") < 0) {
				return -1;
			}
		return 0;
	}
	public override int HorizontalAxis() {
			if(Input.GetKey(left) && Input.GetKey(right))
				return 0;
			else if(Input.GetKey(right))
				return 1;
			else if(Input.GetKey(left))
				return -1;
			else if(Input.GetAxis("Horizontal") > 0) {
				return 1;
			} else if(Input.GetAxis("Horizontal") < 0) {
				return -1;
			}
		return 0;
	}

	public override bool isCrouched() {
		if(Input.GetButton("B Button")) {
			return true;
		}
		return Input.GetKey(crouch);
	}
	public override bool isSprinting() {
		if(Input.GetButton("Y Button")) {
			return true;
		}
		return Input.GetKey(sprint);
	}
	public override bool isJumping() {
		if(Input.GetButton("A Button")) {
			return true;
		}
		return Input.GetKey(jump);
	}
	public override bool isActionPressed() {
		if(Input.GetButton("X Button")) {
			return true;
		}
		return Input.GetKey(action);
	}
	public override bool isTargetPressed() {
		if(Input.GetButton("RB")) {
			return true;
		}
		return Input.GetKey(target);
	}
	public override bool isCameraReset() {
		if(Input.GetButton("LB")) {
			return true;
		}
		return Input.GetKey(cameraReset);
	}
	public override int isQAbility() {
		if(Input.GetAxis("D-Pad Hor") > 0) {
			return 3;
		} 
		else if(Input.GetAxis("D-Pad Hor") < 0) {
			return 1;
		}
		if(Input.GetAxis("D-Pad Vert") > 0) {
			return 2;
		} 
		else if(Input.GetAxis("D-Pad Vert") < 0) {
			return 4;
		}
		else if(Input.GetKey(qAbility1)){
			return 1;
		}
		else if(Input.GetKey(qAbility2)){
			return 2;
		}
		return 0;
	}
}
