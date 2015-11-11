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
		questButton = questUI.transform.FindChild ("Button").gameObject.GetComponent<Button> ();
		buttonHeight = questButton.GetComponent<RectTransform> ().sizeDelta.y;
		showQuests ();
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
		Debug.Log ("Reorganizing Quests");
		float qcHeight = questContainer.GetComponent<RectTransform> ().sizeDelta.y; 
		qcHeight = (qm.questCount * (buttonHeight + spacing)) - spacing;
		float qcPos = questContainer.GetComponent<RectTransform> ().position.y;
		qcPos = 0 - (questContainer.GetComponent<RectTransform> ().sizeDelta.y / 2);
		for (int i = 0; i < qm.questCount; i++) {
			Instantiate(questUI, new Vector3(0, i * (buttonHeight + spacing), 0), Quaternion.identity);
			questUI.transform.SetParent(questContainer.transform, false);
			questUI.transform.FindChild ("QuestName").GetComponent<Text>().text = qm.currentQuests[i].GetName();
		}
		Debug.Log ("Fin");
	}
}
