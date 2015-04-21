/*











						This script has been automatically generated.
						Do not alter it, or your changes will be undone.














*/
using System.Collections.Generic;
public static class EventLibrary {

	public static Dictionary<string, System.Type> staticClasses = new Dictionary<string, System.Type> {
		{ "TestScriptRightStatic", typeof(TestScriptRightStatic) },
	};

	public static string[] staticClassesNice = new string[] { "Test Static Class", };

	public static Dictionary<string, System.Type> monoClasses = new Dictionary<string, System.Type> {
		{ "AIMain", typeof(AIMain) },
		{ "TestScriptLeft", typeof(TestScriptLeft) },
		{ "TestScriptRight", typeof(TestScriptRight) },
		{ "Crate", typeof(Crate) },
		{ "Enemy", typeof(Enemy) },
		{ "Rope", typeof(Rope) },
		{ "Well", typeof(Well) },
	};

	public static string[] monoClassesNice = new string[] { "AIMain",  "TestScriptLeft",  "TestScriptRight",  "Crate",  "Enemy",  "Rope",  "Well", };

	public static Dictionary<string, string[]> library = new Dictionary<string, string[]> {
		{ "AIMainMethods", new string[] {"SetAgression", "ChangeNavPoint", } },
		{ "AudioManagerMethods", new string[] {"changeVol", "seeVol", } },
		{ "TestScriptLeftMethods", new string[] {"FunctionOne", "FunctionTwo", } },
		{ "TestScriptRightMethods", new string[] {"NullFunction", "IntFunction", "FloatFunction", "VectorFunction", "GameObjectFunction", } },
		{ "TestScriptRightStaticMethods", new string[] {"GameObjectFunction", "IntFunction", "TwoStrings", "Several", } },
		{ "AIMainFields", new string[] {"seesTarget", "panic", } },
		{ "TestScriptLeftFields", new string[] {"counter", "fieldA", "fieldB", "trueFalse", "dictionary", "testString", "vectorA", } },
		{ "TestScriptRightStaticFields", new string[] {"field", } },
		{ "CrateFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "EnemyFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "ItemFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "RopeFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
		{ "WellFields", new string[] {"pushCounter", "pullCounter", "cutCounter", "soundThrowCounter", "stunCounter", "quincAffected", } },
	};

	public static Dictionary<string, string[]> libraryNice = new Dictionary<string, string[]> {
		{ "AIMainMethods", new string[] {"SetAgression", "ChangeNavPoint", } },
		{ "AudioManagerMethods", new string[] {"changeVol", "seeVol", } },
		{ "TestScriptLeftMethods", new string[] {"Function One", "FunctionTwo", } },
		{ "TestScriptRightMethods", new string[] {"NullFunction", "IntFunction", "FloatFunction", "VectorFunction", "GameObjectFunction", } },
		{ "TestScriptRightStaticMethods", new string[] {"Print \"Debug\"", "Pass Int", "Two Strings", "Several Parameters", } },
		{ "AIMainFields", new string[] {"seesTarget", "panic", } },
		{ "TestScriptLeftFields", new string[] {"Nice Counter", "fieldA", "fieldB", "trueFalse", "dictionary", "testString", "vectorA", } },
		{ "TestScriptRightStaticFields", new string[] {"field", } },
		{ "CrateFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "EnemyFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "ItemFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "RopeFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
		{ "WellFields", new string[] {"Pushed X Times", "Pulled X Times", "Cut X Times", "Sound Thrown X Times", "Stunned X Times", "Affected by QuinC", } },
	};
}