using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Debug = FFP.Debug;

/*! ----------------------------------------------------------------------------
 * Main hud control, contains functions for updating HUD information on the player's screen
 * these functions are designed to be called from whatever script needs to update them.
 * ----------------------------------------------------------------------------
 */
[EventVisible("UI")]
public class GameHUD : MonoBehaviour {
	#region singletonEnforcement
	private static GameHUD instance;
	public static GameHUD Instance {
		get {
			return instance;
		}
		private set { }
	}
	#endregion

#pragma warning disable 0219
#pragma warning disable 0414
	GameObject mainHUDCanvas;				//!<The canvas HUD is rendered on
	GameObject worldMapCanvas;				//!<All the game map elements
	GameObject gameMap;						//!<The map iamge on a plane
	GameObject player;						//!<reference to player
	GameObject pauseMenu;
	GameObject questBox;
	
	public int numOfAbilities;						//!<temporary int for number of abilities in game
	public GameObject[] hudAbilityIcons;			//!<Array of hud icons, set in inspector
	public bool abilitiesUp = false;
	public GameObject[] abilityWheelIcons;
	Animator abilityWheelAnchorAnim;

	List<GameObject> phoneAbilitiesAvailible;		//!<List containing hud phone abilties
	GameObject mapCam;								//!<Camera used for minimap
	static GameObject objectiveText;						//!<Objective Text UI element
	static GameObject dialogueBox, dialogueText, dialogueTitleText;

	GameObject[] mapLabels;							//!<Array of text taht appears on minimap

	GameObject middleAbilityIcon;					//!<Phone ability icon references
	GameObject rightAbilityIcon;
	GameObject leftAbilityIcon;
	GameObject[] abilityButtons;

	[ReadOnly]public int curAbility = 1;

	public bool skillsOpen = false;
	bool canSpin = false;
	GameObject skillWheel;
	GameObject closeMapButton;
	GameObject phoneButtons;
	GameObject mapElements;
	GameObject compassCameraPoint;					//!<Point at camera location used to calculate objective positions
	GameObject compass;
	GameObject slider;
	GameObject leftArrow;
	GameObject rightArrow;

	GameObject testObjective;

	void Awake() {
		#region singletonEnforcement
		if(instance == null) {
			instance = this;
		} else {
			Destroy(this.gameObject);
			Debug.Error("core", "Second GameHUD detected. Deleting gameOject.");
			return;
		}
		#endregion

		mainHUDCanvas = GameObject.Find("mainHUD");
		skillWheel = GameObject.Find("abilityWheel");
		abilityWheelAnchorAnim = GameObject.Find("AbilityWheelAnchor").GetComponent<Animator>();
		worldMapCanvas = GameObject.Find("worldMapCanvas");
		gameMap = GameObject.Find("mapBG");
		player = GameObject.Find("_Player");
		testObjective = GameObject.Find("testObjectiveCanvas");
		pauseMenu = GameObject.Find ("pauseMenu");
		pauseMenu.SetActive (false);
		questBox = GameObject.Find ("questBox");
		
		//!Turn on UI stuff
		worldMapCanvas.SetActive(true);
		skillWheel.SetActive(false);

		//!Fill mapLabels array
		mapLabels = GameObject.FindGameObjectsWithTag("worldMapLabel");
		closeMapButton = GameObject.Find("CloseMapButton");
		closeMapButton.SetActive(false);

		abilityButtons = GameObject.FindGameObjectsWithTag("abilityButton");

		//!Set mapcam reference
		mapCam = GameObject.Find("mapCam");
		//!Set compassCameraPoint reference
		compassCameraPoint = GameObject.Find("compassCameraPoint");
		compass = GameObject.Find("compassSlider");
		slider = compass.transform.FindChild ("Handle Slide Area").gameObject;
		slider.SetActive (false);
		leftArrow = compass.transform.FindChild ("leftArrow").gameObject;
		leftArrow.SetActive (false);
		rightArrow = compass.transform.FindChild ("rightArrow").gameObject;
		rightArrow.SetActive (false);

		//!Set objective text reference
		objectiveText = GameObject.Find("objectiveText");
		dialogueBox = GameObject.Find("SpeechBubble");
		dialogueText = GameObject.Find("speechPanelDialogueText");
		dialogueTitleText = GameObject.Find("speechPanelNameText");
		dialogueBox.SetActive(false);


		phoneButtons = GameObject.Find("PhoneButtons");
		mapElements = GameObject.Find("MapElements");
		mapElements.SetActive(false);

		//Testing for filling ability list
		List<string> tempAbList = new List<string>();
		tempAbList.Add("Push");
		tempAbList.Add("Pull");
		tempAbList.Add("Shock");

		//Initialize phone abilities list
		phoneAbilitiesAvailible = new List<GameObject>();

		fillAbilityList(tempAbList);
	}

	void Start() {
		//Place the ability buttons in the Phone Menu
		//SpawnHudAbilityIcons ();
		skillWheel.GetComponent<Animator>().speed = 0;
	}

