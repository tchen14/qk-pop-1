using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(EventManager), true)]
public class EventManagerEditor : Editor {

    string[] conditionNames = new string[] { };
    string[] actionNames = new string[] { };

    GUIStyle style;

    EventManager m;

    void OnEnable() {
        Reload();
    }

    void Reload() {
        m = (EventManager)target;
        style = new GUIStyle();
        style.richText = true;
        if (m.couples == null) {
            m.couples = new List<EventCouple>();
            m.couples.Add(new EventCouple());
        }
    }

    override public void OnInspectorGUI() {

        //  conditioning Components
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        m.delay = EditorGUILayout.FloatField("Check Every", m.delay);
        EditorGUILayout.LabelField("Seconds");
        EditorGUILayout.EndHorizontal();


        foreach (var couple in m.couples) {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            foreach (var c in couple.conditions) {
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("<b><color=#106050ff>" + c.conditionScript + "</color></b>", style);
                c.conditionScript = (MonoBehaviour)EditorGUILayout.ObjectField("Condition Script", c.conditionScript, typeof(MonoBehaviour), true);
                EditorGUI.EndChangeCheck();

                if (c.conditionScript != null) {
                    EventLibrary.library.TryGetValue((c.conditionScript.GetType().Name + "Fields"), out conditionNames);
                    if (conditionNames != null) {
                        c.conditionIndex = EditorGUILayout.Popup("Condition", c.conditionIndex, conditionNames, EditorStyles.popup);

                        if (conditionNames.Length <= c.conditionIndex) {
                            c.conditionIndex = 0;
                        }
                    
                        if (conditionNames.Length > c.conditionIndex) {
                            c.conditionField = conditionNames[c.conditionIndex];
                            c.conditionType = c.conditionScript.GetType().GetField(c.conditionField).FieldType;
                        }

                        if (c.conditionType == typeof(System.Int32)) {
                            c.conditionInt = EditorGUILayout.IntField("Target Value", c.conditionInt);
                        }
                        else if (c.conditionType == typeof(System.Single)) {
                            c.conditionFloat = EditorGUILayout.FloatField("Target Value", c.conditionFloat);
                        }
                    }
                    else {
                        EditorGUILayout.LabelField(" ", "<b><color=#ff2222ff>No Valid Fields</color></b>", style);
                    }
                }
            }

            EditorGUILayout.Space();

            foreach (var a in couple.actions) {

                //  Action Components
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("<b><color=#3030a0ff>" + a.actionScript + "</color></b>", style);
                a.actionScript = (MonoBehaviour)EditorGUILayout.ObjectField("Action Script", a.actionScript, typeof(MonoBehaviour), true);
                EditorGUI.EndChangeCheck();

                if (a.actionScript != null) {
                    EventLibrary.library.TryGetValue((a.actionScript.GetType().Name + "Methods"), out actionNames);
                    if (actionNames != null) {
                        a.actionIndex = EditorGUILayout.Popup("Action Function", a.actionIndex, actionNames, EditorStyles.popup);


                        if (actionNames.Length <= a.actionIndex) {
                            a.actionIndex = 0;
                        }

                        if (actionNames.Length > a.actionIndex) {
                            a.actionName = actionNames[a.actionIndex];
                            var par = a.actionScript.GetType().GetMethod(a.actionName).GetParameters();
                            if (par.Length > 0) {
                                a.actionType = par[0].ParameterType;
                            }
                            else {
                                a.actionType = typeof(void);
                            }
                        }
                        //  Determine type to pass
                        if (a.actionType == typeof(System.Int32)) {
                            a.actionInt = EditorGUILayout.IntField("Value to Pass", a.actionInt);
                        }
                        else if (a.actionType == typeof(Vector3)) {
                            a.actionVector3 = EditorGUILayout.Vector3Field("Value to Pass", a.actionVector3);
                        }
                    }
                    else {
                        EditorGUILayout.LabelField(" ", "<b><color=#ff2222ff>No Valid Methods</color></b>", style);
                    }

  
                }
            }
        }
        EditorGUILayout.Space();


        if (GUILayout.Button("Add Couple")) {
            AddCouple();
        }

        EditorGUILayout.EndVertical();

    }

    void AddCouple() {
        int count = m.couples.Count;
        m.couples.Add(new EventCouple());
        m.couples[count].conditions = new List<EventCondition>();
        m.couples[count].conditions.Add(new EventCondition());
        m.couples[count].actions = new List<EventAction>();
        m.couples[count].actions.Add(new EventAction());
    }

}