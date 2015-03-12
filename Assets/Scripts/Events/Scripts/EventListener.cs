using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventListener {


    public static void Report(MonoBehaviour mono, string value) {
        //StackFrame stackFrame = new StackFrame(1);
        //MonoBehaviour.print(stackFrame.GetMethod().Name);
    }

    public static List<EventCouple> coupleScripts = new List<EventCouple>();

    public static void SlowUpdate(EventCouple couple) {
        int numberOfConditions = 0;
        int testsPassed = 0;

        foreach(EventCondition condition in couple.conditions){
            if (condition.conditionScript != null) {
                numberOfConditions++;
                if (condition.conditionType == null) {
                    condition.conditionType = condition.conditionScript.GetType().GetField(condition.conditionField).FieldType;
                }
            }

            if (condition.conditionType == typeof(System.Int32)) {
                int intValue = (int)condition.conditionScript.GetType().GetField(condition.conditionField).GetValue(condition.conditionScript);
                if (Compare(intValue, condition.conditionInt, condition.numberCompareOption)) {
                    testsPassed++;
                }
            }
            else if (condition.conditionType == typeof(System.Single)) {
                float floatValue = (float)condition.conditionScript.GetType().GetField(condition.conditionField).GetValue(condition.conditionScript);
                if (Compare(floatValue, condition.conditionFloat, condition.numberCompareOption)) {
                    testsPassed++;
                }
            }
        }
        if (testsPassed >= numberOfConditions) {
            InvokeAction(couple);
        }
    }

    public static void InvokeAction(EventCouple couple) {
        foreach (EventAction action in couple.actions) {
            action.actionScript.GetType().GetMethod(action.actionName).Invoke(action.actionScript, action.args);
        }
    }


    /*!     Comparison functions        */
    public static bool Compare(int valueA, int valueB, EventCondition.NumberCompareOption compareOption) {
        if (compareOption == EventCondition.NumberCompareOption.EqualTo) {
            if (valueA == valueB) { return true; }
        }
        else if (compareOption == EventCondition.NumberCompareOption.GreaterThan) {
            if (valueA > valueB) { return true; }
        }
        else if (compareOption == EventCondition.NumberCompareOption.LessThan) {
            if (valueA < valueB) { return true; }
        }
        return false;
    }

    public static bool Compare(float valueA, float valueB, EventCondition.NumberCompareOption compareOption) {
        if (compareOption == EventCondition.NumberCompareOption.EqualTo) {
            if (valueA == valueB) { return true; }
        }
        else if (compareOption == EventCondition.NumberCompareOption.GreaterThan) {
            if (valueA > valueB) { return true; }
        }
        else if (compareOption == EventCondition.NumberCompareOption.LessThan) {
            if (valueA < valueB) { return true; }
        }
        return false;
    }

}