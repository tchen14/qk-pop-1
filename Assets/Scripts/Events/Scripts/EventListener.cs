using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventListener {

    public static List<PopEvent> eventList = new List<PopEvent>();
    public static void AddPopEvent(PopEvent popEvent) {
        for (int i = 0; i < eventList.Count; i++){
            if (eventList[i] == null) {
                eventList.RemoveAt(i);
                //MonoBehaviour.print(eventList.Count);
                AddPopEvent(popEvent);
                return;
            }
            if (eventList[i] == popEvent) {
                return;
            }
        }
        eventList.Add(popEvent);
        //MonoBehaviour.print(eventList.Count);
    }

    public static bool CheckForDuplicateId(PopEvent popEvent, string id) {
        if (id == "") {
            return false;
        }
        for (int i = 0; i < eventList.Count; i++) {
            if (eventList[i] != popEvent) {
                if (eventList[i].uniqueId == id) {
                    return true;
                }
            }
        }
        return false;
    }

    public static void Report(MonoBehaviour mono, string value) {
        //StackFrame stackFrame = new StackFrame(1);
        //MonoBehaviour.print(stackFrame.GetMethod().Name);
    }

    public static List<EventCouple> coupleScripts = new List<EventCouple>();

    public static void SlowUpdate(EventCouple couple) {
        if (couple.popEvent.executeOnce == true && couple.popEvent.hasExecuted == true) { return; }
        int numberOfConditions = 0;
        int testsPassed = 0;

        foreach(EventCondition condition in couple.conditions){
            numberOfConditions++;
            //  Watch Script Type Condition
            if (condition.watchType == "Watch Script") {
                if (condition.conditionScript != null) {
                    numberOfConditions--;
                    if (condition.conditionType == null) {
                        condition.conditionType = condition.conditionScript.GetType().GetField(condition.conditionName).FieldType;
                    }
                }

                if (condition.conditionType == typeof(System.Int32)) {
                    int intValue = (int)condition.conditionScript.GetType().GetField(condition.conditionName).GetValue(condition.conditionScript);
                    if (Compare(intValue, condition.p_int, condition.numberCompareOption)) {
                        testsPassed++;
                    }
                }
                else if (condition.conditionType == typeof(System.Single)) {
                    float floatValue = (float)condition.conditionScript.GetType().GetField(condition.conditionName).GetValue(condition.conditionScript);
                    if (Compare(floatValue, condition.p_float, condition.numberCompareOption)) {
                        testsPassed++;
                    }
                }
                else if (condition.conditionType == typeof(System.Boolean)) {
                    bool boolValue = (bool)condition.conditionScript.GetType().GetField(condition.conditionName).GetValue(condition.conditionScript);
                    bool conditionBool = (condition.p_int == 1) ? true : false;
                    if (boolValue == conditionBool) {
                        testsPassed++;
                    }
                }
            }
            else if (condition.watchType == "Player Enters Area") {
                if (condition.p_Transform == null) {
                    condition.p_Transform = GameObject.Find("/_Player").transform;
                }
                //  Add to couple.popEvent.conditionRegionRadius a distance equal to the width of the player (right now we can test with 1)
                if (Vector3.Distance(condition.p_Transform.position, couple.popEvent.transform.position) < couple.popEvent.conditionRegionRadius + 1) {
                    testsPassed++;
                }
            }
            else if (condition.watchType == "Player Leaves Area") {
                if (condition.p_Transform == null) {
                    condition.p_Transform = GameObject.Find("/_Player").transform;
                }
                if (Vector3.Distance(condition.p_Transform.position, couple.popEvent.transform.position) > couple.popEvent.conditionRegionRadius + 1) {
                    testsPassed++;
                }
            }
            else if (condition.watchType == "Wait X Seconds") {
                if (couple.popEvent.totalTimeActive >= condition.p_float) {
                    testsPassed++;
                }
            }
            else if (condition.watchType == "Choose A Condition"){
                numberOfConditions--;
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
        couple.popEvent.hasExecuted = true;
        bool destroyAfterwards = false;
        foreach (EventAction action in couple.actions) {
            if (action.executeStaticFunction == false) {
                if (action.executeType == "Execute Function") {
                    if (action.actionName != string.Empty && action.actionScript != null) {
                        action.actionScript.GetType().GetMethod(action.actionName).Invoke(action.actionScript, action.args);
                    }
                }
                else if (action.executeType == "Debug Message") {
                    MonoBehaviour.print(action.p_string);
                }
                else if (action.executeType == "Activate Next Event") {
                    couple.popEvent.ActivateNextEvent();
                }
                else if (action.executeType == "Activate Another Event") {
                    ActivateById(action.p_string, true);
                }
                else if (action.executeType == "Deactivate Another Event") {
                    ActivateById(action.p_string, false);
                }
                else if (action.executeType == "Create Prefab At Position") {
                    MonoBehaviour.Instantiate(action.p_GameObject, action.p_Vector3, Quaternion.identity);
                }
                else if (action.executeType == "Create Prefab Here") {
                    MonoBehaviour.Instantiate(action.p_GameObject, couple.popEvent.gameObject.transform.position, Quaternion.identity);
                }
                else if (action.executeType == "Destroy This Object") {
                    destroyAfterwards = true;
                }
            }
            else {
                EventLibrary.staticClasses[action.executeCategory].GetMethod(action.executeType).Invoke(null, action.args);
            }
        }
        if (couple.popEvent.executeOnce == true) {
            couple.popEvent.MakeActive(false);
        }
        if (destroyAfterwards == true) {
            MonoBehaviour.Destroy(couple.popEvent.gameObject);
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

    private static void ActivateById(string id, bool active) {
        for (int i = 0; i < eventList.Count; i++) {
            if (eventList[i].uniqueId == id) {
                eventList[i].MakeActive(active);
            }
        }
    }

}