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

	public GameObject mainHUDCanvas;				//!The canvas HUD is rendered on
	public GameObject worldMapCanvas;				//!<All the game map elements
	public GameObject gameMap;						//!<The map iamge on a plane
	public GameObject player;						//!<reference to player
	public int numOfAbilities;						//!<temporary int for number of abilities in game
	public GameObject[] hudAbilityIcons;			//!<Array of hud icons, set in inspector

	List<GameObject> phoneAbilitiesAvailible;		//!<List containing hud phone abilties
	GameObject mapCam;								//!<Camera used for minimap
	GameObject objectiveText;						//!<Objective Text UI element
	GameObject[] mapLabels;							//!<Array of text taht appears on minimap

	GameObject middleAbilityIcon;					//!<Phone ability icon references
	GameObject rightAbilityIcon;
	GameObject leftAbilityIcon;

	GameObject compassCameraPoint;					//!<Point at camera location used to calculate objective positions

	public GameObject testObjective;

	void Awake () {

		mainHUDCanvas = GameObject.Find ("mainHUD");
		//!Turn on UI stuff
		worldMapCanvas.SetActive (true);

		//!Fill mapLabels array
		mapLabels = GameObject.FindGameObjectsWithTag ("worldMapLabel");

		//!Set mapcam reference
		mapCam = GameObject.Find ("mapCam");
		//!Set compassCameraPoint reference
		compassCameraPoint = GameObject.Find ("compassCameraPoint");
		//!Set objective text reference
		objectiveText = GameObject.Find("objectiveText");
		//!Set initial test objective
		UpdateObjectiveText("Head to the Objective Point");

		//Testing for filling ability list
		List<string> tempAbList = new List<string>();
		tempAbList.Add ("Push");
		tempAbList.Add ("Pull");
		tempAbList.Add ("Shock");


		//Initialize phone abilities list
		phoneAbilitiesAvailible = new List<GameObject> ();

		fillAbilityList (tempAbList);



		
	}

	void Start() {
		//Place the ability buttons in the Phone Menu
		SpawnHudAbilityIcons ();
	}
	

	void Update () {
		//!This is for testing, call update map from player movements
		rotateMapObjects ();

		//!Set the compass indicator
		setCompassValue (calculateObjectiveAngle (testObjective));
		//Debug.Log("Angle found: " + calculateObjectiveAngle(testObjective));

	}

	//!Call this to update objective tet at top of the screen
	public void UpdateObjectiveText (string newObjective) {
		objectiveText.GetComponent<Text> ().text = "Objective: " + newObjective;
	}

	//!Rotates map labels so that the text is always right side up, call this from anything that rotates the camera
	//!Right now its based on Player rotation, needs to be based on camera
	public void rotateMapObjects () {
		Quaternion newRotation;
		foreach(GameObject curLabel in mapLabels) {
			newRotation = Quaternion.Euler(new Vector3(90,0,-player.transform.rotation.eulerAngles.y));
			curLabel.GetComponent<RectTransform>().rotation = newRotation;
		}
	}

	public void setCompassValue (float newValue) {
		GameObject compass = GameObject.Find ("compassSlider");
		if(newValue > 105){
			newValue = 105;
		}
		if(newValue < 75){
			newValue = 75;
		}
		compass.GetComponent<Slider> ().value = newValue;
	}

	public float calculateObjectiveAngle (GameObject objective) {
		Vector3 pointToObjective;
		Vector3 pointStraightFromCam;

		//!Set points to determine which side of the player the vector towards the objective is going
		GameObject camPointRight = GameObject.Find ("camPointRight");
		GameObject camPointleft = GameObject.Find ("camPointLeft");


		//!create vector3 from player to objective and normalize it
		pointToObjective = objective.gameObject.transform.position - compassCameraPoint.transform.position;
		pointToObjective.Normalize ();

		//!Set this vector to point right away from camera
		pointStraightFromCam = -compassCameraPoint.transform.right;
		pointStraightFromCam.Normalize ();

		//!return angle from right facing vector
		
		return Vector3.Angle(pointStraightFromCam, pointToObjective);
		

	}
	
	//This is for testing
	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag == "Finish"){
			UpdateObjectiveText("Objective Complete!");
		}
	}

	//fills HUD ability list
	void fillAbilityList (List<string> abilities){
		//Add the proper ability to the phone wheel
		foreach (string curAbility in abilities){
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
	void SpawnHudAbilityIcons () {
		GameObject spawnPoint = GameObject.Find ("abilitySelectPivot");

		//Calculate size of buttons based on screen size (HUD Canvas Size)
		middleAbilityIcon = Instantiate (phoneAbilitiesAvailible [0], spawnPoint.transform.position, Quaternion.identity) as GameObject;
		RectTransform middleRect = middleAbilityIcon.GetComponent<RectTransform> ();

		Vector2 newMainDmiensions = new Vector2 (Screen.height/4, Screen.height/4);

		middleRect.sizeDelta = newMainDmiensions;

		middleAbilityIcon.transform.SetParent (spawnPoint.transform);

		//spawn ability on the right if there is at least 2 elements in the array
		if(phoneAbilitiesAvailible.Count > 1){
			Vector3 newPos = spawnPoint.transform.position;

			newPos.x += Screen.width/20;
			newPos.y -= Screen.height/8;
			rightAbilityIcon = Instantiate (phoneAbilitiesAvailible [1], newPos, Quaternion.identity) as GameObject;
			Vector3 newScale = rightAbilityIcon.transform.localScale;
			newScale *= 1.8f;

			rightAbilityIcon.transform.localScale = newScale;
			rightAbilityIcon.transform.SetParent (spawnPoint.transform);
		}

		//spawn ability on the left if there is at least 3 elements in the array
		if(phoneAbilitiesAvailible.Count > 2){
			Vector3 newPos = spawnPoint.transform.position;
			Vector3 newScale = phoneAbilitiesAvailible [0].transform.localScale;
			newScale /= 1.5f;
			newPos.x += Screen.width/12;
			newPos.y += Screen.height/8;
			leftAbilityIcon = Instantiate (phoneAbilitiesAvailible[phoneAbilitiesAvailible.Count-1], newPos, Quaternion.identity) as GameObject;
			leftAbilityIcon.transform.localScale = newScale;
			leftAbilityIcon.transform.SetParent (spawnPoint.transform);
		}

	}
}
