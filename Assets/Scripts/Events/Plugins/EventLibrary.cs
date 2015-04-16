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
		{ "TestScriptLeftMethods", new string[] {"FunctionOne", "FunctionTwo", } },
		{ "TestScriptRightMethods", new string[] {"NullFunction", "IntFunction", "FloatFunction", "VectorFunction", "GameObjectFunction", } },
		{ "TestScriptRightStaticMethods", new string[] {"GameObjectFunction", } },
		{ "TestScriptLeftFields", new string[] {"counter", "fieldA", "fieldB", "trueFalse", "vectorA", } },
		{ "CrateFields", new string[] {"isDirty", } },
	};

	public static Dictionary<string, string[]> libraryNice = new Dictionary<string, string[]> {
		{ "TestScriptLeftMethods", new string[] {"Function One", "FunctionTwo", } },
		{ "TestScriptRightMethods", new string[] {"NullFunction", "IntFunction", "FloatFunction", "VectorFunction", "GameObjectFunction", } },
		{ "TestScriptRightStaticMethods", new string[] {"Print \"Debug\"", } },
		{ "TestScriptLeftFields", new string[] {"Nice Counter", "fieldA", "fieldB", "trueFalse", "vectorA", } },
		{ "CrateFields", new string[] {"isDirty", } },
	};
}