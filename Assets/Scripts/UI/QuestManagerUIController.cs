using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestManagerUIController : MonoBehaviour {

	GameObject player;
	public GameObject questContainer;
	QuestManager qm;
	public GameObject questUI;
	Button questButton;
	float buttonHeight;
	public float spacing;
	GameObject moreQuestInfo;
	Text moreQuestInfoTitle;
	Text moreQuestInfoDescription;

	void Start(){
		player = GameObject.Find ("_Player");
		if (player) {
			qm = player.GetComponent<QuestManager>();
		}
		else {
			Debug.LogError("QuestManagerUI Script attached to 'QuestManager' object could not find a player in the scene!");
		}
		//questContainer = this.transform.Find ("Quests").gameObject;
		if (!questContainer) {
			Debug.LogError("QuestManagerUI Script attached to 'QuestManager' object could not find a child GameObject called 'Quests' the prefab connection could be broken.");
		}
		moreQuestInfo = transform.FindChild ("MoreQuestInfo").gameObject;
		if (!moreQuestInfo) {
			Debug.LogError ("QuestManagerUI script could not find the child object called 'MoreQuestInfo' the prefab may be broken");
		}
		moreQuestInfoTitle = moreQuestInfo.transform.FindChild ("QuestTitle").GetComponent<Text> ();
		if (!moreQuestInfoTitle) {
			Debug.LogError ("QuestManagerUI script could not find the child object called 'QuestTitle' the prefab may be broken");
		}
		moreQuestInfoDescription = moreQuestInfo.transform.FindChild ("ScrollView").transform.FindChild ("QuestDescription").GetComponent<Text> ();
		if (!moreQuestInfoDescription) {
			Debug.LogError ("QuestManagerUI script could not find the child object called 'QuestDescription' the prefab may be broken");
		}
		questButton = questUI.GetComponent<Button> ();
		buttonHeight = questButton.GetComponent<RectTransform> ().sizeDelta.y;
		qm.LoadQuests ();
	}

	void Update(){
		//For debugging, remove later.
		if (Input.GetKeyDown (KeyCode.F5)) {
			showQuests();
		}
	}

	public void showQuests(){
		removeQuestUIobjects ();
		reorganizeQuests ();
	}

	public void addNewQuest(){

	}

	/* This function is called when the quest UI elements need to be reorganized.
	 * Some times when this would be called is when attempting to add a quest or a quest is completed.
	 * This handles the calculation of the size of the scrollable area and physical placement of the quest UI elements in the quest manager.
	 * This function will also call the sort method on the Quests list to organize by completion. Completed items will fall to the bottom.
	 */
	public void reorganizeQuests(){

		moreQuestInfoTitle.text = "";
		moreQuestInfoDescription.text = "";
		float qcHeight;
		qcHeight = (qm.questCount * (buttonHeight + spacing) - spacing);
		RectTransform containerTransform = questContainer.GetComponent<RectTransform> ();
		containerTransform.sizeDelta = new Vector2 (100, qcHeight);

		for (int i = 0; i < qm.questCount; i++) {
			GameObject newQuestButton = Instantiate(questUI, new Vector3(0, 0 - (i * (buttonHeight + spacing) + (buttonHeight/2)), 0), Quaternion.identity) as GameObject;
			newQuestButton.transform.SetParent(questContainer.transform, false);
			Text newButtonText = newQuestButton.transform.FindChild ("Text").GetComponent<Text>();
			newButtonText.text = qm.currentQuests[i].GetName();
			Button qb = newQuestButton.GetComponent<Button>();
			addListener(qb, i);
			if (i == 0){
				qb.Select();
			}
		}
		if (qm.questCount > 0) {
			moreQuestInfoTitle.text = qm.currentQuests [0].GetName ();
			Goal[] currQuestGoals = qm.currentQuests[0].GetGoal();
			string goalText = "\n";
			if(qm.currentQuests[0].HasTimer()){
				goalText += "Quest Duration: " + qm.currentQuests[0].GetTimerLength() + " Seconds";
			}
			for(int i = 0; i < currQuestGoals.Length; i++){
				goalText += "\n" + currQuestGoals[i].GetName() + " - " + currQuestGoals[i].GetProgress() + " / " + currQuestGoals[i].GetProgrssNeeded();
			}
			moreQuestInfoDescription.text = qm.currentQuests [0].GetDescription () + goalText;
		}
	}

	void addListener(Button b, int i){
		b.onClick.AddListener (() => clickButton (i));
	}

	void clickButton(int iter){
		moreQuestInfoTitle.text = qm.currentQuests [iter].GetName ();
		Goal[] currQuestGoals = qm.currentQuests[iter].GetGoal();
		string goalText = "\n";
		for(int i = 0; i < currQuestGoals.Length; i++){
			goalText += "\n" + currQuestGoals[i].GetName() + " - " + currQuestGoals[i].GetProgress() + " / " + currQuestGoals[i].GetProgrssNeeded();
		}
		moreQuestInfoDescription.text = qm.currentQuests [iter].GetDescription () + goalText;
	}

	public void removeQuestUIobjects(){
		foreach (Transform child in questContainer.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
}
