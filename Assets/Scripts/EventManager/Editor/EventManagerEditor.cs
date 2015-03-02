using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(EventManager), true)]
public class EventManagerEditor : Editor {
	#pragma warning disable 0219
    string[] conditionNames = new string[] { };
    string[] actionNames = new string[] { };

    private GUIStyle style;
    private int columnWidth = 200;

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

            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            foreach (var c in couple.conditions) {
                
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("<b><color=#106050ff>" + c.conditionScript + "</color></b>", style, GUILayout.MaxWidth(columnWidth));
                EditorGUILayout.LabelField("Condition Script", GUILayout.MaxWidth(columnWidth));
                c.conditionScript = (MonoBehaviour)EditorGUILayout.ObjectField(c.conditionScript, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
                EditorGUI.EndChangeCheck();

                if (c.conditionScript != null) {
                    EventLibrary.library.TryGetValue((c.conditionScript.GetType().Name + "Fields"), out conditionNames);
                    if (conditionNames != null) {
                        EditorGUILayout.LabelField("Condition", GUILayout.MaxWidth(columnWidth));
                        c.conditionIndex = EditorGUILayout.Popup(c.conditionIndex, conditionNames, EditorStyles.popup, GUILayout.MaxWidth(columnWidth));

                        if (conditionNames.Length <= c.conditionIndex) {
                            c.conditionIndex = 0;
                        }
                    
                        if (conditionNames.Length > c.conditionIndex) {
                            c.conditionField = conditionNames[c.conditionIndex];
                            c.conditionType = c.conditionScript.GetType().GetField(c.conditionField).FieldType;
                        }

                        EditorGUILayout.LabelField("Target Value", GUILayout.MaxWidth(columnWidth));
                        GUILayout.BeginHorizontal();
                        c.comparisonIndex = EditorGUILayout.Popup(c.comparisonIndex, c.comparisonOperators, EditorStyles.popup, GUILayout.MaxWidth(columnWidth / 4));
                        c.intCompareOption = (EventCondition.IntCompareOption)EditorGUILayout.EnumPopup(c.intCompareOption, GUILayout.MaxWidth(columnWidth / 4));
                        
                        if (c.conditionType == typeof(System.Int32)) {
                            c.conditionInt = EditorGUILayout.IntField(c.conditionInt, GUILayout.MaxWidth(columnWidth / 4 * 3));
                        }
                        else if (c.conditionType == typeof(System.Single)) {
                            c.conditionFloat = EditorGUILayout.FloatField(c.conditionFloat, GUILayout.MaxWidth(columnWidth / 4 * 3));

                        }
                        GUILayout.EndHorizontal();

                    }
                    else {
                        EditorGUILayout.LabelField(" ", "<b><color=#ff2222ff>No Valid Fields</color></b>", style);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Separator();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();


            foreach (var a in couple.actions) {

                //  Action Components
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.LabelField("<b><color=#3030a0ff>" + a.actionScript + "</color></b>", style, GUILayout.MaxWidth(columnWidth));
                EditorGUILayout.LabelField("Action Script", GUILayout.MaxWidth(columnWidth));
                a.actionScript = (MonoBehaviour)EditorGUILayout.ObjectField(a.actionScript, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
                EditorGUI.EndChangeCheck();

                if (a.actionScript != null) {
                    EventLibrary.library.TryGetValue((a.actionScript.GetType().Name + "Methods"), out actionNames);
                    if (actionNames != null) {
                        EditorGUILayout.LabelField("Action Function", GUILayout.MaxWidth(columnWidth));
                        a.actionEditorIndex = EditorGUILayout.Popup(a.actionEditorIndex, actionNames, EditorStyles.popup, GUILayout.MaxWidth(columnWidth));


                        if (actionNames.Length <= a.actionEditorIndex) {
                            a.actionEditorIndex = 0;
                        }

                        //  Determine type to pass
                        System.Type paramType = typeof(void);

                        if (actionNames.Length > a.actionEditorIndex) {
                            a.actionName = actionNames[a.actionEditorIndex];
                            var par = a.actionScript.GetType().GetMethod(a.actionName).GetParameters();
                            if (par.Length > 0) {
                                paramType = par[0].ParameterType;
                            }
                        }
                        //  Label for non-null
                        if (paramType != typeof(void)) {
                            EditorGUILayout.LabelField("Value to Pass", GUILayout.MaxWidth(columnWidth));
                        }
                        //  Expose the proper variable
                        if (paramType == typeof(System.Int32)) {
                            a.p_int = EditorGUILayout.IntField(a.p_int, GUILayout.MaxWidth(columnWidth));
                        }
                        else if (paramType == typeof(System.Single)) {
                            a.p_float = EditorGUILayout.FloatField(a.p_float, GUILayout.MaxWidth(columnWidth));
                        }
                        else if (paramType == typeof(Vector3)) {
                            a.p_Vector3 = EditorGUILayout.Vector3Field("", a.p_Vector3, GUILayout.MaxWidth(columnWidth));
                        }
                        else if (paramType == typeof(GameObject)) {
                            a.p_GameObject = (GameObject)EditorGUILayout.ObjectField(a.p_GameObject, typeof(GameObject), true, GUILayout.MaxWidth(columnWidth));
                        }
                        else if (paramType == typeof(MonoBehaviour)) {
                            a.p_MonoBehaviour = (MonoBehaviour)EditorGUILayout.ObjectField(a.p_MonoBehaviour, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
                        }

                        //  Set the parameters
                        a.args = a.SetParameters(paramType);
                    }
                    else {
                        EditorGUILayout.LabelField(" ", "<b><color=#ff2222ff>No Valid Methods</color></b>", style, GUILayout.MaxWidth(columnWidth));
                    }

  
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Condition")) {
            AddCondition();
        } 
        if (GUILayout.Button("Add Action")) {
            AddAction();
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

    void AddCondition() {
        EventCouple couple = m.couples[0];
        int count = couple.actions.Count;
        couple.conditions.Add(new EventCondition());
    }
    
    void AddAction() {
        EventCouple couple = m.couples[0];
        int count = couple.actions.Count;
        couple.actions.Add(new EventAction());
    }

}