using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EventCouple), true)]
public class EventCoupleEditor : Editor {

    string[] triggerNames = new string[] { };
    string[] actionNames = new string[] { };

    bool repaint;

    GUIStyle style;

    EventCouple e;
    EventTable triggerEventTable;
    EventTable actionEventTable;

    void OnEnable() {
        e = (EventCouple)target;
        style = new GUIStyle();
        style.richText = true;
        Reload();
    }

    void Reload() {
        triggerEventTable = null;
        if (e.triggerScript is IEventScript){
            IEventScript iscripts = e.triggerScript as IEventScript;
            triggerEventTable = iscripts.eventTable();
        }
        actionEventTable = null;
        if (e.actionScript is IEventScript) {
            IEventScript iscripts = e.actionScript as IEventScript;
            actionEventTable = iscripts.eventTable();
        }
    }

    override public void OnInspectorGUI() {

        //  Triggering Components
        EditorGUILayout.BeginVertical();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("<b><color=#106050ff>" + e.triggerScript + "</color></b>", style);
        e.triggerScript = (MonoBehaviour)EditorGUILayout.ObjectField("Trigger Script", e.triggerScript, typeof(MonoBehaviour), true);
        EditorGUI.EndChangeCheck();

        e.triggerIndex = EditorGUILayout.Popup("Keyword", e.triggerIndex, triggerNames, EditorStyles.popup);
        if (triggerEventTable != null){
            triggerNames = triggerEventTable.GetKeywords();
            if (triggerNames.Length <= e.triggerIndex) {
                e.triggerIndex = 0;
            }
        }

        if (triggerNames.Length > e.triggerIndex) {
            e.triggerKeyword = triggerNames[e.triggerIndex];
        }
        if (triggerEventTable != null && triggerEventTable.triggerEntries.Length > e.triggerIndex) {
            if (triggerEventTable.triggerEntries[e.triggerIndex].testType == TestType.Int) {
                e.targetValue = EditorGUILayout.IntField("Target Value", e.targetValue);
            }
        }

        EditorGUILayout.Space();

        //  Action Components
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("<b><color=#3030a0ff>" + e.actionScript + "</color></b>", style);
        e.actionScript = (MonoBehaviour)EditorGUILayout.ObjectField("Action Script", e.actionScript, typeof(MonoBehaviour), true);
        EditorGUI.EndChangeCheck();

        e.actionIndex = EditorGUILayout.Popup("Action Function:", e.actionIndex, actionNames, EditorStyles.popup);
        if (actionEventTable != null) {
            actionNames = actionEventTable.GetFunctions();
            if (actionNames.Length <= e.actionIndex) {
                e.actionIndex = 0;
            }
        }

        if (actionNames.Length > e.actionIndex) {
            e.actionName = actionNames[e.actionIndex];
        }
        if (actionEventTable != null && actionEventTable.actionEntries.Length > e.actionIndex) {
            if (actionEventTable.actionEntries[e.actionIndex].testType == TestType.Int) {
                e.intToPass = EditorGUILayout.IntField("Value to Pass", e.intToPass);
            }
            else if (actionEventTable.actionEntries[e.actionIndex].testType == TestType.Vector3) {
                e.v3ToPass = EditorGUILayout.Vector3Field("Value to Pass", e.v3ToPass);
            }
        }

        EditorGUILayout.EndVertical();

        if (GUI.changed) {
            Reload();
        }
    }
}