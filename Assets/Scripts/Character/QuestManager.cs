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

	[EventVisibleAttribute]
	public void LoadQuests() {
		currentQuests = _questSaveManager.LoadQuests();
		Debug.Log (currentQuests.Count + " quests loaded!");
		return;
	}

	[EventVisibleAttribute]
	public void SaveQuests() {
		_questSaveManager.SaveQuests (currentQuests);
		Debug.Log ("Quests saved!");
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
				_questSaveManager.SaveCompletedQuest(currentQuests[count]);
				currentQuests.RemoveAt(count);
			}
		}

		return;
	}

	[EventVisibleAttribute]
	public void CompleteGoalInQuest(int questID, int goalIndex) {
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

	[EventVisibleAttribute]
	public void ProgressGoalInQuest(int questID, int goalIndex) {
		if (currentQuests.Count < 1) {
			Debug.Log("No quests in List!");
			return;
		}

		foreach (Quest q in currentQuests) {
			if(q.GetID() == questID) {
				q.ProgressGoalInQuest(goalIndex);
			}
		}
	}

	void Update() {
		questCount = currentQuests.Count;
	}

	[EventVisibleAttribute]
	public void AddQuest(int questID) {

		if (_questSaveManager.CompletedQuest (questID) == true) {
			Debug.Log("Quest has already been completed. Delete in PlayerPrefs probably");
			return;
		}

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
