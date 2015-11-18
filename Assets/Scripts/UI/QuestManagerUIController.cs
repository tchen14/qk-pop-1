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
		showQuests ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.F5)) {
			Debug.Log ("Refreshing Quests");
			reorganizeQuests();
		}
	}

	public void showQuests(){
		Debug.Log ("Showing Quests");
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

		//Debug.Log("reorganizing!");
		Debug.Log (qm.questCount.ToString ());

		float qcHeight;
		qcHeight = (qm.questCount * (buttonHeight + spacing) - spacing);
		RectTransform containerTransform = questContainer.GetComponent<RectTransform> ();
		containerTransform.sizeDelta = new Vector2 (100, qcHeight);

		for (int i = 0; i < qm.questCount; i++) {
			GameObject newQuestButton = Instantiate(questUI, new Vector3(0, 0 - (i * (buttonHeight + spacing) + (buttonHeight/2)), 0), Quaternion.identity) as GameObject;
			newQuestButton.transform.SetParent(questContainer.transform, false);
			Text newButtonText = newQuestButton.transform.FindChild ("Text").GetComponent<Text>();
			newButtonText.text = qm.currentQuests[i].GetName();
		}
		if (qm.questCount > 0) {
			moreQuestInfoTitle.text = qm.currentQuests [0].GetName ();
			Goal[] currQuestGoals = qm.currentQuests[0].GetGoal();
			string goalText = "\n\n";
			for(int i = 0; i < currQuestGoals.Length; i++){
				goalText += currQuestGoals[i].GetName() + " - " + currQuestGoals[i].GetProgress() + " / " + currQuestGoals[i].GetProgrssNeeded() + "\n";
			}
			moreQuestInfoDescription.text = qm.currentQuests [0].GetDescription () + goalText;
			Debug.Log ("done reorganizing!");
		}
		/*Debug.Log ("Reorganizing Quests");
		float qcHeight = questContainer.GetComponent<RectTransform> ().sizeDelta.y; 
		qcHeight = (qm.questCount * (buttonHeight + spacing)) - spacing;
		Debug.Log(qcHeight.ToString());
		float qcPos = questContainer.GetComponent<RectTransform> ().position.y;
		qcPos = 0 - (questContainer.GetComponent<RectTransform> ().sizeDelta.y / 2);
		for (int i = 0; i < qm.questCount; i++) {
			Instantiate(questUI, new Vector3(0, i * (buttonHeight + spacing), 0), Quaternion.identity);
			questUI.transform.SetParent(questContainer.transform, false);
			questUI.transform.FindChild ("QuestName").GetComponent<Text>().text = qm.currentQuests[i].GetName();
		}
		Debug.Log ("Fin");*/
	}
}
