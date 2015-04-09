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

    PopEvent popTarget;

    bool duplicateId = false;

    void OnEnable() {
        Reload();
    }

    void Reload() {
        popTarget = (PopEvent)target;
        style = new GUIStyle();
        style.richText = true;
        #if UNITY_PRO_LICENSE
            style.normal.textColor = Color.white;
        #else
            style.normal.textColor = Color.black;
        #endif
        style.fontStyle = FontStyle.Bold;
        style.clipping = TextClipping.Clip;

        if (popTarget.couple == null) {
            popTarget.couple = new EventCouple(popTarget);
        }
        EventListener.AddPopEvent(popTarget);
        duplicateId = EventListener.CheckForDuplicateId(popTarget, popTarget.uniqueId);
    }

    #region General Inspector GUI
    override public void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();

        columnWidth = Mathf.FloorToInt(Screen.width / 2.4f);

        /*!     Enabled Boolean & Update Timer     */
        EditorGUILayout.Space();
        int halfWidth = columnWidth / 2;
        int thirdWidth = columnWidth / 3;
        int quarterWidth = columnWidth / 4;
        int sixthWidth = columnWidth / 6;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Active", GUILayout.MaxWidth(halfWidth));
        popTarget.isActive = EditorGUILayout.Toggle(popTarget.isActive, GUILayout.MaxWidth(sixthWidth));
        GUILayout.Label("", GUILayout.MaxWidth(columnWidth / 10));

        EditorGUILayout.LabelField("Unique Id (optional)", GUILayout.MaxWidth(halfWidth + quarterWidth));
        popTarget.uniqueId = EditorGUILayout.TextField(popTarget.uniqueId, GUILayout.MaxWidth(halfWidth + sixthWidth));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Execute Once", GUILayout.MaxWidth(halfWidth));
        popTarget.executeOnce = EditorGUILayout.Toggle(popTarget.executeOnce, GUILayout.MaxWidth(sixthWidth));
        GUILayout.Label("", GUILayout.MaxWidth(columnWidth / 10));

        EditorGUILayout.LabelField("Check Every (seconds)", GUILayout.MaxWidth(halfWidth + quarterWidth));
        popTarget.delay = EditorGUILayout.FloatField(popTarget.delay, GUILayout.MaxWidth(halfWidth + sixthWidth));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Regional", GUILayout.MaxWidth(halfWidth));
        popTarget.isRegional = EditorGUILayout.Toggle(popTarget.isRegional, GUILayout.MaxWidth(sixthWidth));
        GUILayout.Label("", GUILayout.MaxWidth(columnWidth / 10));

        EditorGUILayout.LabelField("Conditions to Meet", GUILayout.MaxWidth(halfWidth + quarterWidth));
        popTarget.couple.andOrCompare = (EventCouple.AndOrCompare)EditorGUILayout.EnumPopup(popTarget.couple.andOrCompare, GUILayout.MaxWidth(halfWidth + sixthWidth));
        EditorGUILayout.EndHorizontal();

        /*!     Conditions and Actions    */
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        /*!     List of every Condition in this manager    */
        DrawConditions();

        /*!     List of every Action in this manager    */
        DrawActions();

        EditorGUILayout.Space();

        EditorGUILayout.EndHorizontal();


        /*!     Condition and Action Buttons    */
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (duplicateId == true) {
            EditorGUILayout.HelpBox("Id \"" + popTarget.uniqueId + "\" is not Unique. If this event is referenced by id, all events with this id will be targeted.", MessageType.Warning, true);
        }

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
        EditorGUILayout.Space();
        if (GUILayout.Button("Add Another Event", GUILayout.MaxWidth(columnWidth * 2.14f))) {
            AddPopEvent.AddComponent();
        }
        if (EditorGUI.EndChangeCheck()) {
            Reload();
        }
    }

    void OnSceneGUI() {

        if (popTarget.drawRegionTwo == true) {
            popTarget.conditionRegionRadius = Handles.RadiusHandle(Quaternion.identity, popTarget.transform.position, popTarget.conditionRegionRadius);
        }
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }

    }

    #endregion General Inspector GUI

    #region Condition GUI
    //  Draw Condition    -------------------------------------------------------------------------------------------------
    void DrawConditions() {
        EditorGUILayout.BeginVertical();
        int count = 0;
        popTarget.drawRegionTwo = false;
        foreach (var condition in popTarget.couple.conditions) {
            count++;
            if (DrawOneCondition(condition, count) == false) {
                break;
            }
        }
        EditorGUILayout.EndVertical();
    }

    bool DrawOneCondition(EventCondition condition, int count){
        DrawBackground(condition.watchType);

        GUILayout.BeginHorizontal();

        condition.watchIndex = FindIndex(condition.watchType, PopEventCore.watchTypes);
        condition.watchIndex = (int)EditorGUILayout.Popup(condition.watchIndex, PopEventCore.watchTypes, GUILayout.MaxWidth(columnWidth));
        condition.watchType = PopEventCore.watchTypes[condition.watchIndex];

        GUI.backgroundColor = Color.red;
        if (popTarget.couple.conditions.Count > 1 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
            RemoveCondition(count - 1);
            return false;
        }
        else if (popTarget.couple.conditions.Count == 1) {
            GUILayout.Label(" ", GUILayout.MaxWidth(20));
        }

        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();

        if (condition.watchType == "Watch Script") {
            DrawWatchScript(condition);
        }
        else if (condition.watchType == "Player Enters Area") {
            DrawPlayerEntersArea(condition);
        }
        else if (condition.watchType == "Player Leaves Area") {
            DrawPlayerLeavesArea(condition);
        }
        else if (condition.watchType == "Wait X Seconds") {
            DrawWaitXSeconds(condition);
        }
        EditorGUILayout.Space();
        return true;
    }

    void DrawWatchScript(EventCondition condition) {
        EditorGUILayout.LabelField("Condition Script", GUILayout.MaxWidth(columnWidth));
        condition.conditionScript = (MonoBehaviour)EditorGUILayout.ObjectField(condition.conditionScript, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));

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
                    condition.p_int = EditorGUILayout.IntField(condition.p_int, GUILayout.MaxWidth(columnWidth / 2));
                }
                else if (condition.conditionType == typeof(System.Single) || condition.conditionType == typeof(Vector3)) {
                    condition.p_float = EditorGUILayout.FloatField(condition.p_float, GUILayout.MaxWidth(columnWidth / 2));

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
    }

    void DrawPlayerEntersArea(EventCondition condition) {
        popTarget.drawRegionTwo = true;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Radius", GUILayout.MaxWidth(columnWidth / 3));
        popTarget.conditionRegionRadius = EditorGUILayout.FloatField(popTarget.conditionRegionRadius, GUILayout.MaxWidth(columnWidth / 3 * 2));
        GUILayout.EndHorizontal();
    }

    void DrawPlayerLeavesArea(EventCondition condition) {
        popTarget.drawRegionTwo = true;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Radius", GUILayout.MaxWidth(columnWidth / 3));
        popTarget.conditionRegionRadius = EditorGUILayout.FloatField(popTarget.conditionRegionRadius, GUILayout.MaxWidth(columnWidth / 3 * 2));
        GUILayout.EndHorizontal();
    }

    void DrawWaitXSeconds(EventCondition condition) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Seconds to Wait", GUILayout.MaxWidth(columnWidth / 2));
        condition.p_float = EditorGUILayout.FloatField(condition.p_float, GUILayout.MaxWidth(columnWidth / 2));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Seconds Waited", GUILayout.MaxWidth(columnWidth / 2));
        EditorGUILayout.LabelField(popTarget.totalTimeActive.ToString(), GUILayout.MaxWidth(columnWidth / 2));
        EditorGUILayout.EndHorizontal();

    }

    #endregion Condition GUI

    #region Action GUI
    //  Draw Actions    -------------------------------------------------------------------------------------------------
    void DrawActions() {
        EditorGUILayout.BeginVertical();
        int count = 0;
        foreach (var action in popTarget.couple.actions) {
            count++;
            if (DrawOneAction(action, count) == false) {
                break;
            }
        }
        EditorGUILayout.EndVertical();
    }

    bool DrawOneAction(EventAction action, int count) {
        DrawBackground(action.executeType);

        //  Action Components

        EditorGUILayout.BeginHorizontal();
        action.executeIndex = FindIndex(action.executeType, PopEventCore.executeTypes);
        action.executeIndex = (int)EditorGUILayout.Popup(action.executeIndex, PopEventCore.executeTypes, GUILayout.MaxWidth(columnWidth));
        action.executeType = PopEventCore.executeTypes[action.executeIndex];

        GUI.backgroundColor = Color.red;
        if (popTarget.couple.actions.Count > 1 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
            RemoveAction(count - 1);
            return false;
        }
        else if (popTarget.couple.actions.Count == 1) {
            GUILayout.Label(" ", GUILayout.MaxWidth(20));
        }

        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();

        if (action.executeType == "Execute Function") {
            DrawExecuteFunction(action);
        }
        else if (action.executeType == "Debug Message") {
            DrawDebugMessage(action);
        }
        else if (action.executeType == "Activate Another Event" || action.executeType == "Deactivate Another Event") {
            DrawActivateAnotherEvent(action);
        }
        EditorGUILayout.Space();
        return true;
    }

    void DrawExecuteFunction(EventAction action){

        EditorGUILayout.LabelField("Action Script", GUILayout.MaxWidth(columnWidth));
        action.actionScript = (MonoBehaviour)EditorGUILayout.ObjectField(action.actionScript, typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));

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
    }

    void DrawDebugMessage(EventAction action) {
        action.p_string = EditorGUILayout.TextField(action.p_string, GUILayout.MaxWidth(columnWidth));
    }

    void DrawActivateAnotherEvent(EventAction action) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Event Id", GUILayout.MaxWidth(columnWidth / 2));
        action.p_string = EditorGUILayout.TextField(action.p_string, GUILayout.MaxWidth(columnWidth / 2));
        EditorGUILayout.EndHorizontal();
    }

    #endregion Action GUI

    #region Background

    //  Background  -------------------------------------------------------------------------------------
    void DrawBackground(string type) {
        Color blue = new Color(0, 0.58f, 0.69f, 0.45f);
        Color red = new Color(1, 0.46f, 0, 0.45f);

        if (type == "Player Enters Area") {
            DrawBackground(42, blue);
        }
        if (type == "Player Leaves Area") {
            DrawBackground(42, blue);
        }
        else if (type == "Watch Script") {
            DrawBackground(132, blue);
        }
        else if (type == "Wait X Seconds") {
            DrawBackground(60, blue);
        }
        else if (type == "Choose A Condition") {
            DrawBackground(24, blue - new Color(0, 0, 0, 0.2f));
        }
        else if (type == "Execute Function") {
            DrawBackground(132, red);
        }
        else if (type == "Activate Next Event") {
            DrawBackground(24, red);
        }
        else if (type == "Activate Another Event" || type == "Deactivate Another Event") {
            DrawBackground(42, red);
        }
        else if (type == "Debug Message") {
            DrawBackground(42, red);
        }
        else if (type == "Choose An Action") {
            DrawBackground(24, red - new Color(0, 0, 0, 0.2f));
        }
    }
    
    void DrawBackground(float height, Color color){
        Rect rt = GUILayoutUtility.GetRect(0,0);
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(new Rect(rt.x, rt.y, Screen.width / 2.15f, height), GUIContent.none);
    }
    #endregion Background

    /*!     Buttons         */
    void AddCondition() {
        Undo.RecordObject(popTarget, "Add Condition");
        popTarget.couple.conditions.Add(new EventCondition());
    }
    
    void AddAction() {
        Undo.RecordObject(popTarget, "Add Action");
        popTarget.couple.actions.Add(new EventAction());
    }

    void RemoveCondition(int index) {
        Undo.RecordObject(popTarget, "Delete Condition");
        popTarget.couple.conditions.RemoveAt(index);
    }

    void RemoveAction(int index) {
        Undo.RecordObject(popTarget, "Delete Action");
        popTarget.couple.actions.RemoveAt(index);
    }

    int FindIndex(string name, string[] names) {
        for (int i=0; i < names.Length; i++) {
            if (name == names[i]) {
                return i;
            }
        }
        return 0;
    }
}