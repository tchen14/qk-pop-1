using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

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
	protected KeyCode target = KeyCode.Mouse1;
	protected KeyCode cameraReset = KeyCode.Mouse2;
	protected string abilityEquip = "tab";
	protected string notifications = "l";
	protected string compass = "k";
	protected string journal = "j";
	protected string qAbility1 = "q";
	protected bool nextTarget {get { return Input.GetAxis("Mouse ScrollWheel") > 0; } }
	protected bool previousTarget { get { return Input.GetAxis("Mouse ScrollWheel") < 0; } }
	protected string pause = "escape";
	//_______________________________
	protected string controllerHor = "Horizontal";
	protected string controllerVert = "Vertical";
	protected string controllerRightHor = "Right Stick Horizontal";
	protected string controllerRightVert = "Right Stick Vertical";
	protected string controllerCrouch = "Left Stick Press";
	protected string controllerJump = "A Button";
	protected string controllerAction = "B Button";
	protected string controllerSprint = "RB";
	protected string controllerTarget = "LB";
	protected string controllerNextTarget = "Right Trigger";
	protected string controllerPreviousTarget = "Left Trigger";
	protected string controllerCameraReset = "Right Stick Press";
	protected string dPadHorString = "D-Pad Hor";
	protected string dPadVertString = "D-Pad Vert";
	protected bool dPadUp { get { return Input.GetAxis(dPadVertString) > 0; } }
	protected bool dPadDown { get { return Input.GetAxis(dPadVertString) < 0; } }
	protected bool dPadRight { get { return Input.GetAxis(dPadHorString) > 0; } }
	protected bool dPadLeft { get { return Input.GetAxis(dPadHorString) < 0; } }

	const string controllerInputFilePath = "/controllerInput.json";

	void Start(){
		//string json = System.IO.File.ReadAllText (Application.dataPath + controllerInputFilePath);
	}

	public override float CameraVerticalAxis() {
		if(Input.GetAxis("Mouse Y") != 0)
			return Input.GetAxis("Mouse Y");
		else if(Input.GetAxis(controllerVert) != 0)
			return Input.GetAxis(controllerVert);
			
		if(Input.GetAxis("Mouse Y") > 0 || Input.GetAxis(controllerVert)>0){
			return 1;
		}else if(Input.GetAxis("Mouse Y") < 0 || Input.GetAxis(controllerVert) < 0){
			return -1;
		}
		return 0;
	}

	public override float CameraHorizontalAxis() {
		if(Input.GetAxis("Mouse X") != 0)
			return Input.GetAxis("Mouse X");
		else if(Input.GetAxis(controllerHor) != 0)
			return Input.GetAxis(controllerHor);
			
		if(Input.GetAxis("Mouse X") > 0 || Input.GetAxis(controllerHor)>0){
			return 1;
		}else if(Input.GetAxis("Mouse X") < 0 || Input.GetAxis(controllerHor) < 0){
			return -1;
		}
		return 0;
	}

	public override int MoveVerticalAxis() {
		if(Input.GetKey(forward) && Input.GetKey(backward))
			return 0;
		else if(Input.GetKey(forward)) {
			return 1;
		} else if(Input.GetKey(backward))
			return -1;
		//todo: fix this shit
//		else if(Input.GetAxis(controllerRightVert) > 0) {
//			return 1;
//		} else if(Input.GetAxis(controllerRightVert) < 0) {
//			return -1;
//		}
		return 0;
	}
	public override int MoveHorizontalAxis() {
		if(Input.GetKey(left) && Input.GetKey(right))
			return 0;
		else if(Input.GetKey(right))
			return 1;
		else if(Input.GetKey(left))
			return -1;
		//todo: fix this shit
//		else if(Input.GetAxis(controllerRightHor) > 0) {
//			return 1;
//		} else if(Input.GetAxis(controllerRightHor) < 0) {
//			return -1;
//		}
		return 0;
	}

	public override bool isCrouched() {
		//todo: fix this shit
//		if(Input.GetButton(controllerCrouch)) {
//			return true;
//		}
		return Input.GetKey(crouch);
	}
	public override bool isSprinting() {
		//todo: fix this shit
//		if(Input.GetButton(controllerSprint)) {
//			return true;
//		}
		return Input.GetKey(sprint);
	}
	public override bool isJumping() {
		if(Input.GetButton(controllerJump)) {
			return true;
		}
		return Input.GetKey(jump);
	}
	public override bool isActionPressed() {
		if(Input.GetButton(controllerAction)) {
			return true;
		}
		return Input.GetKey(action);
	}
	public override bool isTargetPressed() {
	//todo: fix this
//		if(Input.GetButton(controllerTarget)) {
//			return true;
//		}
		return Input.GetKeyDown(target);
	}
	public override bool isCameraResetPressed() {
	//todo: fix this
//		if(Input.GetButton(controllerCameraReset)) {
//			return true;
//		}
		return Input.GetKeyDown(cameraReset);
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
	public override int CameraScrollTarget() {
		if(!InputManager.changeSkills)
			return ScrollTarget();
		else
			return 0;
	}
	public override int ScrollTarget() {
	//todo: fix this
		if(Input.GetAxis("Mouse ScrollWheel") > 0 /*|| Input.GetAxis(controllerNextTarget)>0*/){
			return 1;
		}else if(Input.GetAxis("Mouse ScrollWheel") < 0 /*|| Input.GetAxis(controllerPreviousTarget) < 0*/){
			return -1;
		}
		return 0;
	}

	//Save keyboard controls
	public override void SaveInput(){
		Dictionary<string, string> inputs = new Dictionary<string, string>();
		inputs.Add ("forward", forward);
		inputs.Add ("backward", backward);
		inputs.Add ("left", left);
		inputs.Add ("right", right);
		inputs.Add ("action", action);
		inputs.Add ("sprint", sprint);
		inputs.Add ("crouch", crouch);
		inputs.Add ("cover", cover);
		inputs.Add ("climb", climb);
		inputs.Add ("jump", jump);
		inputs.Add ("target", target.ToString());
		inputs.Add ("cameraReset", cameraReset.ToString());
		inputs.Add ("abilityEquip", abilityEquip);
		inputs.Add ("notifications", notifications);
		inputs.Add ("compass", compass);
		inputs.Add ("journal", journal);
		inputs.Add ("qAbility1", qAbility1);
		inputs.Add ("pause", pause);

		InputSerialization.SaveInput (inputs);
	}

	//Load keyboard controls
	public override void LoadInput(){
		Dictionary<string, string> inputs = InputSerialization.LoadInput ();
		forward = inputs ["forward"];
		backward = inputs ["backward"];
		left = inputs ["left"];
		right = inputs ["right"];
		action = inputs ["action"];
		sprint = inputs ["sprint"];
		crouch = inputs ["crouch"];
		cover = inputs ["cover"];
		climb = inputs ["climb"];
		jump = inputs ["jump"];
		//todo: fix this
		//target = inputs ["target"];
		//cameraReset = inputs ["cameraReset"];
		abilityEquip = inputs ["abilityEquip"];
		notifications = inputs ["notifications"];
		compass = inputs ["compass"];
		journal = inputs ["journal"];
		qAbility1 = inputs ["qAbility1"];
		pause = inputs ["pause"];

	}

}
