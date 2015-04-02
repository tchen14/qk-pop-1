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
            numberOfConditions++;
            //  Watch Script Type Condition
            if (condition.watchType == EventCondition.WatchType.WatchScript) {
                if (condition.conditionScript != null) {
                    numberOfConditions--;
                    if (condition.conditionType == null) {
                        condition.conditionType = condition.conditionScript.GetType().GetField(condition.conditionField).FieldType;
                    }
                }

                if (condition.conditionType == typeof(System.Int32)) {
                    int intValue = (int)condition.conditionScript.GetType().GetField(condition.conditionField).GetValue(condition.conditionScript);
                    if (Compare(intValue, condition.p_int, condition.numberCompareOption)) {
                        testsPassed++;
                    }
                }
                else if (condition.conditionType == typeof(System.Single)) {
                    float floatValue = (float)condition.conditionScript.GetType().GetField(condition.conditionField).GetValue(condition.conditionScript);
                    if (Compare(floatValue, condition.p_float, condition.numberCompareOption)) {
                        testsPassed++;
                    }
                }
            }
            //  Player Enters Area
            else if (condition.watchType == EventCondition.WatchType.PlayerEntersArea) {
                if (condition.p_Transform == null) {
                    condition.p_Transform = GameObject.Find("/_Player").transform;
                }
                if (Vector3.Distance(condition.p_Transform.position, couple.popEvent.conditionRegionCenter) <= couple.popEvent.conditionRegionRadius) {
                    testsPassed++;
                }
            }
            else if (condition.watchType == EventCondition.WatchType.WaitXSeconds) {
                if (couple.popEvent.totalTimeActive >= condition.p_float) {
                    testsPassed++;
                }
            }
        }
        if (couple.andOrCompare == EventCouple.AndOrCompare.EveryCondition) {
            if (testsPassed >= numberOfConditions) {
                InvokeAction(couple);
            }
        }
        else if (couple.andOrCompare == EventCouple.AndOrCompare.OneOrMore) {
            if (testsPassed >= 1) {
                InvokeAction(couple);
            }
        }
        else if (couple.andOrCompare == EventCouple.AndOrCompare.ExactlyOne) {
            if (testsPassed == 1) {
                InvokeAction(couple);
            }
        }
    }

    public static void InvokeAction(EventCouple couple) {
        foreach (EventAction action in couple.actions) {
            if (action.executeType == EventAction.ExecuteType.ExecuteFunction) {
                if (action.actionName != string.Empty) {
                    action.actionScript.GetType().GetMethod(action.actionName).Invoke(action.actionScript, action.args);
                }
            }
            else if (action.executeType == EventAction.ExecuteType.DebugMessage) {
                MonoBehaviour.print(action.p_string);
            }
            else if (action.executeType == EventAction.ExecuteType.ActivateNextEvent) {
                couple.popEvent.ActivateNextEvent();
            }
        }
        if (couple.popEvent.executeOnce == true) {
            couple.popEvent.Deactivate();
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