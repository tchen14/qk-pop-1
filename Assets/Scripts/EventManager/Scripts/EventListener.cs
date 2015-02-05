using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventListener {

    public static List<EventCouple> coupleScripts = new List<EventCouple>();

    public static void AddCouple(EventCouple newCoupleScript) {
        coupleScripts.Add(newCoupleScript);
    }

    public static void Report(MonoBehaviour triggeringScript, string keyword) {
        foreach (var s in coupleScripts) {
            s.Report(triggeringScript, keyword);
        }
    }

    public static void Report(MonoBehaviour triggeringScript, string keyword, int value) {
        foreach (var s in coupleScripts) {
            s.Report(triggeringScript, keyword, value);
        }
    }
}

public class MethodEventAttribute : System.Attribute {

}