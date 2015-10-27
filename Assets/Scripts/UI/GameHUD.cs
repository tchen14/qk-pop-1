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
[System.Serializable]
public class AbilityButton{
	public string name;
	public Sprite icon;
}

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
	
	public int numOfAbilities;						//!<temporary int for number of abilities in game
	public GameObject[] hudAbilityIcons;			//!<Array of hud icons, set in inspector
	public bool abilitiesUp = false;
	public GameObject[] abilityWheelIcons;

	public bool gamePaused = false;


	List<GameObject> phoneAbilitiesAvailible;		//!<List containing hud phone abilties
	GameObject mapCam;								//!<Camera used for minimap
	static GameObject objectiveText;						//!<Objective Text UI element
	static GameObject dialogueBox, dialogueText, dialogueTitleText;

	GameObject[] mapLabels;							//!<Array of text taht appears on minimap

	GameObject middleAbilityIcon;					//!<Phone ability icon references
	GameObject rightAbilityIcon;
	GameObject leftAbilityIcon;
	public GameObject[] abilityButtons;

	[ReadOnly]public int curAbility = 1;

	GameObject closeMapButton;
	GameObject phoneButtons;
	GameObject mapElements;

	//The global variables affiliated with the ability wheel
	GameObject abilityScroller;
	GameObject abilityWheelAnchor;
	GameObject skillWheelView;
	GameObject skillWheelCursor;
	RectTransform skillWheelBounds;
	public float [] ablocy = {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f}; // the variable that stores the location data of the ability buttons
	bool skillsOpen = false;
	bool skillsMoving = false;
	bool skillsRotating = false;

	int maxcurAbility;
	bool canSpin = false; 

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
		worldMapCanvas = GameObject.Find("worldMapCanvas");
		gameMap = GameObject.Find("mapBG");
		player = GameObject.Find("_Player");
		testObjective = GameObject.Find("testObjectiveCanvas");
		pauseMenu = GameObject.Find ("pauseMenu");
		pauseMenu.SetActive (false);
		
		//!Turn on UI stuff
		worldMapCanvas.SetActive(true);

		//!Fill mapLabels array
		mapLabels = GameObject.FindGameObjectsWithTag("worldMapLabel");
		closeMapButton = GameObject.Find("CloseMapButton");
		closeMapButton.SetActive(false);

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

		//!Set Ability Wheel references
		abilityScroller = GameObject.Find ("AbilityWheel");
		skillWheelView = GameObject.Find ("abilityWheelView");
		abilityWheelAnchor = GameObject.Find ("AbilityWheelAnchor");
		skillWheelCursor = GameObject.Find("AbilityWheelCursor");
		GameObject sWheelTmp = GameObject.Find ("abBounds");

		settingUpAbilityWheel ();

		skillWheelBounds = sWheelTmp.GetComponent<RectTransform>();
		skillWheelCursor.SetActive (false);


		phoneButtons = GameObject.Find("PhoneButtons");
		mapElements = GameObject.Find("MapElements");
		mapElements.SetActive(false);

		//Testing for filling ability list
		List<string> tempAbList = new List<string>();
		tempAbList.Add("Push");
		tempAbList.Add("Pull");
		tempAbList.Add("Shock");
		tempAbList.Add ("Sound");

		//Initialize phone abilities list
		phoneAbilitiesAvailible = new List<GameObject>();
		maxcurAbility = abilityButtons.Length - 1;

		fillAbilityList(tempAbList);
	}

	void Start() {
		//Place the ability buttons in the Phone Menu
		//SpawnHudAbilityIcons ();
		}

	void Update() {
		//!This is for testing, call update map from player movements
		rotateMapObjects ();

		//!Set the compass indicator
		setCompassValue (calculateObjectiveAngle (testObjective));
		
		if (Input.GetKey ("tab"))
			showSkills ();
		else
			hideSkills ();

		if (skillsOpen && !skillsRotating) {
			if (Input.GetKeyDown ("up")) {
				StartCoroutine(Rotate_skills_up ());
			} else if (Input.GetKeyDown ("down")) {
				StartCoroutine(Rotate_skills_down ());
			}
		}
		if (Input.GetKeyDown ("escape"))
			PauseGame ();
	}

	void LateUpdate(){
		moveAbilities();
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


	//These are the functions regarding the Compass
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
			showSkills();
			//Debug.Log("skills closed");
			abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale /= 1.5f;
			curAbility = 4;
			canSpin = false;
		}
		phoneButtons.SetActive(false);
		mapElements.SetActive(true);
		closeMapButton.SetActive(true);
		abilityWheelAnchor.SetActive (false);
		GameObject.Find("PhoneMenu").GetComponent<Animator>().SetBool("mapActive", true);
	}

	//hides map and plays close phone animation
	public void hideMap() {
		mapElements.SetActive(false);
		phoneButtons.SetActive(true);
		closeMapButton.SetActive(false);
		GameObject.Find("PhoneMenu").GetComponent<Animator>().SetBool("mapActive", false);
		abilityWheelAnchor.SetActive (true);

	}

	/***********************************************************************
	 *------<These are the functions Involving the the ability wheel>------*
	 **********************************************************************/
	public void showSkills()
	{
		if (!skillsMoving && !skillsOpen) 
		{
			RectTransform abv = skillWheelView.GetComponent<RectTransform> ();
			skillsOpen = true;
			skillsMoving = true;
			canSpin = true;
		}

	}

	public void hideSkills()
	{
		if(!skillsMoving && skillsOpen)
		{
			RectTransform abv = skillWheelView.GetComponent<RectTransform> ();
			skillsMoving = true;
			canSpin = false;
			skillsOpen = false;
		}
	}

	//This is the function that causes the ability wheel to move into position
	void moveAbilities() 
	{
		RectTransform abs = abilityWheelAnchor.GetComponent<RectTransform> ();
		RectTransform abv = skillWheelView.GetComponent<RectTransform> ();
		float movespeed = Time.deltaTime * 250; //this is the speed the wheel travels to the designated locations
		if(skillsOpen)
		{	
			skillWheelCursor.SetActive (true);
			abv.offsetMax = new Vector2(abv.offsetMax.x, Mathf.MoveTowards(abv.offsetMax.y, 160.0f, movespeed*2));
			abv.offsetMin = new Vector2(abv.offsetMin.x, Mathf.MoveTowards(abv.offsetMin.y, -160.0f, movespeed*2));
			abs.localPosition = new Vector2(abs.localPosition.x, Mathf.MoveTowards(abs.localPosition.y, skillWheelBounds.offsetMax.y, movespeed));
		}
		else
		{
			skillWheelCursor.SetActive (false);
			abv.offsetMax = new Vector2(abv.offsetMax.x, Mathf.MoveTowards(abv.offsetMax.y, 23.0f, movespeed*2));
			abv.offsetMin = new Vector2(abv.offsetMin.x, Mathf.MoveTowards(abv.offsetMin.y, -23.0f, movespeed*2));
			abs.localPosition = new Vector2(abs.localPosition.x, Mathf.MoveTowards(abs.localPosition.y, skillWheelBounds.offsetMin.y, movespeed));
		}
		skillsMoving = false;
	}

	void settingUpAbilityWheel()
	{
		for (int i = 0; i < abilityButtons.Length; i++) {
			RectTransform tmp = abilityButtons [i].GetComponent<RectTransform> ();
			ablocy [i] = tmp.localPosition.y;
		}
		updateAbilityIcons();
	}

	void updateAbilityIcons()
	{
		GameObject abbotim = Instantiate(abilityButtons[5]);
		abbotim.transform.SetParent (GameObject.Find ("content Panel").transform, false);
		abbotim.transform.localPosition = new Vector2 (0.0f,ablocy[0]);
		Destroy (abilityButtons [0], 0.0f);
		abilityButtons [0] = abbotim;

		GameObject abtop = Instantiate(abilityButtons[1]);
		abtop.transform.SetParent (GameObject.Find ("content Panel").transform, false);
		abtop.transform.localPosition = new Vector2 (0.0f,ablocy[6]);
		Destroy (abilityButtons [6], 0.0f);
		abilityButtons [6] = abtop;
	}

	public IEnumerator Rotate_skills_up()
	{
		skillsRotating = true;
		GameObject abtemp = abilityButtons[1];
		int ab_amount = abilityButtons.Length - 1;

		for (int i = ab_amount; i >=1; i--) 
		{
			RectTransform tmp = abilityButtons[i].GetComponent<RectTransform>();
			tmp.localPosition = new Vector2(tmp.localPosition.x, Mathf.MoveTowards(tmp.localPosition.y, ablocy[i-1], 100));
		}

		yield return new WaitForSeconds(0.2f);

		RectTransform topImage = abilityButtons[1].GetComponent<RectTransform>();
		RectTransform abBottom = abilityButtons[6].GetComponent<RectTransform>();
		topImage.localPosition = abBottom.localPosition;
		abBottom.localPosition = new Vector2 (abBottom.localPosition.x, ablocy [6]);

		for (int i = 1; i < ab_amount - 1; i++)
				abilityButtons[i] = abilityButtons[i+1];
		abilityButtons[ab_amount - 1] = abtemp;

		yield return new WaitForSeconds(0.1f);

		updateAbilityIcons ();
		
		skillsRotating = false;
	}

	public IEnumerator Rotate_skills_down()
	{
		skillsRotating = true;
		GameObject abtemp = abilityButtons[5];
		int ab_amount = abilityButtons.Length - 1;

		for (int i = 0; i < ab_amount; i++) 
		{
			RectTransform tmp = abilityButtons[i].GetComponent<RectTransform>();
			tmp.localPosition = new Vector2(tmp.localPosition.x, Mathf.MoveTowards(tmp.localPosition.y, ablocy[i+1], 100));
		}

		RectTransform botImage = abilityButtons[5].GetComponent<RectTransform>();
		RectTransform abtop = abilityButtons[0].GetComponent<RectTransform>();
		botImage.localPosition = abtop.localPosition;
		abtop.localPosition = new Vector2 (abtop.localPosition.x, ablocy [0]);

		for (int i = ab_amount - 1; i > 1; i--)
			abilityButtons[i] = abilityButtons[i-1];
		abilityButtons[1] = abtemp;

		yield return new WaitForSeconds(0.1f);
		updateAbilityIcons ();

		skillsRotating = false;
	}

	/***********************************************************************
	 *-------<These are the functions Involving the dialogue>--------------*
	 **********************************************************************/

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

	public void ChangeInputToUI(bool change = true) {
		if(change)
			InputManager.instance.ChangeInputType("UIInputType");
		else
			InputManager.instance.ChangeInputType("GameInputType");
	}


	public void PauseGame()
	{
		if (!gamePaused) {
			pauseMenu.SetActive(true);
			Time.timeScale = 0;
			gamePaused = true;
		}
		else {
			pauseMenu.SetActive(false);
			Time.timeScale = 1;
			gamePaused = false;
		}

	}

	/*
	public void loadScene(string s) {
		Application.LoadLevel(s);
	}*/

	public void quitGame () {
		Application.Quit ();
	}

}
