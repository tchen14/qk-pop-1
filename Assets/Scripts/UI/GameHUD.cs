using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

/*! ----------------------------------------------------------------------------
 * Main hud control, contains functions for updating HUD information on the player's screen
 * these functions are designed to be called from whatever script needs to update them.
 * ----------------------------------------------------------------------------
 */

public class GameHUD : MonoBehaviour {
#pragma warning disable 0219
#pragma warning disable 0414
	GameObject mainHUDCanvas;				//!<The canvas HUD is rendered on
	GameObject worldMapCanvas;				//!<All the game map elements
	GameObject gameMap;						//!<The map iamge on a plane
	GameObject player;						//!<reference to player
	public int numOfAbilities;						//!<temporary int for number of abilities in game
	public GameObject[] hudAbilityIcons;			//!<Array of hud icons, set in inspector
	public bool abilitiesUp = false;
	public GameObject[] abilityWheelIcons;
	Animator abilityWheelAnchorAnim;

	List<GameObject> phoneAbilitiesAvailible;		//!<List containing hud phone abilties
	GameObject mapCam;								//!<Camera used for minimap
	static GameObject objectiveText;						//!<Objective Text UI element
	GameObject[] mapLabels;							//!<Array of text taht appears on minimap

	GameObject middleAbilityIcon;					//!<Phone ability icon references
	GameObject rightAbilityIcon;
	GameObject leftAbilityIcon;
	GameObject[] abilityButtons;

	int curAbility = 4;

	bool skillsOpen = false;
	bool canSpin = true;
	GameObject skillWheel;
	GameObject closeMapButton;
	GameObject phoneButtons;
	GameObject mapElements;
	GameObject compassCameraPoint;					//!<Point at camera location used to calculate objective positions

	GameObject testObjective;

	void Awake() {

		mainHUDCanvas = GameObject.Find("mainHUD");
		skillWheel = GameObject.Find("abilityWheel");
		abilityWheelAnchorAnim = GameObject.Find("AbilityWheelAnchor").GetComponent<Animator>();
		worldMapCanvas = GameObject.Find ("worldMapCanvas");
		gameMap = GameObject.Find("mapBG");
		player = GameObject.Find ("_Player");
		testObjective = GameObject.Find("testObjectiveCanvas");
		//!Turn on UI stuff
		worldMapCanvas.SetActive(true);
		canSpin = false;
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
		//!Set objective text reference
		objectiveText = GameObject.Find("objectiveText");
		//!Set initial test objective
		UpdateObjectiveText("Head to the Objective Point");

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
		//Debug.Log("Angle found: " + calculateObjectiveAngle(testObjective));

		//Testing
		if(Input.GetKeyDown("1") && canSpin) {
			StartCoroutine(rotateSkillDown());

		}

		if(Input.GetKeyDown("2") && canSpin) {
			StartCoroutine(rotateSkillUp());

		}

	}

	//!Call this to update objective tet at top of the screen
	public static void UpdateObjectiveText(string newObjective) {
		objectiveText.GetComponent<Text>().text = newObjective;
	}

	//!Rotates map labels so that the text is always right side up, call this from anything that rotates the camera
	//!Right now its based on Player rotation, needs to be based on camera
	public void rotateMapObjects() {
		Quaternion newRotation;
		foreach (GameObject curLabel in mapLabels) {
			newRotation = Quaternion.Euler(new Vector3(90, 0, -player.transform.rotation.eulerAngles.y));
			curLabel.GetComponent<RectTransform>().rotation = newRotation;
		}
	}

	public void setCompassValue(float newValue) {
		GameObject compass = GameObject.Find("compassSlider");
		if (newValue > 105) {
			newValue = 105;
		}
		if (newValue < 75) {
			newValue = 75;
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
		if (col.gameObject.tag == "Finish") {
			UpdateObjectiveText("Objective Complete!");
		}
	}

	//fills HUD ability list
	void fillAbilityList(List<string> abilities) {
		//Add the proper ability to the phone wheel
		foreach (string curAbility in abilities) {
			switch (curAbility) {

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
		if (!abilitiesUp) {
			abilitiesUp = true;
			GameObject spawnPoint = GameObject.Find("abilitySelectPivot");

			//Calculate size of buttons based on screen size (HUD Canvas Size)
			middleAbilityIcon = Instantiate(phoneAbilitiesAvailible[0], spawnPoint.transform.position, Quaternion.identity) as GameObject;
			RectTransform middleRect = middleAbilityIcon.GetComponent<RectTransform>();

			Vector2 newMainDmiensions = new Vector2(Screen.height / 4, Screen.height / 4);

			middleRect.sizeDelta = newMainDmiensions;

			middleAbilityIcon.transform.SetParent(spawnPoint.transform);

			//spawn ability on the right if there is at least 2 elements in the array
			if (phoneAbilitiesAvailible.Count > 1) {
				Vector3 newPos = spawnPoint.transform.position;

				newPos.x += Screen.width / 24;
				newPos.y -= Screen.height / 8;
				rightAbilityIcon = Instantiate(phoneAbilitiesAvailible[1], newPos, Quaternion.identity) as GameObject;
				rightAbilityIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 6, Screen.height / 6);


				rightAbilityIcon.transform.SetParent(spawnPoint.transform);
			}

			//spawn ability on the left if there is at least 3 elements in the array
			if (phoneAbilitiesAvailible.Count > 2) {
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
		if(skillsOpen) {
			skillsOpen = false;
			abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale /= 1.5f;
			skillWheel.SetActive(false);
			curAbility = 0;
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
		} else {
			skillsOpen = false;
			Debug.Log("skills closed");
			abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale /= 1.5f;
			skillWheel.SetActive(false);
			curAbility = 4;
			abilityWheelAnchorAnim.SetBool("slideIn", false);
			canSpin = false;
		}
	}

	public IEnumerator rotateSkillDown() {
		canSpin = false;
		skillWheel.GetComponent<Animator>().speed = 1;
		yield return new WaitForSeconds(0.49f);
		skillWheel.GetComponent<Animator>().speed = 0;

		curAbility++;
		if(curAbility > 7){
			curAbility = 0;
			abilityWheelIcons[7].GetComponent<RectTransform>().localScale /= 1.5f;
		}
		else{
			abilityWheelIcons[curAbility -1].GetComponent<RectTransform>().localScale /= 1.5f;
		}
		
		abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale *= 1.5f;

		canSpin = true;
	}

	public IEnumerator rotateSkillUp() {
		canSpin = false;
		if(curAbility == 0){
		
		}
		skillWheel.GetComponent<Animator>().speed = -1;
		yield return new WaitForSeconds(0.49f);
		skillWheel.GetComponent<Animator>().speed = 0;


		curAbility--;
		if(curAbility < 0){
			curAbility = 7;
			abilityWheelIcons[0].GetComponent<RectTransform>().localScale /= 1.5f;
		}
		else{
			abilityWheelIcons[curAbility +1].GetComponent<RectTransform>().localScale /= 1.5f;
		}
		
		abilityWheelIcons[curAbility].GetComponent<RectTransform>().localScale *= 1.5f;


		canSpin = true;
	}

}
