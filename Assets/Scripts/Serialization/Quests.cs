using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


// This contains the base Quest class 

public class Quest {

	const string questListFilePath = "/Resources/questList.json";

	int iden;
	string name;
	string description;
	string objective;
	Goal[] goal;
	int progress = 0;
	int duration;
	bool timer = false;
	bool failed = false;

	//Quest constructor without a Timer
	public Quest(string n, string d, string o, int i, Goal[] newGoals ) {
		name = n;
		description = d;
		objective = o;
		iden = i;
		goal = newGoals;
	}

	//Quest constructor with a Timer
	public Quest (string n, string d, string o, int t, int i, Goal[] newGoals) {
		name = n;
		description = d;
		objective = o;
		duration = t;
		iden = i;
		goal = newGoals;
		timer = true;
	}

	/* Creates quest based on ID
	 * Looks for ID in questList.json
	 * Takes parsed json file and fills in Quest information
	 */
	public Quest AddQuest(int id) {
		JSONNode quests = RetrieveQuestsFromJSON ();

		if (quests[id.ToString()] == null) {
			Debug.LogError("Quest ID: " + id + " doesn't exist! Make sure it is added into the JSON file.");
			return null;
		}
		if (quests [id.ToString()].Count == 4) {

			Goal[] newGoals = new Goal[quests[id.ToString ()][3].Count];

			for(int i = 0; i < quests[id.ToString ()][3].Count; i++) {

				if(quests[id.ToString ()][3][i][0] == null) {
					Goal newGoal = new Goal(quests[id.ToString ()][3][i]);
					newGoals[i] = newGoal;
				}
				else {
					Goal newGoal = new Goal(quests[id.ToString ()][3][i], quests[id.ToString ()][3][i][0].AsInt);
					Debug.Log(quests[id.ToString ()][3][i].Keys);
					newGoals[i] = newGoal;
				}
			}
			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], id, newGoals);
			return newQuest;
		}

		if (quests [id.ToString()].Count == 5) {
			Goal[] newGoals = new Goal[quests[id.ToString ()][4].Count];

			for(int i = 0; i < quests[id.ToString ()][4].Count; i++) {
				Goal newGoal = new Goal(quests[id.ToString ()][i]);
				newGoals[i] = newGoal;
			}
			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], quests[id.ToString ()][3].AsInt, id, newGoals);
			return newQuest;
		}
		Debug.Log ("Not the right amount of parameters in questList.json for Quest ID:" + id);
		return null;
	}

	/* Basically a modified version of AddQuest()
	 * Only thing different is it loads progress saved
	 */
	public Quest AddQuestFromSave(int id, int[] progress) {
		JSONNode quests = RetrieveQuestsFromJSON ();
		
		
		
		if (quests[id.ToString()] == null) {
			Debug.LogError("Quest ID: " + id + " doesn't exist! Make sure it is added into the JSON file.");
			return null;
		}
		if (quests [id.ToString()].Count == 4) {
			
			Goal[] newGoals = new Goal[quests[id.ToString ()][3].Count];

			for(int i = 0; i < quests[id.ToString ()][3].Count; i++) {
				
				if(quests[id.ToString ()][3][i][0] == null) {
					Goal newGoal = new Goal(quests[id.ToString ()][3][i]);
					newGoals[i] = newGoal;
				}
				else {
					Goal newGoal = new Goal(quests[id.ToString ()][3][i], quests[id.ToString ()][3][i][0].AsInt, progress[i]);
					newGoals[i] = newGoal;
				}
			}
			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], id, newGoals);
			return newQuest;
		}
		if (quests [id.ToString()].Count == 5) {
			Goal[] newGoals = new Goal[quests[id.ToString ()][4].Count];
			
			for(int i = 0; i < quests[id.ToString ()][4].Count; i++) {
				Goal newGoal = new Goal(quests[id.ToString ()][i]);
				newGoals[i] = newGoal;
			}
			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], quests[id.ToString ()][3].AsInt, id, newGoals);
			return newQuest;
		}
		return null;
	}

	// This opens and reads the questList.json file and returns the parsed information
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

	//Returns Quest ID
	public int GetID() {
		return iden;
	}

	//Returns Quest Name
	public string GetName() {
		return name;
	}

	//Returns true if all Goals in a quest if complete, false if any are not
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

	// Use to complete a certain goal in quest
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
	// Use to progress a certain goal in quest
	public void ProgressGoalInQuest(int goalIndex) {
		if (goal [goalIndex] == null) {
			Debug.Log("Goal " + goalIndex + " does not exist!");
			return;
		}
		
		if (goal [goalIndex].IsCompleted () == true) {
			Debug.Log("Goal already completed!");
		}
		
		goal [goalIndex].Progress ();
		return;
	}

	//Returns array of a goals in quest
	public Goal[] GetGoal() {
		return goal;
	}

	//Returns true if quest has a Timer
	public bool HasTimer() {
		return timer;
	}

	//Return Timer's length
	public int GetTimerLength() {
		return duration;
	}

	//Fails quest
	public void Fail() {
		failed = true;
		return;
	}

	//Returns true if Quest is failed
	public bool IsFailed() {
		return failed;
	}
}