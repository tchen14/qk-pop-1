using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

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

    public static void SlowUpdate(PopEvent popEvent) {
        if (popEvent.executeOnce == true && popEvent.hasExecuted == true) { return; }
        int numberOfConditions = 0;
        int testsPassed = 0;

        foreach (EventHalf condition in popEvent.conditions) {
            numberOfConditions++;
            //  Watch Script Type Condition
            if (condition.e_categoryString == "Object Script") {
                if (condition.e_MonoBehaviour == null) {
                    numberOfConditions--;
                    if (condition.e_fieldType == null) {
                        condition.e_fieldType = condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).FieldType;
                    }
                }

                if (condition.e_fieldType == typeof(System.Int32)) {
                    int intValue = (int)condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    if (Compare(intValue, condition.p_int[0], condition.compareString)) {
                        testsPassed++;
                    }
                }
                else if (condition.e_fieldType == typeof(System.String)) {
                    string stringValue = (string)condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    if (Compare(stringValue, condition.p_string[0], condition.compareString)) {
                        testsPassed++;
                    }
                }
                else if (condition.e_fieldType == typeof(System.Single)) {
                    float floatValue = (float)condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    if (Compare(floatValue, condition.p_float[0], condition.compareString)) {
                        testsPassed++;
                    }
                }
                else if (condition.e_fieldType == typeof(System.Boolean)) {
                    bool boolValue = (bool)condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    bool conditionBool = true;
                    if (condition.compareString == "Is False") {
                        conditionBool = false;
                    }
                    if (boolValue == conditionBool) {
                        testsPassed++;
                    }
                }
                else if (condition.e_fieldType == typeof(Dictionary<string, int>)) {
                    Dictionary<string, int> dictionaryValue = (Dictionary<string, int>)condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    if (dictionaryValue.ContainsKey(condition.p_string[0])) {
                        if (Compare(dictionaryValue[condition.p_string[0]], condition.p_int[0], condition.compareString)) {
                            testsPassed++;
                        }
                    }
                }
            }
            else if (condition.e_categoryString == "Static Script") {
                if (condition.e_fieldType == null) {
                    condition.e_fieldType = EventLibrary.staticClasses[condition.e_classString].GetField(condition.e_fieldString).FieldType;
                }

                if (condition.e_fieldType == typeof(System.Int32)) {
                    int intValue = (int)EventLibrary.staticClasses[condition.e_classString].GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    if (Compare(intValue, condition.p_int[0], condition.compareString)) {
                        testsPassed++;
                    }
                }
                else if (condition.e_fieldType == typeof(System.Single)) {
                    float floatValue = (float)EventLibrary.staticClasses[condition.e_classString].GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    if (Compare(floatValue, condition.p_float[0], condition.compareString)) {
                        testsPassed++;
                    }
                }
                else if (condition.e_fieldType == typeof(System.Boolean)) {
                    bool boolValue = (bool)EventLibrary.staticClasses[condition.e_classString].GetField(condition.e_fieldString).GetValue(condition.e_MonoBehaviour);
                    bool conditionBool = true;
                    if (condition.compareString == "Is False"){
                        conditionBool = false;
                    }
                    if (boolValue == conditionBool) {
                        testsPassed++;
                    }
                }
            }
            else if (condition.e_classString == "Player Enters Area") {
                if (condition.p_Transform[0] == null) {
                    condition.p_Transform[0] = GameObject.Find("/_Player").transform;
                }
                //  Add to couple.popEvent.conditionRegionRadius a distance equal to the width of the player (right now we can test with 1)
                if (Vector3.Distance(condition.p_Transform[0].position, popEvent.transform.position) < popEvent.conditionRegionRadius + 1) {
                    testsPassed++;
                }
            }
            else if (condition.e_classString == "Player Leaves Area") {
                if (condition.p_Transform[0] == null) {
                    condition.p_Transform[0] = GameObject.Find("/_Player").transform;
                }
                if (Vector3.Distance(condition.p_Transform[0].position, popEvent.transform.position) > popEvent.conditionRegionRadius + 1) {
                    testsPassed++;
                }
            }
            else if (condition.e_classString == "Wait X Seconds") {
                if (popEvent.totalTimeActive >= condition.p_float[0]) {
                    testsPassed++;
                }
            }
            else if (condition.e_classString == "Choose A Condition") {
                numberOfConditions--;
            }
        }
        if (popEvent.andOrCompareString == "Every Condition") {
            if (testsPassed >= numberOfConditions) {
                InvokeAction(popEvent);
            }
        }
        else if (popEvent.andOrCompareString == "One or More") {
            if (testsPassed >= 1) {
                InvokeAction(popEvent);
            }
        }
        else if (popEvent.andOrCompareString == "Exactly One") {
            if (testsPassed == 1) {
                InvokeAction(popEvent);
            }
        }
    }

    public static void InvokeAction(PopEvent popEvent) {
        popEvent.hasExecuted = true;
        bool destroyAfterwards = false;
        foreach (EventHalf action in popEvent.actions) {
            if (action.args == null) {
                action.SetParameters();
            }
            if (action.args.Length > 5) {
                Debug.Warning("core", "Event functions cannot have more than 5 parameters");
                continue;
            }
            if (action.e_categoryString == "Static Script") {
                EventLibrary.staticClasses[action.e_classString].GetMethod(action.e_fieldString).Invoke(null, action.args);
            }
            else if (action.e_categoryString == "Object Script") {
                if (action.e_fieldString != string.Empty && action.e_MonoBehaviour != null) {
                    action.e_MonoBehaviour.GetType().GetMethod(action.e_fieldString).Invoke(action.e_MonoBehaviour, action.args);
                }
			}
			else if (action.e_categoryString == "GUI") {
				if (action.e_classString == "Set Objective") {
					GameHUD.UpdateObjectiveText(action.p_string[0]);
				}else if (action.e_classString == "Set Dialogue") {
					GameHUD.SetDialogueBoxText(action.p_string[0], action.p_string[1]);
				}
			} else if (action.e_categoryString == "System") {
                if (action.e_classString == "Debug Message") {
					Debug.Log("event", action.p_string[0]);
                    //MonoBehaviour.print(action.p_string[0]);
                }
                else if (action.e_classString == "Activate Next Event") {
                    popEvent.ActivateNextEvent();
                }
                else if (action.e_classString == "Activate Another Event") {
                    ActivateById(action.p_string[0], true);
                }
                else if (action.e_classString == "Deactivate Another Event") {
                    ActivateById(action.p_string[0], false);
                }
                else if (action.e_classString == "Create Prefab At Position") {
                    if (action.p_GameObject[0] != null) {
                        MonoBehaviour.Instantiate(action.p_GameObject[0], action.p_Vector3[0], Quaternion.Euler(action.p_Vector3[1]));
                    }
                }
                else if (action.e_classString == "Create Prefab Here") {
                    if (action.p_GameObject[0] != null) {
                        MonoBehaviour.Instantiate(action.p_GameObject[0], popEvent.gameObject.transform.position, Quaternion.identity);
                    }
                }
                else if (action.e_classString == "Move This Object") {
                    popEvent.gameObject.transform.position = action.p_Vector3[0];
                    popEvent.gameObject.transform.eulerAngles = action.p_Vector3[1];
				}
				else if (action.e_classString == "Destroy This Object") {
					destroyAfterwards = true;
				}
            }
        }
        if (popEvent.executeOnce == true) {
            popEvent.MakeActive(false);
        }
        if (destroyAfterwards == true) {
            MonoBehaviour.Destroy(popEvent.gameObject);
        }
    }


    /*!     Comparison functions        */
    public static bool Compare(int valueA, int valueB, string compareOption) {
        if (compareOption == "Equal To") {
            if (valueA == valueB) { return true; }
        }
        else if (compareOption == "Greater Than") {
            if (valueA > valueB) { return true; }
        }
        else if (compareOption == "Less Than") {
            if (valueA < valueB) { return true; }
        }
        return false;
    }

    public static bool Compare(float valueA, float valueB, string compareOption) {
        if (compareOption == "Equal To") {
            if (valueA == valueB) { return true; }
        }
        else if (compareOption == "Greater Than") {
            if (valueA > valueB) { return true; }
        }
        else if (compareOption == "Less Than") {
            if (valueA < valueB) { return true; }
        }
        return false;
    }

    public static bool Compare(string valueA, string valueB, string compareOption) {
        if (compareOption == "Equal To") {
            if (valueA == valueB) { return true; }
        }
        else if (compareOption == "Different Than") {
            if (valueA != valueB) { return true; }
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