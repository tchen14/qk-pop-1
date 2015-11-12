using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

[EventVisibleAttribute]
public class DialogueManager : MonoBehaviour {

	private bool _showing = false;
	private string _text;
	private string[] _choices;
	private bool isGood = true;

	GameObject _dialogueGO;
	GameObject _goodBackground;
	GameObject _badBackground;
	GameObject _dialogueText;

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
			}
		} else {
			if (_badBackground.activeInHierarchy == false) {
				_badBackground.SetActive (true);
			}
		}



		if (_choices == null && _choices.Length > 0) {
			//DrawContinueButton
		} else {
			for(int i = 0; i < _choices.Length; i++) {
				//DrawChoices
			}
		}
		
		
	}

	[EventVisibleAttribute]
	public void Speak(int npcID, int index) {
		JSONNode npcDialogue = RetrieveDialogueFromJSON();

		if (npcDialogue [npcID.ToString ()] == null) {
			Debug.Log("NPC ID: " + npcID + " doesn't exist, check dialogue list json file!");
			return;
		}

		if (npcDialogue [npcID.ToString ()] [index] == null) {
			Debug.Log("Index: " + index + " on NPC ID: " + npcID + " doesn't exist, check dialogue list json file!");
			return;
		}

		if (npcDialogue [npcID.ToString ()] [-1] == "Good") {
			isGood = true;
		} else if (npcDialogue [npcID.ToString ()] [-1] == "Bad") {
			isGood = false;
		} else {
			Debug.Log("NPC ID: " + npcID + " is neither 'Good' or 'Bad', check last index in dialogue list json file");
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
