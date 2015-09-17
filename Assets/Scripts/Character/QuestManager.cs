using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	List <Quest> currentQuests;
	Quest _quest;

	void Start() {
		_quest = new Quest (null, null, null);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.J)) {
			Quest newQuest = _quest.AddQuest(4);
			if(newQuest != null) {
				currentQuests.Add(newQuest);
			}
		}
	}

}
