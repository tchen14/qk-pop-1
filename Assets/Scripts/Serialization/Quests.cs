using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


public class Quest {

	const string questListFilePath = "/Resources/questList.json";
	
	string name;
	string description;
	string objective;
	int progress = 0;
	int currTimer = 0;
	int duration;
	bool timer;

	public Quest(string n, string d, string o) {
		name = n;
		description = d;
		objective = o;
	}

	public Quest (string n, string d, string o, int t) {
		name = n;
		description = d;
		objective = o;
		duration = t;
	}

	public Quest AddQuest(int id) {
		JSONNode quests = RetrieveQuestsFromJSON ();

		if (quests[id.ToString()] == null) {
			Debug.LogError("Quest ID: " + id + " doesn't exist! Make sure it is added into the JSON file.");
			return null;
		}

		if (quests [id.ToString()].Count == 3) {
			Quest newQuest = new Quest(quests[id]["name"], quests[id]["description"], quests[id]["objective"]);
			return newQuest;
		}

		if (quests [id.ToString()].Count == 4) {
			Quest newQuest = new Quest(quests[id]["name"], quests[id]["description"], quests[id]["objective"], quests[id]["duration"].AsInt);
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

		return jsonRead;
	}
}