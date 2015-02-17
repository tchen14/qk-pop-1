using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


public static class EventListener {

    public static void Report(MonoBehaviour mono) {
        StackFrame stackFrame = new StackFrame(1);
        MonoBehaviour.print(stackFrame.GetMethod().Name);
    }

    public static List<EventCouple> coupleScripts = new List<EventCouple>();

    public static void AddCouple(EventCouple newCoupleScript) {
        coupleScripts.Add(newCoupleScript);
    }


    public static void SlowUpdate(EventCouple couple) {

        foreach(var c in couple.conditions){
            if (c.conditionType == typeof(System.Int32)) {
                int intValue = (int)c.conditionScript.GetType().GetField(c.conditionField).GetValue(c.conditionScript);
                if (intValue == c.conditionInt) {
                    InvokeAction(couple);
                }
            }
            else if (c.conditionType == typeof(System.Single)) {
                float floatValue = (float)c.conditionScript.GetType().GetField(c.conditionField).GetValue(c.conditionScript);
                if (floatValue >= c.conditionFloat) {
                    InvokeAction(couple);
                }
            }
        }
    }

    public static void InvokeAction(EventCouple couple) {

        foreach (var a in couple.actions) {
            if (a.actionType == typeof(void)) {
                a.actionScript.GetType().GetMethod(a.actionName).Invoke(a.actionScript, null);
            }
            else if (a.actionType == typeof(System.Int32)) {
                a.actionScript.GetType().GetMethod(a.actionName).Invoke(a.actionScript, new object[] { a.actionInt });
            }
            else if (a.actionType == typeof(Vector3)) {
                a.actionScript.GetType().GetMethod(a.actionName).Invoke(a.actionScript, new object[] { a.actionVector3 });
            }
        }
    }
}