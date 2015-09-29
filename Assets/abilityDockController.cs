using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class abilityDockController : MonoBehaviour {

	public Button[] abilities;
	public int iconHeight;
	public int iconSpacing;
	public int spacingFromBottom;
	bool dockClosed;

	// Use this for initialization
	void Start () {
		closeDock ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			if(dockClosed){
				openDock();
			}
			else{
				closeDock();
			}
		}
	}

	void closeDock(){
		for (int i = 0; i < abilities.Length; i++) {
			abilities[i].transform.position = new Vector3(abilities[i].transform.position.x, 25, 0);
		}
		dockClosed = true;
	}

	void openDock(){
		for (int i = 0; i < abilities.Length; i++) {
			abilities[i].transform.position = new Vector3(abilities[i].transform.position.x, i*(iconHeight + iconSpacing) + spacingFromBottom, 0);
			//abilities[i].GetComponent<Canvas>().sortingOrder = 0;
		}
		dockClosed = false;
	}

	/*void buttonSelected(int i){
		abilities[i].GetComponent<Canvas>().sortingOrder = 10;
		closeDock ();
	}*/

}