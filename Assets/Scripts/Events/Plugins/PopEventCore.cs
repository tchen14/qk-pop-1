//using UnityEngine;
//using System.Diagnostics;

public class EventVisibleAttribute : System.Attribute { }

public static class PopEventCore {

    public static string[] watchTypes = { "Choose A Condition", "Watch Script", "Player Enters Area", "Player Leaves Area", "Wait X Seconds" };
    public static string[] executeTypes = { "Choose An Action", "Execute Function", "Activate Next Event", "Debug Message", "Create Text Box" };
}