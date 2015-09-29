using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


public class Quest {

	const string questListFilePath = "/Resources/questList.json";

	int iden;
	string name;
	string description;
	string objective;
	Goal[] goal;
	int progress = 0;
	int currTimer = 0;
	int duration;
	bool timer;

	public Quest(string n, string d, string o, int i, Goal[] newGoals ) {
		name = n;
		description = d;
		objective = o;
		iden = i;
		goal = newGoals;
	}

	public Quest (string n, string d, string o, int t, int i, Goal[] newGoals) {
		name = n;
		description = d;
		objective = o;
		duration = t;
		iden = i;
		goal = newGoals;
	}

	public Quest AddQuest(int id) {
		JSONNode quests = RetrieveQuestsFromJSON ();
		 


		if (quests[id.ToString()] == null) {
			Debug.LogError("Quest ID: " + id + " doesn't exist! Make sure it is added into the JSON file.");
			return null;
		}

		if (quests [id.ToString()].Count == 4) {

			Goal[] newGoals = new Goal[quests[id.ToString ()][3].Count];

			for(int i = 0; i < quests[id.ToString ()][3].Count; i++) {
				Goal newGoal = new Goal();
				newGoals[i] = newGoal;
			}

			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], id, newGoals);
			return newQuest;
		}

		if (quests [id.ToString()].Count == 5) {
			Goal[] newGoals = new Goal[quests[id.ToString ()][4].Count];

			for(int i = 0; i < quests[id.ToString ()][4].Count; i++) {
				Goal newGoal = new Goal();
				newGoals[i] = newGoal;
			}

			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], quests[id.ToString ()][3].AsInt, id, newGoals);
			return newQuest;
		}

		return null;
	}

	private JSONNode RetrieveQuestsFromJSON() {

		if(!System.IO.File.Exists(Application.dataPath + "/Resources/questList.json"))
		{
			Debug.LogError ("Could not find Quest List JSON file");
			return null;
		}

		string jsonRead = System.IO.File.ReadAllText(Application.dataPath + "/Resources/questList.json");
		JSONNode jsonParsed = JSON.Parse (jsonRead);

		return jsonParsed;

	}

	public int GetID() {
		return iden;
	}

	public string GetName() {
		return name;
	}

	public bool IsCompleted() {
		bool allCompleted = true;
		foreach (Goal g in goal) {
			if(g.IsCompleted() == false) {
				allCompleted = false;
				break;
			}
		}

		return allCompleted;
	}

	public void CompleteGoalInQuest(int goalIndex) {
		if (goal [goalIndex] == null) {
			Debug.Log("Goal " + goalIndex + " does not exist!");
			return;
		}

		if (goal [goalIndex].IsCompleted () == true) {
			Debug.Log("Goal already completed!");
		}

		goal [goalIndex].Complete ();
		return;
	}

	public Goal[] GetGoal() {
		return goal;
	}
}