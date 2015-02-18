using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* ----------------------------------------------------------------------------
 * Main hud control, contains functions for updating HUD information on the player's screen
 * these functions are designed to be called from whatever script needs to update them.
 * ----------------------------------------------------------------------------
 */

public class GameHUD : MonoBehaviour {

	public GameObject gameMap;
	public GameObject player;
	GameObject mapPivot;
	GameObject objectiveText;

	void Start () {
		objectiveText = GameObject.Find ("objectiveText");
		mapPivot = GameObject.Find ("mapPivot");
	}
	

	void Update () {
		//this is for testing, call update map from player movements
		UpdateMap ();

	}

	public void UpdateMap () {

		Vector2 newMapPos = player.transform.position;
		newMapPos.y = player.transform.position.z;
		gameMap.GetComponent<RectTransform>().localPosition = newMapPos;

		Quaternion newMapRotation = Quaternion.identity;
		newMapRotation.z = player.transform.rotation.y;
		newMapRotation.y = 0;
		newMapRotation.x = 0;
		mapPivot.transform.rotation = newMapRotation;
		Debug.Log (newMapRotation.ToString ());

		//newMapRotation;
		/*
		//Keep map pivot in place
		Vector2 newPivot;
		newPivot = mapPivot.GetComponent<RectTransform> ().pivot;
		gameMap.GetComponent<RectTransform> ().pivot = newPivot;

		//Rotate map with player
		Quaternion newMapRotation;
		newMapRotation = this.transform.rotation;
		newMapRotation.z = player.transform.rotation.y;
		newMapRotation.y = 0;
		newMapRotation.x = 0;
		gameMap.GetComponent<RectTransform> ().rotation = newMapRotation;
		*/
		
	}

	public void UpdateObjectiveText (string newObjective) {
		objectiveText.GetComponent<Text> ().text = "Objective: " + newObjective;
	}
}