	void Update() {
		//!This is for testing, call update map from player movements
		rotateMapObjects();

		//!Set the compass indicator
		setCompassValue(calculateObjectiveAngle(testObjective));

		//Testing
		if(InputManager.input.ScrollTarget() != 0 && canSpin) {
			if(InputManager.input.ScrollTarget() > 0)
				StartCoroutine(rotateSkillDown());
			else
				StartCoroutine(rotateSkillUp());
		}

	}

	//!Call this to update objective tet at top of the screen
	[EventVisible]
	public void UpdateObjectiveText(string newObjective) {
		objectiveText.GetComponent<Text>().text = newObjective;
	}

	//!Rotates map labels so that the text is always right side up, call this from anything that rotates the camera
	//!Right now its based on Player rotation, needs to be based on camera
	public void rotateMapObjects() {
		Quaternion newRotation;
		foreach(GameObject curLabel in mapLabels) {
			newRotation = Quaternion.Euler(new Vector3(90, 0, -player.transform.rotation.eulerAngles.y));
			curLabel.GetComponent<RectTransform>().rotation = newRotation;
		}
	}

	public void setCompassValue(float newValue) {

		//!Calculates distances between "the player and the objective" and "the camera and the objective"
		float distanceBetweenCamAndObj = Vector3.Distance (compassCameraPoint.transform.position, testObjective.transform.position);
		float distanceBetweenPlayerAndObj = Vector3.Distance (player.transform.position, testObjective.transform.position);



		//!If the camera is closer to the objective, this means the objective is behind the player.
		//!In these first two cases, the compass will be forced to one side or the other as to not confuse the player
		if (distanceBetweenCamAndObj < distanceBetweenPlayerAndObj && newValue >= 90) {
			newValue = 105;
		} 
		else if (distanceBetweenCamAndObj < distanceBetweenPlayerAndObj && newValue < 90) {
			newValue = 75;
		}

		else if(newValue > 105) {
			newValue = 105;
		}
		else if(newValue < 75) {
			newValue = 75;
		}

		if (newValue == 105) {
			slider.SetActive (false);
			rightArrow.SetActive (true);
			leftArrow.SetActive(false);
		} 
		else if (newValue == 75) {
			slider.SetActive (false);
			leftArrow.SetActive (true);
			rightArrow.SetActive(false);
		} 
		else {
			leftArrow.SetActive(false);
			rightArrow.SetActive(false);
			slider.SetActive(true);
		}
		compass.GetComponent<Slider>().value = newValue;
	}

	public float calculateObjectiveAngle(GameObject objective) {
		Vector3 pointToObjective;
		Vector3 pointStraightFromCam;

		//!Set points to determine which side of the player the vector towards the objective is going
		GameObject camPointRight = GameObject.Find("camPointRight");
		GameObject camPointleft = GameObject.Find("camPointLeft");


		//!create vector3 from player to objective and normalize it
		pointToObjective = objective.gameObject.transform.position - compassCameraPoint.transform.position;
		pointToObjective.Normalize();

		//!Set this vector to point right away from camera
		pointStraightFromCam = -compassCameraPoint.transform.right;
		pointStraightFromCam.Normalize();

		//!return angle from right facing vector

		return Vector3.Angle(pointStraightFromCam, pointToObjective);


	}

