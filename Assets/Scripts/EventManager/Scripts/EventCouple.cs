using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventCouple : MonoBehaviour {

    public MonoBehaviour triggerScript;
    public string triggerKeyword;
    public int triggerIndex;
    public int targetValue;

    public MonoBehaviour actionScript;
    public string actionName;
    public int actionIndex;
    public int intToPass;
    public Vector3 v3ToPass;

    void Start() {
        EventListener.AddCouple(this);
    }

    public void Report(MonoBehaviour triggeringScript, string keyword) {
        if (triggerScript == triggeringScript && keyword == triggerKeyword) {
            Invoke();
        }
    }

    public void Report(MonoBehaviour triggeringScript, string keyword, int value) {
        if (triggerScript == triggeringScript && keyword == triggerKeyword && value == targetValue) {
            Invoke();
        }
    }

    private void Invoke() {
        try {
            if (actionScript is IEventScript) {
                IEventScript iscripts = actionScript as IEventScript;
                EventTable actionEventTable = iscripts.eventTable();
                TestType testType = actionEventTable.actionEntries[actionIndex].testType;
                if (testType == TestType.Int) {
                    UnityEventBase.GetValidMethodInfo(actionScript, actionName, new System.Type[] { typeof(int) }).Invoke(actionScript, new object[] { intToPass });
                }
                else if (testType == TestType.Vector3) {
                    UnityEventBase.GetValidMethodInfo(actionScript, actionName, new System.Type[] { typeof(Vector3) }).Invoke(actionScript, new object[] { v3ToPass });
                }
                else {
                    UnityEventBase.GetValidMethodInfo(actionScript, actionName, new System.Type[] { }).Invoke(actionScript, null);
                }
            }

        }
        catch {
            if (actionScript != null) { print("Method " + actionName + " not found in " + actionScript.ToString()); }
            else { print("No target script"); }
        }
    }
}