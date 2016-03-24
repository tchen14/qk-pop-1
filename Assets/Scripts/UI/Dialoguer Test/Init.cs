using UnityEngine;
using System.Collections;
using Debug = FFP.Debug;

public class Init : MonoBehaviour {

	//Design options for placement and size of prompt textbox and text
	public string textBoxText;
	public int textBoxWidth;
	public int textBoxHeight;
	public int textBoxVertOffset;
	public int textBoxHorzOffset;
	public bool inDialogue;

	private bool triggerDialoguer;
	private GameObject player;
	private bool dialogueCol;
	private GameObject dialogueGUI;

	GameObject interactDialoguer;

	void Awake () {
		Dialoguer.Initialize ();
		interactDialoguer = GameObject.Find ("InteractDialoguer");
		if (!interactDialoguer) {
			Debug.Log ("ui", "Could not find the 'InteractDialoguer' GameObject in the scene: " + Application.loadedLevelName);
		} else {
			interactDialoguer.SetActive (false);
		}
	}

	// Use this for initialization
	void Start () {
		triggerDialoguer = false;
		dialogueCol = false;
		player = GameObject.FindGameObjectWithTag ("Player");
		dialogueGUI = GameObject.Find ("GUI");
	}
	
	// Update is called once per frame
	void Update () {
		//Check if NPC has dialogue
		if (player.GetComponentInChildren<DialogueCollider> ().NPCDialogue == true) {
			//Asks player if they want to interact
			dialogueCol = true;
			interactDialoguer.SetActive(true);
			if (Input.GetKeyDown (KeyCode.F)) {
				//triggerDialoguer = true;
				Dialoguer.StartDialogue (player.GetComponentInChildren<DialogueCollider> ().NPCDialogueNumber, dialoguerCallback);
				this.enabled = false;
				triggerDialoguer = false;
				inDialogue = true;
				interactDialoguer.SetActive (false);
			}
		}
		else {
			dialogueCol = false;
			interactDialoguer.SetActive (false);
		}
		if (!dialogueGUI.GetComponent<DialoguerGUI> ().showingDialoguer) {
			inDialogue = false;
		}
	}

	//Checks if player is interacting and runs dialoguer
	/*
	void OnGUI() {
		if (dialogueCol) {
			GUI.Box (new Rect ((Screen.width / 2) - (textBoxWidth / 2) + textBoxHorzOffset, (Screen.height / 2) - (textBoxHeight / 2) + textBoxVertOffset, textBoxWidth, textBoxHeight), textBoxText);
			if (triggerDialoguer) {
				Dialoguer.StartDialogue (player.GetComponentInChildren<DialogueCollider> ().NPCDialogueNumber, dialoguerCallback);
				this.enabled = false;
				triggerDialoguer = false;
				inDialogue = true;
			}
		}
	}
	*/

	private void dialoguerCallback(){
		this.enabled = true;
	}
}
