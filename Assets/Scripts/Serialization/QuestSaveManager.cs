using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;
using Debug = FFP.Debug;
 
public class QuestSaveManager : SaveManager {

	QuestManager _questManager;
	Quest _quest;

	public void SaveQuests(List<Quest> currQuests) {
		/*_questManager = (QuestManager)FindObjectOfType (typeof(QuestManager));
		List<Quest> currQuests = _questManager.CurrentQuests ();*/

		if (currQuests == null || currQuests.Count == 0) {
			return;
		}

		JSONClass questJSONNode = new JSONClass ();

		int count = 0;
		foreach (Quest q in currQuests) {
			questJSONNode["Quests"][count] = q.GetID().ToString();
			for(int i = 0; i < q.GetGoal().Count(); i++) {
				if(q.GetGoal()[i].GetProgress() != null) {
					questJSONNode["Quests"][count][i] = q.GetGoal()[i].GetProgress().ToString();
				}
				else {
					questJSONNode["Quests"][count][i] = "null";
				}
			}
			count++;
		}

		PlayerPrefs.SetString ("PlayerQuests", questJSONNode.ToString ());
	}

	public void SaveCompletedQuest(Quest questToSave) {
		if (PlayerPrefs.HasKey ("CompletedQuests") == true) {
			JSONNode completedQuests = JSONClass.Parse(PlayerPrefs.GetString("CompletedQuests"));
			completedQuests["CompletedQuests"][completedQuests["CompletedQuests"].Count] = questToSave.GetID().ToString();
			PlayerPrefs.SetString("CompletedQuests", completedQuests.ToString());
			return;
		}

		JSONClass completedQuestJSONNode = new JSONClass ();
		completedQuestJSONNode ["CompletedQuests"] [0] = questToSave.GetID ().ToString();
		PlayerPrefs.SetString("CompletedQuests", completedQuestJSONNode.ToString());
		return;

	}

	public bool CompletedQuest(int questID) {
		if (PlayerPrefs.HasKey ("CompletedQuests") == true) {
			JSONNode completedQuests = JSONClass.Parse(PlayerPrefs.GetString("CompletedQuests"));
			for(int i = 0; i < completedQuests["CompletedQuests"].Count; i++) {
				if(completedQuests["CompletedQuests"][i] == questID.ToString()) {
					return true;
				}
			}
		} else {
			return false;
		}
		return true;
	}

	public List<Quest> LoadQuests() {
		_quest = new Quest (null, null, null, -1, null);
		List<Quest> quests = new List<Quest>();
		JSONNode loadedQuests = JSONClass.Parse(PlayerPrefs.GetString("PlayerQuests"));

		for (int i = 0; i < loadedQuests["Quests"].Count; i++) {

			int[] progress = new int[loadedQuests["Quests"][i].Count];

			for(int j = 0; j < loadedQuests["Quests"][i].Count; j++) {
				progress[j] = loadedQuests["Quests"][i][j].AsInt;
			}

			Quest newQuest = _quest.AddQuestFromSave(loadedQuests["Quests"][i].AsInt, progress);
			quests.Add(newQuest);
		}

		return quests;
	}
}