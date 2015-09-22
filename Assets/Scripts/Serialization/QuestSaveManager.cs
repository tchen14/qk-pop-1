using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;
using Debug = FFP.Debug;
 
public class QuestSaveManager : SaveManager {

	QuestManager _questManager;
	Quest _quest;

	void SaveQuests() {
		_questManager = (QuestManager)FindObjectOfType (typeof(QuestManager));
		List<Quest> currQuests = _questManager.CurrentQuests ();

		if (currQuests == null || currQuests.Count == 0) {
			return;
		}

		JSONClass questJSONNode = new JSONClass ();

		int count = 0;
		foreach (Quest q in currQuests) {
			questJSONNode["Quests"][count] = q.GetID().ToString();
			count++;
		}

		PlayerPrefs.SetString ("PlayerQuests", questJSONNode.ToString ());
	}

	public List<Quest> LoadQuests() {
		_quest = new Quest (null, null, null, -1);
		List<Quest> quests = new List<Quest>();
		JSONNode loadedQuests = JSONClass.Parse(PlayerPrefs.GetString("PlayerQuests"));

		for (int i = 0; i < loadedQuests["Quests"].Count; i++) {
			Quest newQuest = _quest.AddQuest(loadedQuests["Quests"][i].AsInt);
			quests.Add(newQuest);
		}

		return quests;
	}
}