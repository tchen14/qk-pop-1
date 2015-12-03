using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

[EventVisibleAttribute]
public class DialogueManager : MonoBehaviour {

	private static string portraitPATH = "/DialoguePortraits/";

	private bool _showing = false;
	private string _text;
	private string[] _choices;
	private bool isGood;
	private string _theme;
	private string _portrait;

	GameObject _dialogueGO;
	GameObject _goodBackground;
	GameObject _badBackground;
	GameObject _dialogueText;
	GameObject _continueButton;
	GameObject _goodPortrait;
	GameObject _badPortrait;
	GameObject _portraitImage;

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
		_goodPortrait = _dialogueGO.transform.FindChild ("GoodPortrait").gameObject;
		_badPortrait = _dialogueGO.transform.FindChild ("BadPortrait").gameObject;
		_portraitImage = _dialogueGO.transform.FindChild ("PortraitImage").gameObject;
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
		_portrait = data.portrait;

		if (_theme == "Good") {
			isGood = true;
		} else if (_theme == "Bad") {
			isGood = false;
		} else {
			Debug.Log("Theme is neither 'Good' or 'Bad'. Check Dialogue Tree");
		}
	}

	private void SetPortrait() {
		_portraitImage.GetComponent<Image> ().sprite = (Sprite)Resources.Load (portraitPATH + _portrait);
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
			_goodBackground.SetActive (true);
			_badBackground.SetActive (false);

			if(_portrait != "") {
				_goodPortrait.SetActive(true);
				_portraitImage.SetActive(true);
				SetPortrait();
			} else {
				_goodPortrait.SetActive(false);
				_portraitImage.SetActive(false);
			}

		} else {
			_goodBackground.SetActive (false);
			_badBackground.SetActive (true);

			if(_portrait != "") {
				_badPortrait.SetActive(true);
				_portraitImage.SetActive(true);
				SetPortrait();
			} else {
				_badPortrait.SetActive(false);
				_portraitImage.SetActive(false);
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
