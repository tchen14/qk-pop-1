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

	public Quest(string n, string d, string o, int i) {
		name = n;
		description = d;
		objective = o;
		iden = i;
	}

	public Quest (string n, string d, string o, int t, int i) {
		name = n;
		description = d;
		objective = o;
		duration = t;
		iden = i;
	}

	public Quest AddQuest(int id) {
		JSONNode quests = RetrieveQuestsFromJSON ();
		 


		if (quests[id.ToString()] == null) {
			Debug.LogError("Quest ID: " + id + " doesn't exist! Make sure it is added into the JSON file.");
			return null;
		}

		if (quests [id.ToString()].Count == 3) {
			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], id);
			return newQuest;
		}

		if (quests [id.ToString()].Count == 4) {
			Quest newQuest = new Quest(quests[id.ToString ()][0], quests[id.ToString ()][1], quests[id.ToString ()][2], quests[id.ToString ()][3].AsInt, id);
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

		Debug.Log (jsonRead);

		return jsonParsed;

	}

	public int GetID() {
		return iden;
	}

	public bool IsCompleted() {

	}

	public Goal[] GetGoal() {
		return goal;
	}
}