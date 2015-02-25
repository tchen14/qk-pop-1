using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventListener {

    public static void Report(MonoBehaviour mono) {
        //StackFrame stackFrame = new StackFrame(1);
        //MonoBehaviour.print(stackFrame.GetMethod().Name);
    }

    public static List<EventCouple> coupleScripts = new List<EventCouple>();

    public static void AddCouple(EventCouple newCoupleScript) {
        coupleScripts.Add(newCoupleScript);
    }


    public static void SlowUpdate(EventCouple couple) {

        int testsPassed = 0;
        foreach(var c in couple.conditions){
            if (c.conditionScript != null && c.conditionType == null) {
                c.conditionType = c.conditionScript.GetType().GetField(c.conditionField).FieldType;
            }

            if (c.conditionType == typeof(System.Int32)) {
                int intValue = (int)c.conditionScript.GetType().GetField(c.conditionField).GetValue(c.conditionScript);
                if (intValue == c.conditionInt) {
                    testsPassed++;
                }
            }
            else if (c.conditionType == typeof(System.Single)) {
                float floatValue = (float)c.conditionScript.GetType().GetField(c.conditionField).GetValue(c.conditionScript);
                if (floatValue >= c.conditionFloat) {
                    testsPassed++;
                }
            }
        }
        if (testsPassed == couple.conditions.Count) {
            InvokeAction(couple);

        }
    }

    public static void InvokeAction(EventCouple couple) {
        foreach (var a in couple.actions) {
            a.actionScript.GetType().GetMethod(a.actionName).Invoke(a.actionScript, a.args);
        }
    }

    public static bool Compare(int valueA, int valueB, EventCondition c) {
        //if (c.comparisonIndex
        return false;
    }
}