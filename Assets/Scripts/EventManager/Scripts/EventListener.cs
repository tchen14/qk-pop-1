using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventListener {

    public static List<EventCouple> coupleScripts = new List<EventCouple>();

    public static void AddCouple(EventCouple newCoupleScript) {
        coupleScripts.Add(newCoupleScript);
    }

    public static void SlowUpdate(EventCouple s) {

        if (s.conditionType == typeof(System.Int32)) {
            int intValue = (int)s.conditionScript.GetType().GetField(s.conditionField).GetValue(s.conditionScript);
            if (intValue == s.conditionInt) {
                InvokeAction(s);
            }
        }
        else if (s.conditionType == typeof(System.Single)) {
            float floatValue = (float)s.conditionScript.GetType().GetField(s.conditionField).GetValue(s.conditionScript);
            if (floatValue >= s.conditionFloat) {
                InvokeAction(s);
            }
        }
    }

    public static void InvokeAction(EventCouple s) {

        if (s.actionType == typeof(void)) {
            s.actionScript.GetType().GetMethod(s.actionName).Invoke(s.actionScript, null);
        }
        else if (s.actionType == typeof(System.Int32)) {
            s.actionScript.GetType().GetMethod(s.actionName).Invoke(s.actionScript, new object[] {s.actionInt});
        }
        else if (s.actionType == typeof(Vector3)) {
            s.actionScript.GetType().GetMethod(s.actionName).Invoke(s.actionScript, new object[] { s.actionVector3 });
        }
    }
}