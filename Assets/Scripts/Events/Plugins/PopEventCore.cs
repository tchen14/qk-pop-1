//using UnityEngine;
//using System.Diagnostics;
using System.Collections.Generic;

public class EventVisibleAttribute : System.Attribute { }

public static class PopEventCore {

    public static string[] watchTypes = { "Choose A Condition", "Watch Script", "Player Enters Area", "Player Leaves Area", "Wait X Seconds", "Collect X Items" };
    public static string[] executeTypes = { "Choose An Action", "Execute Function", "Debug Message", "Activate Next Event", "Activate Another Event", "Deactivate Another Event", "Create Prefab", "Create Text Box", "Destroy Text Box", "Add X Items", "Destroy This Object" };
}