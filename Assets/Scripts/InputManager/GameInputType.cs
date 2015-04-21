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
	protected string cover = "e";
	protected string climb = "q";
	protected string jump = "space";
	protected string target = "Mouse1";
	protected string cameraReset = "Mouse2";
	protected string abilityEquip = "tab";
	protected string notifications = "l";
	protected string compass = "k";
	protected string journal = "j";
	protected string qAbility1 = "q";
	protected string nextAbility = "scroll up";
	protected string previousAbility = "scroll down";
	protected string nextTarget = "scroll up";
	protected string previousTarget = "scroll down";
	protected string pause = "escape";
	//_______________________________
	protected string controllerVert = "Vertical";
	protected string controllerHor = "Horizontal";
	protected string controllerRightVert = "Right Stick Vertical";
	protected string controllerRightHor = "Right Stick Horizontal";
	protected string controllerCrouch = "X Button";
	protected string controllerJump = "A Button";
	protected string controllerAbilityEquip = "D-Pad Up";
	protected string controllerNextAbility = "Right Trigger";
	protected string controllerPreviousAbility = "Left Trigger";
	protected string dPadVertString = "D-Pad Vert";
	protected string dPadHorString = "D-Pad Vert";
	protected bool dPadUp { get { return Input.GetAxis(dPadVertString) > 0; } }
	protected bool dPadDown { get { return Input.GetAxis(dPadVertString) < 0; } }
	protected bool dPadRight { get { return Input.GetAxis(dPadHorString) > 0; } }
	protected bool dPadLeft { get { return Input.GetAxis(dPadHorString) < 0; } }

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

	public override int RightVerticalAxis() {
		if(Input.GetKey(forward) && Input.GetKey(backward))
			return 0;
		else if(Input.GetKey(forward)) {
			return 1;
		} else if(Input.GetKey(backward))
			return -1;
		else if(Input.GetAxis("Right Stick Vertical") > 0) {
			return 1;
		} else if(Input.GetAxis("Right Stick Vertical") < 0) {
			return -1;
		}
		return 0;
	}
	public override int RightHorizontalAxis() {
		if(Input.GetKey(left) && Input.GetKey(right))
			return 0;
		else if(Input.GetKey(right))
			return 1;
		else if(Input.GetKey(left))
			return -1;
		else if(Input.GetAxis("Right Stick Horizontal") > 0) {
			return 1;
		} else if(Input.GetAxis("Right Stick Horizontal") < 0) {
			return -1;
		}
		return 0;
	}

	public override bool isCrouched() {
		if(Input.GetButton("Left Stick Press")) {
			return true;
		}
		return Input.GetKey(crouch);
	}
	public override bool isSprinting() {
		if(Input.GetButton("RB")) {
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
		if(Input.GetButton("B Button")) {
			return true;
		}
		return Input.GetKey(action);
	}
	public override bool isTargetPressed() {
		if(Input.GetButton("LB")) {
			return true;
		}
		return Input.GetKey(target);
	}
	public override bool isCameraReset() {
		if(Input.GetButton("Right Stick Press")) {
			return true;
		}
		return Input.GetKey(cameraReset);
	}
	public override bool isAbilityEquip() {
		if(dPadUp) {
			return true;
		} 
		return Input.GetKey(abilityEquip);
	}
	public override bool isNotifications(){
		if(dPadDown) {
			return true;
		}
		return Input.GetKey(notifications);
	}
	public override bool isCompass() {
		if(dPadLeft) {
			return true;
		}
		return Input.GetKey(compass);
	}
	public override bool isJournal() {
		if(dPadRight) {
			return true;
		}
		return Input.GetKey(journal);
	}
}
