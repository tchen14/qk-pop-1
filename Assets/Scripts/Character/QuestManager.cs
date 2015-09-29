using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[EventVisibleAttribute]
public class QuestManager : MonoBehaviour {

	List <Quest> currentQuests;
	Quest _quest;
	QuestSaveManager _questSaveManager;
	public int questCount;

	void Start() {
		_quest = new Quest (null, null, null, -1, null);
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

	[EventVisibleAttribute]
	public void UpdateQuests() {

		Debug.Log ("Checking Quests for Completion!");
		if (currentQuests.Count == 0) {
			Debug.Log("No quests in List");
			return;
		}



		for (int count = currentQuests.Count - 1; count > -1; count--) {
			if(currentQuests[count].IsCompleted() == true) {
				Debug.Log (currentQuests[count].GetName() + " quest is completed and removed from List!");
				currentQuests.RemoveAt(count);
			}
		}

		return;
	}

	[EventVisibleAttribute]
	public void CompleteGoalinQuest(int questID, int goalIndex) {
		if (currentQuests.Count < 1) {
			Debug.Log("No quests in List!");
			return;
		}

		foreach (Quest q in currentQuests) {
			if(q.GetID() == questID) {
				q.CompleteGoalInQuest(goalIndex);
			}
		}
	}

	void Update() {
		questCount = currentQuests.Count;
	}

	[EventVisibleAttribute]
	public void AddQuest(int questID) {
		Quest newQuest = _quest.AddQuest (questID);

		if (newQuest == null) {
			Debug.Log("New Quest is null. Not adding to List!");
			return;
		}

		currentQuests.Add (newQuest);
		Debug.Log ("Added quest!");
		return;
	}
}