	//This is for testing
	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Finish") {
			UpdateObjectiveText("Objective Complete!");
		}
	}

	//fills HUD ability list
	void fillAbilityList(List<string> abilities) {
		//Add the proper ability to the phone wheel
		foreach(string curAbility in abilities) {
			switch(curAbility) {

				case "Push":
					phoneAbilitiesAvailible.Add(hudAbilityIcons[0]);
					break;

				case "Pull":
					phoneAbilitiesAvailible.Add(hudAbilityIcons[1]);
					break;

				case "Shock":
					phoneAbilitiesAvailible.Add(hudAbilityIcons[2]);
					break;
			}
		}
	}

	//Display the HUD icons in the phone menu
	public void SpawnHudAbilityIcons() {
		if(!abilitiesUp) {
			abilitiesUp = true;
			GameObject spawnPoint = GameObject.Find("abilitySelectPivot");

			//Calculate size of buttons based on screen size (HUD Canvas Size)
			middleAbilityIcon = Instantiate(phoneAbilitiesAvailible[0], spawnPoint.transform.position, Quaternion.identity) as GameObject;
			RectTransform middleRect = middleAbilityIcon.GetComponent<RectTransform>();

			Vector2 newMainDmiensions = new Vector2(Screen.height / 4, Screen.height / 4);

			middleRect.sizeDelta = newMainDmiensions;

			middleAbilityIcon.transform.SetParent(spawnPoint.transform);

			//spawn ability on the right if there is at least 2 elements in the array
			if(phoneAbilitiesAvailible.Count > 1) {
				Vector3 newPos = spawnPoint.transform.position;

				newPos.x += Screen.width / 24;
				newPos.y -= Screen.height / 8;
				rightAbilityIcon = Instantiate(phoneAbilitiesAvailible[1], newPos, Quaternion.identity) as GameObject;
				rightAbilityIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 6, Screen.height / 6);


				rightAbilityIcon.transform.SetParent(spawnPoint.transform);
			}

			//spawn ability on the left if there is at least 3 elements in the array
			if(phoneAbilitiesAvailible.Count > 2) {
				Vector3 newPos = spawnPoint.transform.position;

				newPos.x += Screen.width / 24;
				newPos.y += Screen.height / 8;
				leftAbilityIcon = Instantiate(phoneAbilitiesAvailible[phoneAbilitiesAvailible.Count - 1], newPos, Quaternion.identity) as GameObject;
				leftAbilityIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 6, Screen.height / 6);

				leftAbilityIcon.transform.SetParent(spawnPoint.transform);
			}
		} else {
			Destroy(middleAbilityIcon); Destroy(rightAbilityIcon); Destroy(leftAbilityIcon);
			abilitiesUp = false;
		}
	}

	//Shows map on phone and roates and resizes phone to screen
	public void showMap() {
		if (skillsOpen) {
			skillsOpen = false;
			//Debug.Log("skills closed");
			abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale /= 1.5f;
			skillWheel.SetActive(false);
			curAbility = 4;
			abilityWheelAnchorAnim.SetBool("slideIn", false);
			canSpin = false;
		}
		phoneButtons.SetActive(false);
		mapElements.SetActive(true);
		closeMapButton.SetActive(true);
		GameObject.Find("PhoneMenu").GetComponent<Animator>().SetBool("mapActive", true);
	}

	//hides map and plays close phone animation
	public void hideMap() {
		mapElements.SetActive(false);
		phoneButtons.SetActive(true);
		closeMapButton.SetActive(false);
		GameObject.Find("PhoneMenu").GetComponent<Animator>().SetBool("mapActive", false);

	}

	public void showSkills() {
		if(!skillsOpen) {
			skillsOpen = true;
			skillWheel.SetActive(true);
			abilityWheelIcons[4].GetComponent<RectTransform>().localScale *= 1.5f;
			abilityWheelAnchorAnim.SetBool("slideIn", true);
			canSpin = true;
			InputManager.changeSkills = true;
		} else {
			skillsOpen = false;
			abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale /= 1.5f;
			skillWheel.SetActive(false);
			curAbility = 4;
			abilityWheelAnchorAnim.SetBool("slideIn", false);
			canSpin = false;
			InputManager.changeSkills = false;
		}
	}

	public IEnumerator rotateSkillDown() {
		canSpin = false;
		skillWheel.GetComponent<Animator>().speed = 1;
		yield return new WaitForSeconds(0.49f);
		skillWheel.GetComponent<Animator>().speed = 0;

		curAbility++;
		if(curAbility > 7) {
			curAbility = 0;
			abilityWheelIcons[7].GetComponent<RectTransform>().localScale /= 1.5f;
		} else {
			abilityWheelIcons[curAbility - 1].GetComponent<RectTransform>().localScale /= 1.5f;
		}

		abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale *= 1.5f;

		canSpin = true;
	}

	public IEnumerator rotateSkillUp() {
		canSpin = false;
		skillWheel.GetComponent<Animator>().speed = -1;
		yield return new WaitForSeconds(0.49f);
		skillWheel.GetComponent<Animator>().speed = 0;

		curAbility--;
		if(curAbility < 0) {
			curAbility = 7;
			abilityWheelIcons[0].GetComponent<RectTransform>().localScale /= 1.5f;
		} else {
			abilityWheelIcons[curAbility + 1].GetComponent<RectTransform>().localScale /= 1.5f;
		}

		abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale *= 1.5f;

		canSpin = true;
	}

	public void ShowDialogueBox() {
		dialogueBox.SetActive(true);
	}

	public void HideDialogueBox() {
		dialogueBox.SetActive(false);
	}

	[EventVisible]
	public void SetDialogueBoxText(string name, string dialogue) {
		dialogueBox.SetActive(true);
		dialogueTitleText.GetComponent<Text>().text = name;
		dialogueText.GetComponent<Text>().text = dialogue;
	}

	[EventVisible]
	public void HideDialogueBoxText(string name, string dialogue) {
		dialogueBox.SetActive(false);
	}

	public void skillCut() {

	}

	public void skillSound() {

	}

	public void skillPull() {

	}

	public void skillPush() {

	}

	public void skillStun() {

	}

	public void ChangeInputToUI(bool change = true) {
		if(change)
			InputManager.instance.ChangeInputType("UIInputType");
		else
			InputManager.instance.ChangeInputType("GameInputType");
	}
	
	public void showPauseMenu () {
		pauseMenu.SetActive (true);
	}

	public void hidePauseMenu () {
		pauseMenu.SetActive (false);
	}

	public void loadScene(string s) {
		Application.LoadLevel(s);
	}

	public void quitGame () {
		Application.Quit ();
	}

}
