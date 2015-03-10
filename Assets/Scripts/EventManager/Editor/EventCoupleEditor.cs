/*
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EventCouple), true)]
public class EventCoupleEditor : Editor {

    string[] conditionNames = new string[] { };
    string[] actionNames = new string[] { };

    GUIStyle style;

    EventCouple e;

    void OnEnable() {
        e = (EventCouple)target;
        style = new GUIStyle();
        style.richText = true;
    }

    override public void OnInspectorGUI() {

        //  conditioning Components
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        e.delay = EditorGUILayout.FloatField("Check Every", e.delay);
        EditorGUILayout.LabelField("Seconds");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("<b><color=#106050ff>" + e.conditionScript + "</color></b>", style);
        e.conditionScript = (MonoBehaviour)EditorGUILayout.ObjectField("Condition Script", e.conditionScript, typeof(MonoBehaviour), true);
        EditorGUI.EndChangeCheck();

        e.conditionIndex = EditorGUILayout.Popup("Condition", e.conditionIndex, conditionNames, EditorStyles.popup);
        conditionNames = EventLibrary.library[e.conditionScript.GetType().Name + "Fields"];
        if (conditionNames.Length <= e.conditionIndex) {
            e.conditionIndex = 0;
        }
        if (conditionNames.Length > e.conditionIndex) {
            e.conditionField = conditionNames[e.conditionIndex];
            e.conditionType = e.conditionScript.GetType().GetField(e.conditionField).FieldType;
        }

        if (e.conditionType == typeof(System.Int32)) {
            e.conditionInt = EditorGUILayout.IntField("Target Value", e.conditionInt);
        }
        else if (e.conditionType == typeof(System.Single)) {
            e.conditionFloat = EditorGUILayout.FloatField("Target Value", e.conditionFloat);
        }

        EditorGUILayout.Space();

        //  Action Components
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("<b><color=#3030a0ff>" + e.actionScript + "</color></b>", style);
        e.actionScript = (MonoBehaviour)EditorGUILayout.ObjectField("Action Script", e.actionScript, typeof(MonoBehaviour), true);
        EditorGUI.EndChangeCheck();

        e.actionIndex = EditorGUILayout.Popup("Action Function", e.actionIndex, actionNames, EditorStyles.popup);
        actionNames = EventLibrary.library[e.actionScript.GetType().Name + "Methods"];
        if (actionNames.Length <= e.actionIndex) {
            e.actionIndex = 0;
        }

        if (actionNames.Length > e.actionIndex) {
            e.actionName = actionNames[e.actionIndex];
            var par = e.actionScript.GetType().GetMethod(e.actionName).GetParameters();
            if (par.Length > 0) {
                e.actionType = par[0].ParameterType;
            }
            else {
                e.actionType = typeof(void);
            }
        }

        //  Determine type to pass
        if (e.actionType == typeof(System.Int32)) {
            e.actionInt = EditorGUILayout.IntField("Value to Pass", e.conditionInt);
        }
        else if (e.actionType == typeof(Vector3)) {
            e.actionVector3 = EditorGUILayout.Vector3Field("Value to Pass", e.actionVector3);
        }

        EditorGUILayout.EndVertical();

    }
}
*/