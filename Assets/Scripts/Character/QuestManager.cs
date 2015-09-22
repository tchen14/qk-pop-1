using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	List <Quest> currentQuests;
	Quest _quest;
	QuestSaveManager _questSaveManager;

	void Start() {
		_quest = new Quest (null, null, null, -1);
		currentQuests = new List<Quest> ();
		_questSaveManager = (QuestSaveManager)FindObjectOfType (typeof(QuestSaveManager));
	}

	void LoadQuests(List<Quest> loadedQuests) {
		currentQuests = loadedQuests;
		Debug.Log (currentQuests.Count + " quests loaded!");

		return;
	}

	public List<Quest> CurrentQuests() {
		return currentQuests;
	}
}
