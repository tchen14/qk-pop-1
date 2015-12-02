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


	/* Asks QuestSaveManager to load quests from PlayerPrefs
	 */
	[EventVisibleAttribute]
	public void LoadQuests() {

		List<Quest> newQuestList = _questSaveManager.LoadQuests();
		if (newQuestList != null) {
			currentQuests = newQuestList;
		}
		Debug.Log (currentQuests.Count + " quests loaded!");
		return;
	}

	//Sends Current Quests to QuestSaveManager to save in PlayerPrefs
	[EventVisibleAttribute]
	public void SaveQuests() {
		_questSaveManager.SaveQuests (currentQuests);
		Debug.Log ("Quests saved!");
		return;
	}

	//Returns list of current quests
	public List<Quest> CurrentQuests() {
		return currentQuests;
	}

	/* Checks if current quests are completed or failed
	 * Will remove completed or failed quests from List
	 */
	[EventVisibleAttribute]
	public void UpdateQuests() {

		//Debug.Log ("Checking Quests for Completion!");
		if (currentQuests.Count == 0) {
			Debug.Log("No quests in List");
			return;
		}

		for (int count = currentQuests.Count - 1; count > -1; count--) {

			if(currentQuests[count].IsFailed() == true) {
				Debug.Log(currentQuests[count].GetName() + " quest has failed and removed from List!");
				string iden = currentQuests[count].GetID().ToString();
				currentQuests.RemoveAt(count);
				EventListener.ActivateById(iden, false);
				continue;
			}

			if(currentQuests[count].IsCompleted() == true) {
				Debug.Log (currentQuests[count].GetName() + " quest is completed and removed from List!");
				_questSaveManager.SaveCompletedQuest(currentQuests[count]);
				currentQuests.RemoveAt(count);
			}
		}

		return;
	}

	//Completes a certain goal in certain quest
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

	//Progresses a certain goal in certain quest
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

	//Adds quest to current quest, sends to Quest base class to create
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

		foreach (Quest q in currentQuests) {
			if(q.GetID() == newQuest.GetID()) {
				Debug.Log("Trying to get quest player already has.");
				return;
			}
		}

		currentQuests.Add (newQuest);
		Debug.Log ("Added quest!");

		EventListener.ActivateById (newQuest.GetID ().ToString (), true);
	
		if (newQuest.HasTimer () == true) {

			StartCoroutine("StartTimer", newQuest);
		}

		return;
	}

	[EventVisibleAttribute]
	public void AddQuestWithPrerequisite(int questID, int prerequisite) {

		if (_questSaveManager.CompletedQuest (prerequisite) == true) {

			if (_questSaveManager.CompletedQuest (questID) == true) {
				Debug.Log ("Quest " + questID + " has already been completed. Delete in PlayerPrefs probably");
				return;
			}
			
			Quest newQuest = _quest.AddQuest (questID);
			
			if (newQuest == null) {
				Debug.Log ("New Quest is null. Not adding to List!");
				return;
			}
			
			foreach (Quest q in currentQuests) {
				if (q.GetID () == newQuest.GetID ()) {
					Debug.Log ("Trying to get quest player already has.");
					return;
				}
			}
			
			currentQuests.Add (newQuest);
			Debug.Log ("Added quest!");
			
			EventListener.ActivateById (newQuest.GetID ().ToString (), true);
			
			if (newQuest.HasTimer () == true) {
				
				StartCoroutine ("StartTimer", newQuest);
			}
			
			return;
		} else {
			Debug.Log("Prerequisite quest has not been completed!");
		}
	}

	[EventVisibleAttribute]
	public void FailQuest(int questID) {
		foreach (Quest q in currentQuests) {
			if(q.GetID() == questID) {
				q.Fail();
				break;
			}
		}
	}


	IEnumerator StartTimer(Quest q) {
		Debug.Log ("Starting timer for " + q.GetTimerLength() + " seconds.");

		yield return new WaitForSeconds ((float)q.GetTimerLength());

		if (q.IsCompleted () == false) {
			q.Fail();
			UpdateQuests();
		} else {
			UpdateQuests();
		}
	}
}
