using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {

	public GameObject gameMap;
	public GameObject player;
	GameObject objectiveText;

	void Start () {
		objectiveText = GameObject.Find ("objectiveText");
	}
	

	void Update () {
		UpdateMap ();

	}

	public void UpdateMap () {

		Vector3 newMapPos = player.transform.position;
		newMapPos.y = player.transform.position.z;
		gameMap.GetComponent<RectTransform>().localPosition = newMapPos;
	}

	public void UpdateObjectiveText (string newObjective) {
		objectiveText.GetComponent<Text> ().text = "Objective: " + newObjective;
	}
}
