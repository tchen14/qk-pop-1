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

	public static Dictionary<string, string[]> library = new Dictionary<string, string[]> {
		{ "AIMainMethods", new string[] {"SetAgression", "ChangeNavPoint", } },
		{ "AudioManagerMethods", new string[] {"changeVol", "seeVol", } },
		{ "TestScriptLeftMethods", new string[] {"FunctionOne", "FunctionTwo", } },
		{ "TestScriptRightMethods", new string[] {"NullFunction", "IntFunction", "FloatFunction", "VectorFunction", "GameObjectFunction", } },
		{ "TestScriptRightStaticMethods", new string[] {"GameObjectFunction", } },
		{ "AIMainFields", new string[] {"seesTarget", "panic", } },
		{ "TestScriptLeftFields", new string[] {"counter", "fieldA", "fieldB", "trueFalse", "vectorA", } },
	};

	public static Dictionary<string, string[]> libraryNice = new Dictionary<string, string[]> {
		{ "AIMainMethods", new string[] {"SetAgression", "ChangeNavPoint", } },
		{ "AudioManagerMethods", new string[] {"changeVol", "seeVol", } },
		{ "TestScriptLeftMethods", new string[] {"Function One", "FunctionTwo", } },
		{ "TestScriptRightMethods", new string[] {"NullFunction", "IntFunction", "FloatFunction", "VectorFunction", "GameObjectFunction", } },
		{ "TestScriptRightStaticMethods", new string[] {"Print \"Debug\"", } },
		{ "AIMainFields", new string[] {"seesTarget", "panic", } },
		{ "TestScriptLeftFields", new string[] {"Nice Counter", "fieldA", "fieldB", "trueFalse", "vectorA", } },
	};
}