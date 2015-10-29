using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class abilityDockController : MonoBehaviour {

	public Button[] abilities;
	public int iconHeight;
	public int iconSpacing;
	public int spacingFromBottom;
	public float lerpSpeed;

	bool dockClosed;
	bool opening;
	bool closing;
	public Vector3[] targetLocation;
	public Vector3[] startLocation;

	int selectedAbility;

	// Use this for initialization
	void Start () {
		selectAbility (0);
		abilities [0].Select ();
		//openDock ();
		closeDock ();
		getStartLocation ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			if(dockClosed){
				openDock();
			}
			else{
				//closeDock();

			}
		}
		if (opening) {
			for(int i = 0; i < abilities.Length; i++){
				abilities[i].transform.position = Vector3.Lerp(startLocation[i], targetLocation[i], lerpSpeed);
			}
		}
	}

	void closeDock(){
		for (int i = 0; i < abilities.Length; i++) {
			//abilities[i].transform.position = new Vector3(abilities[i].transform.position.x, 25, 0);
			targetLocation[i] = new Vector3(abilities[i].transform.position.x, 25, 0);
			closing = true;
		}
		dockClosed = true;
	}

	void openDock(){
		abilities [selectedAbility].Select ();
		for (int i = 0; i < abilities.Length; i++) {
			//abilities[i].transform.position = new Vector3(abilities[i].transform.position.x, i*(iconHeight + iconSpacing) + spacingFromBottom, 0);
			targetLocation[i] = new Vector3(abilities[i].transform.position.x, i*(iconHeight + iconSpacing) + spacingFromBottom, 0);
			changeSortOrder(i, 0);
			opening = true;
		}
		dockClosed = false;
	}

	void changeSortOrder(int i, int order){
		abilities[i].GetComponent<Canvas>().sortingOrder = order;
	}

	public void selectAbility(int i){
		if (dockClosed) {
			openDock ();
		} else {
			changeSortOrder (i, 10);
			selectedAbility = i;
			closeDock ();
	
		}
	}

	void getStartLocation(){
		for (int i = 0; i < abilities.Length; i++) {
			startLocation[i] = abilities[i].transform.position;
		}
	}
}