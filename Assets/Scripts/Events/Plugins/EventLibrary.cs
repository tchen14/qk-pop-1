/*











						This script has been automatically generated.
						Do not alter it, or your changes will be undone.














*/
using System.Collections.Generic;
public static class EventLibrary {

	public static Dictionary<string, System.Type> staticClasses = new Dictionary<string, System.Type> {
	};

	public static string[] staticClassesNice = new string[] {};

	public static Dictionary<string, System.Type> monoClasses = new Dictionary<string, System.Type> {
		{ "AIMain", typeof(AIMain) },
		{ "AudioManager", typeof(AudioManager) },
		{ "QuestManager", typeof(QuestManager) },
		{ "Crate", typeof(Crate) },
		{ "Enemy", typeof(Enemy) },
		{ "Rope", typeof(Rope) },
		{ "Well", typeof(Well) },
		{ "GameHUD", typeof(GameHUD) },
	};

	public static string[] monoClassesNice = new string[] { "AIMain",  "AudioManager",  "QuestManager",  "Crate",  "Enemy",  "Rope",  "Well",  "UI", };

	public static Dictionary<string, bool> instanceClasses = new Dictionary<string, bool> {
		{ "AIMain", false},
		{ "AudioManager", false},
		{ "QuestManager", false},
		{ "Crate", false},
		{ "Enemy", false},
		{ "Rope", false},
		{ "Well", false},
		{ "GameHUD", false},
	};

	public static Dictionary<string, string[]> library = new Dictionary<string, string[]> {
		{ "AudioManagerMethods", new string[] {"testPrint", "changeVol", "seeVol", } },
		{ "QuestManagerMethods", new string[] {"LoadQuests", "SaveQuests", "UpdateQuests", "CompleteGoalInQuest", "ProgressGoalInQuest", "AddQuest", } },
		{ "CrateMethods", new string[] {"TestCrateFunction", } },
		{ "GameHUDMethods", new string[] {"UpdateObjectiveText", "SetDialogueBoxText", "HideDialogueBoxText", } },
		{ "AudioManagerFields", new string[] {"testInt", } },
		{ "CrateFields", new string[] {"temp", "pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "EnemyFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "ItemFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "RopeFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "WellFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
	};

	public static Dictionary<string, string[]> libraryNice = new Dictionary<string, string[]> {
		{ "AudioManagerMethods", new string[] {"testPrint", "changeVol", "seeVol", } },
		{ "QuestManagerMethods", new string[] {"LoadQuests", "SaveQuests", "UpdateQuests", "CompleteGoalInQuest", "ProgressGoalInQuest", "AddQuest", } },
		{ "CrateMethods", new string[] {"test", } },
		{ "GameHUDMethods", new string[] {"UpdateObjectiveText", "SetDialogueBoxText", "HideDialogueBoxText", } },
		{ "AudioManagerFields", new string[] {"testInt", } },
		{ "CrateFields", new string[] {"temp", "Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "EnemyFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "ItemFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "RopeFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "WellFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
	};
}