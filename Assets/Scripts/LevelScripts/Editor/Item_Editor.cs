using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Item), true)]
public class Item_Editor : Editor {
	Item script_target;
	string item_types = {"Crate", "Rope", "Well", "Enemy"};
	int item_types_index = 0;
	int item_types_index_old;

	enum selection {
		item_type
	};


	void OnEnable(){
		script_target = (Item)target;
	}

	override public void OnInspectorGUI(){
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical();
		item_types_index = EditorGUILayout.Popup ("Item Type:", item_types_index, item_types);
		EditorGUILayout.LabelField ("item_type: ", script_target.itemType);
		EditorGUILayout.EndVertical();

		if (selection_changed (selection.item_type)) {
			script_target.itemType = item_types[item_types_index];
		}
	}

	bool selection_changed(selection s){
		switch(s){
			case selection.item_type:
			if(item_types_index != item_types_index_old){
				item_types_index_old = item_types_index;
				return true;
			}
			else return false;
			break;
		}
	}

}
