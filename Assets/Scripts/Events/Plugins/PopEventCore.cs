//using UnityEngine;
//using System.Diagnostics;
using System.Collections.Generic;

public class EventVisibleAttribute : System.Attribute { }

public static class PopEventCore {

    public static Dictionary<string, string[]> watchLibrary = new Dictionary<string, string[]> {
		{ "System", new string[] { "Choose A Condition", "Watch Script", "Wait X Seconds" } },
		{ "Player", new string[] { "Choose A Condition", "Player Enters Area", "Player Leaves Area" } },
		{ "Item", new string[] { "Choose A Condition", "Collect X Items" } },
        /*
		{ "Object", new string[] { "Choose A Condition" } },
		{ "GUI", new string[] { "Choose A Condition" } },
		{ "Sound", new string[] { "Choose A Condition" } },
         */
	};

    public static Dictionary<string, string[]> executeLibrary = new Dictionary<string, string[]> {
		{ "System", new string[] { "Choose An Action", "Execute Function", "Debug Message", "Activate Next Event", "Activate Another Event", "Deactivate Another Event" } },
		{ "Player", new string[] { "Choose An Action", "Move Player To Location" } },
		{ "Item", new string[] { "Choose An Action", "Add X Items" } },
		{ "Object", new string[] { "Choose An Action", "Create Prefab At Position", "Create Prefab Here", "Destroy This Object" } },
		{ "GUI", new string[] { "Choose An Action", "Create Text Box", "Destroy Text Box" } },
		{ "Sound", new string[] { "Choose An Action", "Play Sound" } },
	};

}