using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PopEvent), true)]
public class PopEventEditor : Editor {

    string[] conditionNames = new string[] { };
    string[] actionNames = new string[] { };

    private GUIStyle style;
    private int columnWidth = 200;

    PopEvent m;

    void OnEnable() {
        Reload();
    }

    void Reload() {
        m = (PopEvent)target;
        style = new GUIStyle();
        style.richText = true;
        #if UNITY_PRO_LICENSE
            style.normal.textColor = Color.white;
        #else
            style.normal.textColor = Color.black;
        #endif
        style.fontStyle = FontStyle.Bold;
        style.clipping = TextClipping.Clip;

        if (m.couple == null) {
            m.couple = new EventCouple();
        }
    }

    override public void OnInspectorGUI() {
        columnWidth = Mathf.FloorToInt(Screen.width / 2.4f);

        /*!     Enabled Boolean & Update Timer     */
        EditorGUILayout.Space();

        m.checkConditions = EditorGUILayout.Toggle("Check for Conditions", m.checkConditions);
        
        EditorGUILayout.BeginHorizontal();
        m.delay = EditorGUILayout.FloatField("Check Every", m.delay);
        EditorGUILayout.LabelField("Seconds");
        EditorGUILayout.EndHorizontal();

        m.regional = EditorGUILayout.Toggle("Regional", m.regional);
        m.runOnce = EditorGUILayout.Toggle("Only Run Once", m.runOnce);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Conditions to Meet", GUILayout.MaxWidth(columnWidth - 30));
        m.andOrCompare = (PopEvent.AndOrCompare)EditorGUILayout.EnumPopup(m.andOrCompare, GUILayout.MaxWidth(columnWidth));
        EditorGUILayout.EndHorizontal();

        /*!     Conditions and Actions    */
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        /*!     List of every Condition in this manager    */
        EditorGUILayout.BeginVertical();

        int count = 0;
        foreach (var condition in m.couple.conditions) {
            count++;
                
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(condition.conditionScript != null ? condition.conditionScript.ToString() : "", style, GUILayout.MaxWidth(columnWidth - 20));
            GUI.backgroundColor = Color.red;
            if (m.couple.conditions.Count > 1 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
                RemoveCondition(count - 1);
                break;
            }
            GUI.backgroundColor = Color.white;

            GUILayout.EndHorizontal();

            condition.watchType = (EventCondition.WatchType)EditorGUILayout.EnumPopup(condition.watchType, GUILayout.MaxWidth(columnWidth));
            EditorGUILayout.LabelField("Condition Script", GUILayout.MaxWidth(columnWidth));
            condition.conditionScript = (MonoBehaviour)EditorGUILayout.ObjectField(condition.conditionScript, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
            EditorGUI.EndChangeCheck();

            if (condition.conditionScript != null) {
                EventLibrary.library.TryGetValue((condition.conditionScript.GetType().Name + "Fields"), out conditionNames);
                if (conditionNames != null) {
                    EditorGUILayout.LabelField("Condition", GUILayout.MaxWidth(columnWidth));
                    condition.conditionIndex = EditorGUILayout.Popup(condition.conditionIndex, conditionNames, EditorStyles.popup, GUILayout.MaxWidth(columnWidth));

                    if (conditionNames.Length <= condition.conditionIndex) {
                        condition.conditionIndex = 0;
                    }

                    if (conditionNames.Length > condition.conditionIndex) {
                        condition.conditionField = conditionNames[condition.conditionIndex];
                        condition.conditionType = condition.conditionScript.GetType().GetField(condition.conditionField).FieldType;
                    }

                    EditorGUILayout.LabelField("Target Value", GUILayout.MaxWidth(columnWidth));
                    GUILayout.BeginHorizontal();
                    /*!     ComparisonOption        */
                    if (condition.conditionType == typeof(System.Int32) || condition.conditionType == typeof(System.Single)) {
                        condition.numberCompareOption = (EventCondition.NumberCompareOption)EditorGUILayout.EnumPopup(condition.numberCompareOption, GUILayout.MaxWidth(columnWidth / 2));
                    }
                    else if (condition.conditionType == typeof(Vector3)) {
                        condition.vectorCompareOption = (EventCondition.VectorCompareOption)EditorGUILayout.EnumPopup(condition.vectorCompareOption, GUILayout.MaxWidth(columnWidth / 2));
                    }

                    /*!     Value Field             */
                    if (condition.conditionType == typeof(System.Int32)) {
                        condition.conditionInt = EditorGUILayout.IntField(condition.conditionInt, GUILayout.MaxWidth(columnWidth / 2));
                    }
                    else if (condition.conditionType == typeof(System.Single) || condition.conditionType == typeof(Vector3)) {
                        condition.conditionFloat = EditorGUILayout.FloatField(condition.conditionFloat, GUILayout.MaxWidth(columnWidth / 2));

                    }
                    else {
                        for (int sp = 0; sp < 6; sp++) { EditorGUILayout.Space(); }
                    }
                    GUILayout.EndHorizontal();

                }
                else {
                    EditorGUILayout.LabelField("<b><color=#ff2222ff>No Valid Fields</color></b>", style, GUILayout.MaxWidth(columnWidth));
                }
            }
            else {
                for (int sp = 0; sp < 12; sp++) { EditorGUILayout.Space(); }
            }
            if (count < m.couple.conditions.Count) {
                GUILayout.Label("____________________________", GUILayout.MaxWidth(columnWidth));
            }
        }
        EditorGUILayout.EndVertical();

        /*!     List of every Action in this manager    */
        EditorGUILayout.BeginVertical();

        count = 0;
        foreach (var action in m.couple.actions) {
            count++;

            //  Action Components
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(action.actionScript != null ? action.actionScript.ToString() : "", style, GUILayout.MaxWidth(columnWidth - 20));
            GUI.backgroundColor = Color.red;
            if (m.couple.actions.Count > 1 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
                RemoveAction(count - 1);
                break;
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Action Script", GUILayout.MaxWidth(columnWidth));
            action.actionScript = (MonoBehaviour)EditorGUILayout.ObjectField(action.actionScript, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
            EditorGUI.EndChangeCheck();

            if (action.actionScript != null) {
                EventLibrary.library.TryGetValue((action.actionScript.GetType().Name + "Methods"), out actionNames);
                if (actionNames != null) {
                    EditorGUILayout.LabelField("Action Function", GUILayout.MaxWidth(columnWidth));
                    action.actionEditorIndex = EditorGUILayout.Popup(action.actionEditorIndex, actionNames, EditorStyles.popup, GUILayout.MaxWidth(columnWidth));


                    if (actionNames.Length <= action.actionEditorIndex) {
                        action.actionEditorIndex = 0;
                    }

                    //  Determine type to pass
                    System.Type paramType = typeof(void);

                    if (actionNames.Length > action.actionEditorIndex) {
                        action.actionName = actionNames[action.actionEditorIndex];
                        var par = action.actionScript.GetType().GetMethod(action.actionName).GetParameters();
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
                        action.p_int = EditorGUILayout.IntField(action.p_int, GUILayout.MaxWidth(columnWidth));
                    }
                    else if (paramType == typeof(System.Single)) {
                        action.p_float = EditorGUILayout.FloatField(action.p_float, GUILayout.MaxWidth(columnWidth));
                    }
                    else if (paramType == typeof(Vector3)) {
                        action.p_Vector3 = EditorGUILayout.Vector3Field("", action.p_Vector3, GUILayout.MaxWidth(columnWidth));
                    }
                    else if (paramType == typeof(GameObject)) {
                        action.p_GameObject = (GameObject)EditorGUILayout.ObjectField(action.p_GameObject, typeof(GameObject), true, GUILayout.MaxWidth(columnWidth));
                    }
                    else if (paramType == typeof(MonoBehaviour)) {
                        action.p_MonoBehaviour = (MonoBehaviour)EditorGUILayout.ObjectField(action.p_MonoBehaviour, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
                    }
                    else {
                        for (int sp = 0; sp < 6; sp++) { EditorGUILayout.Space(); }
                    }

                    //  Set the parameters
                    action.args = action.SetParameters(paramType);
                }
                else {
                    EditorGUILayout.LabelField("<b><color=#ff2222ff>No Valid Methods</color></b>", style, GUILayout.MaxWidth(columnWidth));
                }
            }
            else {
                for (int sp = 0; sp < 12; sp++) { EditorGUILayout.Space(); }
            }
            if (count < m.couple.actions.Count) {
                GUILayout.Label("____________________________", GUILayout.MaxWidth(columnWidth));
            } 
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        EditorGUILayout.EndHorizontal();


        /*!     Condition and Action Buttons    */
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add Condition", GUILayout.MaxWidth(columnWidth))) {
            AddCondition();
        }
        GUILayout.Label("", GUILayout.MaxWidth(columnWidth / 10));
        if (GUILayout.Button("Add Action", GUILayout.MaxWidth(columnWidth))) {
            AddAction();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();

    }

    /*!     Buttons         */
    void AddCondition() {
        Undo.RecordObject(m, "Add Condition");
        m.couple.conditions.Add(new EventCondition());
    }
    
    void AddAction() {
        Undo.RecordObject(m, "Add Action");
        m.couple.actions.Add(new EventAction());
    }

    void RemoveCondition(int index) {
        Undo.RecordObject(m, "Delete Condition");
        m.couple.conditions.RemoveAt(index);
    }

    void RemoveAction(int index) {
        Undo.RecordObject(m, "Delete Action");
        m.couple.actions.RemoveAt(index);
    }
}