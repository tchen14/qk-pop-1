#pragma warning disable 168     //Variable assigned and not used: window

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[CustomEditor(typeof(PopEvent), true)]
public class PopEventEditor : Editor {

	private GUIStyle style;
	private int columnWidth = 200;

	PopEvent popTarget;

	//  Error and Warning flags
	bool duplicateId = false;
	bool chooseACondition = false;
	bool chooseAnAction = false;
	bool destroyThisObject = false;

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

		EventListener.AddPopEvent(popTarget);
		duplicateId = EventListener.CheckForDuplicateId(popTarget, popTarget.uniqueId);
		chooseACondition = false;
		chooseAnAction = false;
		destroyThisObject = false;
	}

	override public void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();

		columnWidth = Mathf.FloorToInt(Screen.width / 2.4f);

		if(popTarget.executeOnce == true && popTarget.hasExecuted == true) {
			DrawBackground("Execution Complete");
		}

		//     Enabled Boolean & Update Timer
		EditorGUILayout.Space();
		int halfWidth = columnWidth / 2;
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
		popTarget.andOrCompareIndex = EditorGUILayout.Popup(popTarget.andOrCompareIndex, popTarget.andOrCompare, GUILayout.MaxWidth(halfWidth + sixthWidth));
		popTarget.andOrCompareString = popTarget.andOrCompare[popTarget.andOrCompareIndex];
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Show on Map", GUILayout.MaxWidth(halfWidth));
		popTarget.addMapIcon = EditorGUILayout.Toggle(popTarget.addMapIcon, GUILayout.MaxWidth(sixthWidth));
		GUILayout.Label("", GUILayout.MaxWidth(columnWidth / 10));
		
		EditorGUILayout.LabelField("Icon Type", GUILayout.MaxWidth(halfWidth + quarterWidth));
		popTarget.iconTypeIndex = EditorGUILayout.Popup(popTarget.iconTypeIndex, popTarget.iconType, GUILayout.MaxWidth(halfWidth + sixthWidth));
		popTarget.iconTypeString = popTarget.iconType[popTarget.iconTypeIndex];
		EditorGUILayout.EndHorizontal ();

		//     Conditions and Actions
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();

		//     List of every Condition in this manager
		DrawConditions();

		//     List of every Action in this manager
		DrawActions();

		EditorGUILayout.Space();

		EditorGUILayout.EndHorizontal();


		//     Condition and Action Buttons
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if(duplicateId == true) {
			EditorGUILayout.HelpBox("Id \"" + popTarget.uniqueId + "\" is not Unique. If this event is referenced by Id, all events with this Id will be targeted.", MessageType.Warning, true);
		}
		if(chooseACondition == true) {
			EditorGUILayout.HelpBox("Any conditions marked \"Choose A Condition\" will be ignored during gameplay.", MessageType.Warning, true);
		}
		if(chooseAnAction == true) {
			EditorGUILayout.HelpBox("Any actions marked \"Choose An Action\" will be ignored during gameplay.", MessageType.Warning, true);
		}
		if(destroyThisObject == true) {
			EditorGUILayout.HelpBox("\"Destroy This Object\" will destroy this Game Object, all of its children, and everything attached to it.", MessageType.Warning, true);
		}

		EditorGUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add Condition", GUILayout.MaxWidth(columnWidth))) {
			AddCondition();
		}
		GUILayout.Label("", GUILayout.MaxWidth(columnWidth / 10));
		if(GUILayout.Button("Add Action", GUILayout.MaxWidth(columnWidth))) {
			AddAction();
		}
		GUI.backgroundColor = Color.white;

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Add Another Event", GUILayout.MaxWidth(columnWidth * 2.14f))) {
			AddPopEvent.AddComponent();
		}
		if(EditorGUI.EndChangeCheck()) {
			Reload();
		}
		//EditorGUILayout.Space();
		//EditorGUILayout.Space();
		//Rect rt = GUILayoutUtility.GetRect(0, 50);
		//EditorGUI.ObjectField(rt, go, typeof(GameObject));
		var window = EditorWindow.focusedWindow;
		//MonoBehaviour.print(window.position);
		//EditorGUILayout.Space();
		//EditorGUILayout.Space();

	}
	GameObject go;


	void OnSceneGUI() {

		if(popTarget.drawRegionTwo == true) {
			popTarget.conditionRegionRadius = Handles.RadiusHandle(Quaternion.identity, popTarget.transform.position, popTarget.conditionRegionRadius);
		}
		if(GUI.changed) {
			EditorUtility.SetDirty(target);
		}

	}

	//  Draw Condition    -------------------------------------------------------------------------------------------------
	void DrawConditions() {
		EditorGUILayout.BeginVertical();
		int count = 0;
		popTarget.drawRegionTwo = false;
		foreach(EventHalf condition in popTarget.conditions) {
			count++;
			if(DrawOneCondition(condition, count) == false) {
				break;
			}
		}
		EditorGUILayout.EndVertical();
	}

	bool DrawOneCondition(EventHalf condition, int count) {
		string[] popupArray;
		string[] popupArrayNice;
		DrawBackground(condition.editorHeight, condition.color);

		GUILayout.BeginHorizontal();
		popupArray = PopEventCore.watchLibrary.Keys.ToArray();
		popupArrayNice = PopEventCore.watchLibrary.Keys.ToArray();

		int sLength = popupArray.Length;
		string[] popupLArray = EventLibrary.monoClasses.Keys.ToArray();
		string[] popupLArrayNice = EventLibrary.monoClassesNice;
		int length = sLength + popupLArray.Length;

		string[] popupFArray = new string[length];
		string[] popupFArrayNice = new string[length];
		for(int i = 0; i < length; i++) {
			if(i < popupArray.Length) {
				popupFArray[i] = popupArray[i];
				popupFArrayNice[i] = popupArrayNice[i];
			} else {
				popupFArray[i] = popupLArray[i - popupArray.Length];
				popupFArrayNice[i] = popupLArrayNice[i - popupArray.Length];
			}
		}
		popupArray = popupFArray;
		popupArrayNice = popupFArrayNice;

		condition.e_categoryIndex = FindIndex(condition.e_categoryString, popupArray);
		condition.e_categoryIndex = (int)EditorGUILayout.Popup(condition.e_categoryIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth / 3));
		condition.e_categoryString = popupArray[condition.e_categoryIndex];

		if(condition.e_categoryString == "Static Script") {
			popupArray = EventLibrary.staticClasses.Keys.ToArray();
			popupArrayNice = EventLibrary.staticClassesNice;
		} else if(condition.e_categoryString == "System") {
			popupArray = PopEventCore.watchLibrary[condition.e_categoryString];
			popupArrayNice = PopEventCore.watchLibrary[condition.e_categoryString];
		} else {
			popupArray = EventLibrary.monoClasses.Keys.ToArray();
			popupArrayNice = EventLibrary.monoClassesNice;
		}

		if(popupArray.Length < 1) {
			GUILayout.EndHorizontal();
			condition.editorHeight = 40;
			EditorGUILayout.LabelField("No valid Classes in that Category", GUILayout.MaxWidth(columnWidth));
			return true;
		}

		if(condition.e_categoryString == "System") {
			condition.e_classIndex = FindIndex(condition.e_classString, popupArray);
			condition.e_classIndex = (int)EditorGUILayout.Popup(condition.e_classIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth * 2 / 3));
			condition.e_classString = popupArray[condition.e_classIndex];
		} else {
			condition.e_classIndex = condition.e_categoryIndex - sLength;
			condition.e_classString = condition.e_categoryString;
		}

		GUI.backgroundColor = Color.red;
		if(popTarget.conditions.Count > 1 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
			RemoveCondition(count - 1);
			return false;
		} else if(popTarget.conditions.Count == 1) {
			GUILayout.Label(" ", GUILayout.MaxWidth(20));
		}

		GUI.backgroundColor = Color.white;

		GUILayout.EndHorizontal();

		if(condition.e_classString == "Choose A Condition") {
			condition.editorHeight = 20;
			condition.color = new Color(0, 0.58f, 0.69f, 0.25f);
			chooseACondition = true;
		} else {
			condition.color = new Color(0, 0.58f, 0.69f, 0.45f);
			if(condition.e_categoryString == "Static Script") {
				condition.editorHeight = 94;
				DrawWatchStaticScript(condition);
			} else if(condition.e_categoryString == "System") {
				DrawSpecialCondition(condition);
			} else {
				condition.editorHeight = 60;
				DrawWatchScript(condition);
			}
		}

		EditorGUILayout.Space();
		return true;
	}

	void DrawWatchScript(EventHalf condition) {

		string[] popupArray = new string[0];
		string[] popupArrayNice = new string[0];

		if(!EventLibrary.instanceClasses[condition.e_classString]) {
			EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(columnWidth));
			condition.e_GameObject = (GameObject)EditorGUILayout.ObjectField(condition.e_GameObject, typeof(GameObject), true, GUILayout.MaxWidth(columnWidth));
		} else {
			;//condition.e_GameObject = typeof(condition.e_classString)
		}
		if(condition.e_GameObject == null) {
			condition.e_MonoBehaviour = null;
		}
		if(condition.e_GameObject != null) {
			condition.e_MonoBehaviour = condition.e_GameObject.GetComponent(condition.e_classString) as MonoBehaviour;
		}
		if(condition.e_MonoBehaviour == null) {
			condition.e_GameObject = null;
			EditorGUILayout.LabelField("Select a GameObject with attached script:", GUILayout.MaxWidth(columnWidth));
			EditorGUILayout.LabelField("<b>" + condition.e_classString + "</b>", style, GUILayout.MaxWidth(columnWidth));
			condition.editorHeight += 34;
		}

		if(condition.e_MonoBehaviour != null && condition.e_MonoBehaviour.GetType().ToString() != condition.e_classString) {
			condition.e_MonoBehaviour = null;
		}

		if(condition.e_MonoBehaviour != null) {
			condition.editorHeight += 70;
			if(EventLibrary.library.ContainsKey(condition.e_classString + "Fields")) {
				popupArray = EventLibrary.library[condition.e_classString + "Fields"];
				popupArrayNice = EventLibrary.libraryNice[condition.e_classString + "Fields"];
			}
			if(popupArray.Length > 0) {
				EditorGUILayout.LabelField("Condition", GUILayout.MaxWidth(columnWidth));
				condition.e_fieldIndex = FindIndex(condition.e_fieldString, popupArray);
				condition.e_fieldIndex = (int)EditorGUILayout.Popup(condition.e_fieldIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth));
				condition.e_fieldString = popupArray[condition.e_fieldIndex];
				condition.e_fieldType = condition.e_MonoBehaviour.GetType().GetField(condition.e_fieldString).FieldType;

				DrawWatchField(condition);
			} else {
				EditorGUILayout.LabelField("<b>No Valid Fields</b>", style, GUILayout.MaxWidth(columnWidth));
			}
		}
	}
	void DrawWatchStaticScript(EventHalf condition) {
		string[] popupArray = new string[0];
		string[] popupArrayNice = new string[0];

		if(EventLibrary.library.ContainsKey(condition.e_classString + "Fields")) {
			popupArray = EventLibrary.library[condition.e_classString + "Fields"];
			popupArrayNice = EventLibrary.libraryNice[condition.e_classString + "Fields"];
		}
		if(popupArray.Length > 0) {
			EditorGUILayout.LabelField("Condition", GUILayout.MaxWidth(columnWidth));
			condition.e_fieldIndex = FindIndex(condition.e_fieldString, popupArray);
			condition.e_fieldIndex = (int)EditorGUILayout.Popup(condition.e_fieldIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth));
			condition.e_fieldString = popupArray[condition.e_fieldIndex];
			if(EventLibrary.staticClasses[condition.e_classString].GetField(condition.e_fieldString) != null) {
				condition.e_fieldType = EventLibrary.staticClasses[condition.e_classString].GetField(condition.e_fieldString).FieldType;
			}

			DrawWatchField(condition);
		}
	}

	void DrawWatchField(EventHalf condition) {
		EditorGUILayout.LabelField("Target Value", GUILayout.MaxWidth(columnWidth));

		//  Draw above Comparison Popout
		if(condition.e_fieldType == typeof(Dictionary<string, int>)) {
			condition.p_string[0] = EditorGUILayout.TextField(condition.p_string[0], GUILayout.MaxWidth(columnWidth));
			condition.editorHeight += 20;
		}

		GUILayout.BeginHorizontal();

		//     ComparisonOption
		if(condition.e_fieldType == typeof(System.Int32) || condition.e_fieldType == typeof(System.Single)) {
			condition.compareIndex = FindIndex(condition.compareString, condition.numberCompareString);
			condition.compareIndex = EditorGUILayout.Popup(condition.compareIndex, condition.numberCompareString, GUILayout.MaxWidth(columnWidth / 2));
			condition.compareString = condition.numberCompareString[condition.compareIndex];
		} else if(condition.e_fieldType == typeof(Vector3)) {
			condition.compareIndex = FindIndex(condition.compareString, condition.vectorCompareString);
			condition.compareIndex = EditorGUILayout.Popup(condition.compareIndex, condition.vectorCompareString, GUILayout.MaxWidth(columnWidth / 2));
			condition.compareString = condition.vectorCompareString[condition.compareIndex];
		} else if(condition.e_fieldType == typeof(System.Boolean)) {
			condition.compareIndex = FindIndex(condition.compareString, condition.boolCompareString);
			condition.compareIndex = EditorGUILayout.Popup(condition.compareIndex, condition.boolCompareString, GUILayout.MaxWidth(columnWidth / 2));
			condition.compareString = condition.boolCompareString[condition.compareIndex];
		} else if(condition.e_fieldType == typeof(Dictionary<string, int>)) {
			condition.compareIndex = FindIndex(condition.compareString, condition.numberCompareString);
			condition.compareIndex = EditorGUILayout.Popup(condition.compareIndex, condition.numberCompareString, GUILayout.MaxWidth(columnWidth / 2));
			condition.compareString = condition.numberCompareString[condition.compareIndex];
		} else if(condition.e_fieldType == typeof(System.String)) {
			condition.compareIndex = FindIndex(condition.compareString, condition.stringCompareString);
			condition.compareIndex = EditorGUILayout.Popup(condition.compareIndex, condition.stringCompareString, GUILayout.MaxWidth(columnWidth / 2));
			condition.compareString = condition.stringCompareString[condition.compareIndex];
		}

		//     Value Field
		if(condition.e_fieldType == typeof(System.Int32)) {
			condition.p_int[0] = EditorGUILayout.IntField(condition.p_int[0], GUILayout.MaxWidth(columnWidth / 2));
		} else if(condition.e_fieldType == typeof(System.String)) {
			condition.p_string[0] = EditorGUILayout.TextField(condition.p_string[0], GUILayout.MaxWidth(columnWidth / 2));
		} else if(condition.e_fieldType == typeof(System.Single) || condition.e_fieldType == typeof(Vector3)) {
			condition.p_float[0] = EditorGUILayout.FloatField(condition.p_float[0], GUILayout.MaxWidth(columnWidth / 2));
		} else if(condition.e_fieldType == typeof(Dictionary<string, int>)) {
			condition.p_int[0] = EditorGUILayout.IntField(condition.p_int[0], GUILayout.MaxWidth(columnWidth / 2));
		} else {
			for(int sp = 0; sp < 6; sp++) { EditorGUILayout.Space(); }
		}
		GUILayout.EndHorizontal();
	}

	void DrawSpecialCondition(EventHalf condition) {
		if(condition.e_classString == "Player Enters Area" || condition.e_classString == "Player Leaves Area") {
			DrawPlayerEntersArea(condition);
		} else if(condition.e_classString == "Wait X Seconds") {
			DrawWaitXSeconds(condition);
		} else if(condition.e_classString == "Collect X Items") {
			DrawCollectXItems(condition);
		}
	}

	void DrawPlayerEntersArea(EventHalf condition) {
		condition.editorHeight = 42;
		popTarget.drawRegionTwo = true;

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Radius", GUILayout.MaxWidth(columnWidth / 3));
		popTarget.conditionRegionRadius = EditorGUILayout.FloatField(popTarget.conditionRegionRadius, GUILayout.MaxWidth(columnWidth / 3 * 2));
		GUILayout.EndHorizontal();
	}

	void DrawWaitXSeconds(EventHalf condition) {
		condition.editorHeight = 60;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Seconds to Wait", GUILayout.MaxWidth(columnWidth / 2));
		condition.p_float[0] = EditorGUILayout.FloatField(condition.p_float[0], GUILayout.MaxWidth(columnWidth / 2));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Seconds Waited", GUILayout.MaxWidth(columnWidth / 2));
		EditorGUILayout.LabelField(popTarget.totalTimeActive.ToString(), GUILayout.MaxWidth(columnWidth / 2));
		EditorGUILayout.EndHorizontal();
	}

	void DrawCollectXItems(EventHalf condition) {
		condition.editorHeight = 78;
		EditorGUILayout.LabelField("<b>NOT YET IMPLEMENTED</b>", style, GUILayout.MaxWidth(columnWidth));

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Collect", GUILayout.MaxWidth(columnWidth / 3));
		condition.p_int[0] = EditorGUILayout.IntField(condition.p_int[0], GUILayout.MaxWidth(columnWidth / 3));
		EditorGUILayout.LabelField("Items", GUILayout.MaxWidth(columnWidth / 3));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Named", GUILayout.MaxWidth(columnWidth / 3));
		condition.p_string[0] = EditorGUILayout.TextField(condition.p_string[0], GUILayout.MaxWidth(columnWidth * 2 / 3));
		EditorGUILayout.EndHorizontal();
	}

	//  Draw Actions    -------------------------------------------------------------------------------------------------
	void DrawActions() {
		EditorGUILayout.BeginVertical();
		int count = 0;
		foreach(var action in popTarget.actions) {
			count++;
			if(DrawOneAction(action, count) == false) {
				break;
			}
		}
		EditorGUILayout.EndVertical();
	}

	bool DrawOneAction(EventHalf action, int count) {
		string[] popupArray;
		string[] popupArrayNice;
		DrawBackground(action.editorHeight, action.color);

		EditorGUILayout.BeginHorizontal();
		popupArray = PopEventCore.executeLibrary.Keys.ToArray();
		popupArrayNice = PopEventCore.executeLibrary.Keys.ToArray();

		int sLength = popupArray.Length;
		string[] popupLArray = EventLibrary.monoClasses.Keys.ToArray();
		string[] popupLArrayNice = EventLibrary.monoClassesNice;
		int length = sLength + popupLArray.Length;

		string[] popupFArray = new string[length];
		string[] popupFArrayNice = new string[length];
		for(int i = 0; i < length; i++) {
			if(i < popupArray.Length) {
				popupFArray[i] = popupArray[i];
				popupFArrayNice[i] = popupArrayNice[i];
			} else {
				popupFArray[i] = popupLArray[i - popupArray.Length];
				popupFArrayNice[i] = popupLArrayNice[i - popupArray.Length];
			}
		}
		popupArray = popupFArray;
		popupArrayNice = popupFArrayNice;

		action.e_categoryIndex = FindIndex(action.e_categoryString, popupArray);
		action.e_categoryIndex = (int)EditorGUILayout.Popup(action.e_categoryIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth / 3));
		action.e_categoryString = popupArray[action.e_categoryIndex];

		if(action.e_categoryString == "Static Script") {
			popupArray = EventLibrary.staticClasses.Keys.ToArray();
			popupArrayNice = EventLibrary.staticClassesNice;
		} else if(action.e_categoryString == "System") {
			popupArray = PopEventCore.executeLibrary[action.e_categoryString];
			popupArrayNice = PopEventCore.executeLibrary[action.e_categoryString];
		} else {//if (PopEventCore.executeLibrary.ContainsKey(action.e_categoryString)) {
			popupArray = EventLibrary.monoClasses.Keys.ToArray();
			popupArrayNice = EventLibrary.monoClassesNice;
			//popupArray = PopEventCore.executeLibrary[action.e_categoryString];
			//popupArrayNice = PopEventCore.executeLibrary[action.e_categoryString];
		}
		if(popupArray.Length < 1) {
			GUILayout.EndHorizontal();
			action.editorHeight = 40;
			EditorGUILayout.LabelField("No valid Classes in that Category", GUILayout.MaxWidth(columnWidth));
			return true;
		}

		if(action.e_categoryString == "System") {
			action.e_classIndex = FindIndex(action.e_classString, popupArray);
			action.e_classIndex = (int)EditorGUILayout.Popup(action.e_classIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth * 2 / 3));
			action.e_classString = popupArray[action.e_classIndex];
		} else {
			action.e_classIndex = action.e_categoryIndex - sLength;
			action.e_classString = action.e_categoryString;
		}

		GUI.backgroundColor = Color.red;
		if(popTarget.actions.Count > 1 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
			RemoveAction(count - 1);
			return false;
		} else if(popTarget.actions.Count == 1) {
			GUILayout.Label(" ", GUILayout.MaxWidth(20));
		}

		GUI.backgroundColor = Color.white;
		EditorGUILayout.EndHorizontal();

		if(action.e_classString == "Choose An Action") {
			action.editorHeight = 20;
			action.color = new Color(1, 0.46f, 0, 0.35f);
			chooseAnAction = true;
		} else {
			action.color = new Color(1, 0.46f, 0, 0.55f);
			if(action.e_categoryString == "Static Script") {
				action.editorHeight = 60;
				DrawExecuteStaticFunction(action);
			} else if(action.e_categoryString == "System") {
				DrawSpecialAction(action);
			} else if(action.e_classString == "Destroy This Object") {
				action.editorHeight = 20;
				destroyThisObject = true;
			}
				/*else if (action.e_classString == "Move Player To Location" || action.e_classString == "Play Sound") {
					action.editorHeight = 38;
					EditorGUILayout.LabelField("<b>NOT YET IMPLEMENTED</b>", style, GUILayout.MaxWidth(columnWidth));
				}*/
			  else {
				action.editorHeight = 60;
				DrawExecuteFunction(action);
			}
		}

		EditorGUILayout.Space();
		return true;
	}

	void DrawExecuteFunction(EventHalf action) {
		string[] popupArray = new string[0];
		string[] popupArrayNice = new string[0];
		//action.e_classString = action.e_categoryString;


		if(!EventLibrary.instanceClasses[action.e_classString]) {
			EditorGUILayout.LabelField("Target Object", GUILayout.MaxWidth(columnWidth));
			action.e_GameObject = (GameObject)EditorGUILayout.ObjectField(action.e_GameObject, typeof(GameObject), true, GUILayout.MaxWidth(columnWidth));

			if(action.e_GameObject == null) {
				action.e_MonoBehaviour = null;
			} else {
				action.e_MonoBehaviour = action.e_GameObject.GetComponent(action.e_classString) as MonoBehaviour;
			}
			if(action.e_MonoBehaviour == null) {
				action.e_GameObject = null;
				//EditorGUILayout.LabelField("Select a GameObject with attached script:", GUILayout.MaxWidth(columnWidth));
				//EditorGUILayout.LabelField("<b>" + action.e_classString + "</b>", style, GUILayout.MaxWidth(columnWidth));
				//action.editorHeight += 34;
			}
		} else {
			//todo: instance based classes
		}

		if(action.e_MonoBehaviour != null) {
			action.editorHeight += 32;

			if(EventLibrary.library.ContainsKey(action.e_classString + "Methods")) {
				popupArray = EventLibrary.library[action.e_classString + "Methods"];
				popupArrayNice = EventLibrary.libraryNice[action.e_classString + "Methods"];
			}
			if(popupArray.Length > 0) {
				EditorGUILayout.LabelField("Action Function", GUILayout.MaxWidth(columnWidth));
				action.e_fieldIndex = FindIndex(action.e_fieldString, popupArray);
				action.e_fieldIndex = (int)EditorGUILayout.Popup(action.e_fieldIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth));
				action.e_fieldString = popupArray[action.e_fieldIndex];

				//  Determine type to pass
				System.Type[] paramType = new System.Type[] { typeof(void) };

				if(popupArray.Length > action.e_fieldIndex) {
					action.e_fieldString = popupArray[action.e_fieldIndex];
					System.Reflection.ParameterInfo[] par = action.e_MonoBehaviour.GetType().GetMethod(action.e_fieldString).GetParameters();
					if(par.Length > 0) {
						paramType = new System.Type[par.Length];
					}
					if(par.Length > 5) {
						Debug.LogWarning("Event functions cannot have more than 5 parameters");
						return;
					}
					for(int i = 0; i < par.Length; i++) {
						paramType[i] = par[i].ParameterType;
						DrawExecuteParameter(action, paramType[i], i);
					}
				}

				//  Set the parameters
				action.args = action.SetParameters(paramType);
			} else {
				EditorGUILayout.LabelField("<b>No Valid Methods</b>", style, GUILayout.MaxWidth(columnWidth));
			}
		}
	}

	void DrawExecuteStaticFunction(EventHalf action) {
		string[] popupArray = new string[0];
		string[] popupArrayNice = new string[0];

		if(EventLibrary.library.ContainsKey(action.e_classString + "Methods")) {
			popupArray = EventLibrary.library[action.e_classString + "Methods"];
			popupArrayNice = EventLibrary.libraryNice[action.e_classString + "Methods"];
		}

		if(popupArray.Length > 0) {
			EditorGUILayout.LabelField("Action Function", GUILayout.MaxWidth(columnWidth));
			action.e_fieldIndex = FindIndex(action.e_fieldString, popupArray);
			action.e_fieldIndex = (int)EditorGUILayout.Popup(action.e_fieldIndex, popupArrayNice, GUILayout.MaxWidth(columnWidth));
			action.e_fieldString = popupArray[action.e_fieldIndex];

			//  Determine type to pass
			System.Type[] paramType = new System.Type[] { typeof(void) };
			if(EventLibrary.staticClasses[action.e_classString].GetMethod(action.e_fieldString) != null) {
				System.Reflection.ParameterInfo[] par = EventLibrary.staticClasses[action.e_classString].GetMethod(action.e_fieldString).GetParameters();
				if(par.Length > 0) {
					paramType = new System.Type[par.Length];
				}

				if(par.Length > 5) {
					Debug.LogWarning("Event functions cannot have more than 5 parameters");
					return;
				}
				for(int i = 0; i < par.Length; i++) {
					paramType[i] = par[i].ParameterType;
					DrawExecuteParameter(action, paramType[i], i);
				}

				//  Set the parameters
				action.args = action.SetParameters(paramType);
			}
		}
	}

	void DrawExecuteParameter(EventHalf action, System.Type paramType, int i) {
		action.editorHeight += 34;
		//  Label for non-null
		if(paramType != typeof(void)) {
			EditorGUILayout.LabelField(paramType.Name + " to Pass", GUILayout.MaxWidth(columnWidth));
		}
		//  Expose the proper variable
		if(paramType == typeof(System.Int32)) {
			action.p_int[i] = EditorGUILayout.IntField(action.p_int[i], GUILayout.MaxWidth(columnWidth));
		} else if(paramType == typeof(System.Single)) {
			action.p_float[i] = EditorGUILayout.FloatField(action.p_float[i], GUILayout.MaxWidth(columnWidth));
		} else if(paramType == typeof(System.String)) {
			action.p_string[i] = EditorGUILayout.TextField(action.p_string[i], GUILayout.MaxWidth(columnWidth));
		} else if(paramType == typeof(Vector3)) {
			action.p_Vector3[i] = EditorGUILayout.Vector3Field("", action.p_Vector3[i], GUILayout.MaxWidth(columnWidth));
		} else if(paramType == typeof(GameObject)) {
			action.p_GameObject[i] = (GameObject)EditorGUILayout.ObjectField(action.p_GameObject[i], typeof(GameObject), true, GUILayout.MaxWidth(columnWidth));
		} else if(paramType == typeof(MonoBehaviour)) {
			action.p_MonoBehaviour[i] = (MonoBehaviour)EditorGUILayout.ObjectField(action.p_MonoBehaviour[i], typeof(MonoBehaviour), true, GUILayout.MaxWidth(columnWidth));
		} else if(paramType == typeof(System.Boolean)) {
			int boolIndex = 0;
			string[] boolPopup = { "True", "False" };
			if(action.p_bool[i] == false) { boolIndex = 1; }
			boolIndex = EditorGUILayout.Popup(boolIndex, boolPopup, GUILayout.MaxWidth(columnWidth));
			if(boolIndex == 0) { action.p_bool[i] = true; } else { action.p_bool[i] = false; }
		}
	}

	void DrawSpecialAction(EventHalf action) {
		if(action.e_classString == "Execute Function") {
			DrawExecuteFunction(action);
		} else if(action.e_classString == "Debug Message") {
			DrawDebugMessage(action);
		} else if(action.e_classString == "Activate Another Event" || action.e_classString == "Deactivate Another Event") {
			DrawActivateAnotherEvent(action);
		} else if(action.e_classString == "Set Objective") {
			DrawSetObjective(action);
		} else if(action.e_classString == "Set Dialogue") {
			DrawSetDialogue(action);
		} else if(action.e_classString == "Create Prefab At Position") {
			DrawCreatePrefabAtPosition(action);
		} else if(action.e_classString == "Create Prefab Here") {
			DrawCreatePrefabHere(action);
		} else if(action.e_classString == "Move This Object") {
			DrawMoveThisObject(action);
		} else if(action.e_classString == "Add X Items") {
			DrawAddXItems(action);
		}
	}
	void DrawDebugMessage(EventHalf action) {
		action.editorHeight = 42;
		action.p_string[0] = EditorGUILayout.TextField(action.p_string[0], GUILayout.MaxWidth(columnWidth));
	}

	void DrawActivateAnotherEvent(EventHalf action) {
		action.editorHeight = 42;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Event Id", GUILayout.MaxWidth(columnWidth / 2));
		action.p_string[0] = EditorGUILayout.TextField(action.p_string[0], GUILayout.MaxWidth(columnWidth / 2));
		EditorGUILayout.EndHorizontal();
	}

	void DrawSetObjective(EventHalf action) {
		action.editorHeight = 42; //78
		//EditorGUILayout.LabelField("<b>NOT YET IMPLEMENTED</b>", style, GUILayout.MaxWidth(columnWidth));

		//EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.LabelField("Id", GUILayout.MaxWidth(columnWidth / 2));
		action.p_string[0] = EditorGUILayout.TextField(action.p_string[0], GUILayout.MaxWidth(columnWidth / 1));
		//EditorGUILayout.EndHorizontal();

		//action.p_string[1] = EditorGUILayout.TextArea(action.p_string[1], GUILayout.MaxWidth(columnWidth));
	}
	void DrawSetDialogue(EventHalf action) {
		action.editorHeight = 58;
		action.p_string[0] = EditorGUILayout.TextField(action.p_string[0], GUILayout.MaxWidth(columnWidth / 1));
		action.p_string[1] = EditorGUILayout.TextField(action.p_string[1], GUILayout.MaxWidth(columnWidth / 1));
	}
	void DrawCreatePrefabAtPosition(EventHalf action) {
		action.editorHeight = 110;
		action.p_GameObject[0] = (GameObject)EditorGUILayout.ObjectField(action.p_GameObject[0], typeof(GameObject), false, GUILayout.MaxWidth(columnWidth));
		EditorGUILayout.LabelField("World Position", style, GUILayout.MaxWidth(columnWidth));
		action.p_Vector3[0] = EditorGUILayout.Vector3Field("", action.p_Vector3[0], GUILayout.MaxWidth(columnWidth));
		EditorGUILayout.LabelField("Rotation", style, GUILayout.MaxWidth(columnWidth));
		action.p_Vector3[1] = EditorGUILayout.Vector3Field("", action.p_Vector3[1], GUILayout.MaxWidth(columnWidth));
	}

	void DrawMoveThisObject(EventHalf action) {
		action.editorHeight = 100;
		EditorGUILayout.LabelField("New World Position", style, GUILayout.MaxWidth(columnWidth));
		action.p_Vector3[0] = EditorGUILayout.Vector3Field("", action.p_Vector3[0], GUILayout.MaxWidth(columnWidth));
		EditorGUILayout.LabelField("Rotation", style, GUILayout.MaxWidth(columnWidth));
		action.p_Vector3[1] = EditorGUILayout.Vector3Field("", action.p_Vector3[1], GUILayout.MaxWidth(columnWidth));
	}

	void DrawCreatePrefabHere(EventHalf action) {
		action.editorHeight = 42;
		action.p_GameObject[0] = (GameObject)EditorGUILayout.ObjectField(action.p_GameObject[0], typeof(GameObject), false, GUILayout.MaxWidth(columnWidth));
	}

	void DrawAddXItems(EventHalf action) {
		action.editorHeight = 80;
		EditorGUILayout.LabelField("<b>NOT YET IMPLEMENTED</b>", style, GUILayout.MaxWidth(columnWidth));

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Add", GUILayout.MaxWidth(columnWidth / 3));
		action.p_int[0] = EditorGUILayout.IntField(action.p_int[0], GUILayout.MaxWidth(columnWidth / 3));
		EditorGUILayout.LabelField("Items", GUILayout.MaxWidth(columnWidth / 3));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Named", GUILayout.MaxWidth(columnWidth / 3));
		action.p_string[0] = EditorGUILayout.TextField(action.p_string[0], GUILayout.MaxWidth(columnWidth * 2 / 3));
		EditorGUILayout.EndHorizontal();
	}

	void DrawBackground(string type, int extraLength = 0) {
		if(type == "Execution Complete") {
			DrawBackground(70, new Color(1, 0, 0, 0.25f), true);
		}
	}

	void DrawBackground(int height, Color color, bool doubleWidth = false) {
		Rect rt = GUILayoutUtility.GetRect(0, 0);
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		if(doubleWidth == true) {
			GUI.Box(new Rect(rt.x, rt.y, Screen.width * 0.93f, height), GUIContent.none);
		} else {
			GUI.Box(new Rect(rt.x, rt.y, Screen.width / 2.15f, height), GUIContent.none);
		}
	}

	//     Buttons
	void AddCondition() {
		Undo.RecordObject(popTarget, "Add Condition");
		popTarget.conditions.Add(new EventHalf());
	}

	void AddAction() {
		Undo.RecordObject(popTarget, "Add Action");
		popTarget.actions.Add(new EventHalf());
	}

	void RemoveCondition(int index) {
		Undo.RecordObject(popTarget, "Delete Condition");
		popTarget.conditions.RemoveAt(index);
	}

	void RemoveAction(int index) {
		Undo.RecordObject(popTarget, "Delete Action");
		popTarget.actions.RemoveAt(index);
	}

	int FindIndex(string name, string[] names) {
		for(int i=0; i < names.Length; i++) {
			if(name == names[i]) {
				return i;
			}
		}
		return 0;
	}
}