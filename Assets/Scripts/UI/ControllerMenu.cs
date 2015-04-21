using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerMenu : MonoBehaviour {

	bool waitForInput = false;

	public GameObject jumpButton;
	public GameObject interactButton;
	public GameObject ability1Button;
	public GameObject appEquipButton;
	public GameObject nextAbilityButton;
	public GameObject previousAbilityButton;
	public GameObject notificationsButton;
	public GameObject compassAppButton;
	public GameObject journalAppButton;
	public GameObject aimQuincyButton;
	public GameObject findNextTarButton;

	public GameObject findPrevTarButton;
	public GameObject sprintButton;
	public GameObject climbButton;
	public GameObject coverButton;
	public GameObject crouchButton;
	public GameObject centerCamButton;
	public GameObject movePlayerButton;
	public GameObject moveCamButton;
	public GameObject PauseButton;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (waitForInput == true) {
			string temp = Input.inputString;
		}
	}

	public void changeButtonAssign (string action) {
		//string input = Input.inputString;


	}
}
