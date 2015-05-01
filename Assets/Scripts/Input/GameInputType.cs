using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Debug = FFP.Debug;

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
	//Controller input list
	protected string controllerHor = "Xbox L Hor";
	protected string controllerVert = "Xbox L Vert";
	protected string controllerRightHor = "Xbox R Hor";
	protected string controllerRightVert = "Xbox R Vert";
	protected string controllerCrouch = "Xbox L3";
	protected string controllerCamReset = "Xbox R3";
	protected string controllerJump = "Xbox A Button";
	protected string controllerAction = "Xbox B Button";
	protected string controllerStart = "Xbox Start Button";
	protected string controllerSprint = "Xbox RB";
	protected string controllerTarget = "Xbox LB";
	protected string controllerNextAbility = "Xbox RT";
	protected string controllerPreviousAbility = "Xbox LT";
	protected string dPadVertString = "Xbox Dpad Vert";
	protected string dPadHorString = "Xbox Dpad Hor";
	protected bool dPadUp { get { return Input.GetAxis(dPadVertString) > 0; } }
	protected bool dPadDown { get { return Input.GetAxis(dPadVertString) < 0; } }
	protected bool dPadRight { get { return Input.GetAxis(dPadHorString) > 0; } }
	protected bool dPadLeft { get { return Input.GetAxis(dPadHorString) < 0; } }


	void Start(){
		if(!loadListFromFile(StringManager.INPUTKEYS)) {
			Debug.Log("input", "JSON file did not load");
		} else
			Debug.Log("input", "JSON file loaded");
	}

	public override float CameraVerticalAxis() {
		if(Input.GetAxis("Mouse Y") != 0)
			return Input.GetAxis("Mouse Y");
		else if(Input.GetAxis(controllerRightVert) != 0)
			return Input.GetAxis(controllerRightVert);
			
		if(Input.GetAxis("Mouse Y") > 0 || Input.GetAxis(controllerRightVert)>0){
			return 1;
		}else if(Input.GetAxis("Mouse Y") < 0 || Input.GetAxis(controllerRightVert) < 0){
			return -1;
		}
		return 0;
	}

	public override float CameraHorizontalAxis() {
		if(Input.GetAxis("Mouse X") != 0)
			return Input.GetAxis("Mouse X");
		else if(Input.GetAxis(controllerRightHor) != 0)
			return Input.GetAxis(controllerRightHor);
			
		if(Input.GetAxis("Mouse X") > 0 || Input.GetAxis(controllerRightHor)>0){
			return 1;
		}else if(Input.GetAxis("Mouse X") < 0 || Input.GetAxis(controllerRightHor) < 0){
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
		else if(Input.GetAxis(controllerVert) > 0) {
			return 1;
		} else if(Input.GetAxis(controllerVert) < 0) {
			return -1;
		}
		return 0;
	}
	public override int MoveHorizontalAxis() {
		if(Input.GetKey(left) && Input.GetKey(right))
			return 0;
		else if(Input.GetKey(right))
			return 1;
		else if(Input.GetKey(left))
			return -1;
		else if(Input.GetAxis(controllerHor) > 0) {
			return 1;
		} else if(Input.GetAxis(controllerHor) < 0) {
			return -1;
		}
		return 0;
	}
	
	public override bool AbilityPressed() {
		if(Input.GetButton(controllerAction)) {
			return true;
		}
		return Input.GetKey(qAbility1);
	}

	public override bool isCrouched() {
		if(Input.GetButton(controllerCrouch)) {
			return true;
		}
		return Input.GetKey(crouch);
	}
	public override bool isSprinting() {
		if(Input.GetButton(controllerSprint)) {
			return true;
		}
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
		if(Input.GetButton(controllerTarget)) {
			return true;
		}
		return Input.GetKeyDown(target);
	}
	public override bool isCameraResetPressed() {
		if(Input.GetButton(controllerCamReset)) {
			return true;
		}
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
		if(Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis(controllerNextAbility)>0){
			return 1;
		}else if(Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetAxis(controllerPreviousAbility) < 0){
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
		target = (KeyCode) System.Enum.Parse(typeof(KeyCode),inputs ["target"]);
		cameraReset = (KeyCode) System.Enum.Parse(typeof(KeyCode),inputs ["cameraReset"]);
		abilityEquip = inputs ["abilityEquip"];
		notifications = inputs ["notifications"];
		compass = inputs ["compass"];
		journal = inputs ["journal"];
		qAbility1 = inputs ["qAbility1"];
		pause = inputs ["pause"];

	}
	
	//JSON load stuff
	private bool loadListFromFile(string filePath){
		if(!System.IO.File.Exists(Application.dataPath + filePath)) {
			Debug.Log("input", "File does not exist: " + Application.dataPath + filePath);
			return false;
		}
		string json = System.IO.File.ReadAllText(Application.dataPath + filePath);
		return loadListFromJson(json);
	}

	private bool loadListFromJson(string json){
		JSONNode inputs = JSON.Parse (json);
		if(inputs == null) {
			Debug.Log("input", "Json file is empty");
			return false;
		}
		JSONNode cont = inputs ["controller"];

		//If game is running on the PS4
		if (Application.platform == RuntimePlatform.PS4) {
			JSONNode contInputs = cont ["PS4"];
			controllerHor = contInputs[0].Value;
			controllerVert = contInputs[1].Value;
			controllerRightHor = contInputs[2].Value;
			controllerRightVert = contInputs[3].Value;
			controllerCrouch = contInputs[4].Value;
			controllerCamReset = contInputs[5].Value;
			controllerJump = contInputs[6].Value;
			controllerAction = contInputs[7].Value;
			controllerStart = contInputs[8].Value;
			controllerSprint = contInputs[9].Value;
			controllerTarget = contInputs[10].Value;
			controllerNextAbility = contInputs[11].Value;
			controllerPreviousAbility = contInputs[12].Value;
			dPadVertString = contInputs[13].Value;
			dPadHorString = contInputs[14].Value;
			Debug.Log("input", "PS4 Input Loaded");
		}
		return true;
	}

}
