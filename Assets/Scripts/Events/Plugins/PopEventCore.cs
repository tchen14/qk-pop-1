using UnityEngine;
//using System.Diagnostics;
using System.Collections.Generic;

public class EventVisibleAttribute : System.Attribute {
    public string niceName = "";
    public EventVisibleAttribute(string name) {
        niceName = name;
    }
    public EventVisibleAttribute() { }
}

public static class PopEventCore {

    public static Dictionary<string, string[]> watchLibrary = new Dictionary<string, string[]> {
		{ "System", new string[] { "Choose A Condition", "Wait X Seconds", "Player Enters Area", "Player Leaves Area" } },
		//{ "Player", new string[] { "Choose A Condition", "Player Enters Area", "Player Leaves Area" } },
		//{ "Item", new string[] { "Choose A Condition", "Collect X Items" } },
		{ "Object Script", new string[] { "" } },
		//{ "Static Script", new string[] { "" } },
        /*
		{ "Object", new string[] { "Choose A Condition" } },
		{ "GUI", new string[] { "Choose A Condition" } },
		{ "Sound", new string[] { "Choose A Condition" } },
         */
	};

    public static Dictionary<string, string[]> executeLibrary = new Dictionary<string, string[]> {
		{ "System", new string[] { 	"Choose An Action", "Debug Message", "Activate Next Event", "Activate Another Event", "Deactivate Another Event",
									"Choose An Action", "Create Prefab At Position", "Create Prefab Here", "Destroy This Object", "Move This Object" } },
		//{ "Player", new string[] { "Choose An Action", "Move Player To Location" } },
		//{ "Item", new string[] { "Choose An Action", "Add X Items" } },
		//{ "Object", new string[] { "Choose An Action", "Create Prefab At Position", "Create Prefab Here", "Destroy This Object", "Move This Object" } },
		//{ "GUI", new string[] { "Choose An Action", "Set Objective", "Set Dialogue" } },
		//{ "Audio", new string[] { "Choose An Action", "Play Sound" } },
        //{ "Object Script", new string[] { "" } },
		//{ "Static Script", new string[] { "" } },
	};
}