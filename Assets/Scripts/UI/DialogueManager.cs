using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

[EventVisibleAttribute]
public class DialogueManager : MonoBehaviour {

	public GUISkin skin;

	private bool _showing = false;
	private string _text;
	private string[] _choices;
	private bool isGood = true;
	private string _theme;

	GameObject _dialogueGO;
	GameObject _goodBackground;
	GameObject _badBackground;
	GameObject _dialogueText;
	GameObject _continueButton;

	// Use this for initialization
	void Awake () {
		Dialoguer.Initialize ();
	}

	// Use this for initialization
	void Start () {
		Dialoguer.events.onStarted += onStarted;
		Dialoguer.events.onEnded += onEnded;
		Dialoguer.events.onTextPhase += onTextPhase;

		_dialogueGO = GameObject.Find ("Dialogue");
		_goodBackground = _dialogueGO.transform.FindChild ("GoodBackground").gameObject;
		_badBackground = _dialogueGO.transform.FindChild ("BadBackground").gameObject;
		_dialogueText = _dialogueGO.transform.FindChild ("DialogueText").gameObject;
		_continueButton = _dialogueGO.transform.FindChild ("ContinueButton").gameObject;
	}
	
	private void onStarted() {
		_showing = true;
	}
	
	private void onEnded() {
		_showing = false;
		_goodBackground.SetActive (false);
		_badBackground.SetActive (false);
	}

	private void onTextPhase(DialoguerTextData data) {
		_text = data.text;
		_dialogueText.GetComponent<Text> ().text = data.text;
		_choices = data.choices;
		_theme = data.theme;
	}

	private void SetText() {
		_dialogueText.GetComponent<Text>().text = _text;
	}

	void OnGUI() {
		if (!_showing) {
			if(_dialogueGO.activeInHierarchy == true) {
				_dialogueGO.SetActive(false);
			}
			return;
		} 
		
		if (_dialogueGO.activeInHierarchy == false) {
			_dialogueGO.SetActive(true);
		}

		if (isGood) {
			if (_goodBackground.activeInHierarchy == false) {
				_goodBackground.SetActive (true);
				//SetText();
			}
		} else {
			if (_badBackground.activeInHierarchy == false) {
				_badBackground.SetActive (true);
				//SetText();
			}
		}



		if (_choices == null || _choices.Length < 1) {
			ShowContinueButton();
		} else {
			for(int i = 0; i < _choices.Length; i++) {
				//DrawChoices
			}
		}
		
		
	}

	private void ShowContinueButton() {
		_continueButton.GetComponent<Image> ().enabled = true;
		_continueButton.GetComponent<Button> ().enabled = true;
		_continueButton.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
	}

	public void ClickedContinueButton() {
		Dialoguer.ContinueDialogue ();
		//SetText();
	}

	private void HideContinueButton() {
		_continueButton.GetComponent<Image> ().enabled = false;
		_continueButton.GetComponent<Button> ().enabled = false;
		_continueButton.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
	}

	[EventVisibleAttribute]
	public void Speak(int npcID, int index) {

		if (_showing) {
			return;
		}

		JSONNode npcDialogue = RetrieveDialogueFromJSON();

		if (npcDialogue [npcID.ToString ()] == null) {
			Debug.Log("NPC ID: " + npcID + " doesn't exist, check dialogue list json file!");
			return;
		}

		if (npcDialogue [npcID.ToString ()] [index] == null) {
			Debug.Log("Index: " + index + " on NPC ID: " + npcID + " doesn't exist, check dialogue list json file!");
			return;
		}

		if (_theme == "Good") {
			isGood = true;
		} else if (_theme == "Bad") {
			isGood = false;
		} else {
			Debug.Log("Theme is neither 'Good' or 'Bad'. Check Dialogue Tree");
			return;
		}

		Dialoguer.StartDialogue (npcDialogue [npcID.ToString ()] [index].AsInt);

		return;
	}

	private JSONNode RetrieveDialogueFromJSON() {
		
		if(!System.IO.File.Exists(Application.dataPath + "/Resources/dialogueList.json"))
		{
			Debug.LogError ("Could not find Dialogue List JSON file");
			return null;
		}
		
		string jsonRead = System.IO.File.ReadAllText(Application.dataPath + "/Resources/dialogueList.json");
		JSONNode jsonParsed = JSON.Parse (jsonRead);
		
		return jsonParsed;
	}

}
