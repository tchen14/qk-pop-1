using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Item), true)]
public class Item_Editor : Editor {

	struct item_settings {
		bool _quinc;
		bool _push;
		bool _pull;
		bool _cut;
		bool _stun;
		bool _sound;
		bool _heat;
		bool _cold;
		bool _blast;

		public item_settings(bool quinc, bool push, bool pull, bool cut, bool stun, bool sound, bool heat, bool cold, bool blast){
			_quinc = quinc;
			_push = push;
			_pull = pull;
			_cut = cut;
			_stun = stun;
			_sound = sound;
			_heat = heat;
			_cold = cold;
			_blast = blast;
		}

		public void load(Item i){
			i.quincAffected = _quinc;
			i.pushCompatible = _push;
			i.pullCompatible = _pull;
			i.cutCompatible = _cut;
			i.stunCompatible = _stun;
			i.soundThrowCompatible = _sound;
			i.heatCompatible = _heat;
			i.coldCompatible = _cold;
			i.blastCompatible = _blast;
		}
	}

	public enum selection {
		item_type
	};

	Item script_target;
	string[] item_types = new string[]{"Crate", "Rope", "Well", "Enemy"};
	int item_types_index = 0;
	int item_types_index_old = 0;

	item_settings crate_settings = new item_settings (true, true, true, false, false, false, false, false, false);
	item_settings rope_settings = new item_settings (true, false, false, true, false, false, false, false, false);
	item_settings well_settings = new item_settings (true, false, false, false, false, true, false, false, false);
	item_settings enemy_settings = new item_settings (true, false, false, false, true, false, false, false, false);

	void OnEnable(){
		script_target = (Item)target;
		item_types_index = script_target.itemIndex;
	}

	override public void OnInspectorGUI(){
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical();
		item_types_index = EditorGUILayout.Popup ("Item Type:", item_types_index, item_types);

		if (script_target.pushCompatible || script_target.pullCompatible) {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Push Pull type:", GUILayout.MaxWidth(125));
			script_target.current_push_type = (Item.push_type)EditorGUILayout.EnumPopup(script_target.current_push_type);
			EditorGUILayout.EndHorizontal();
		}

		script_target.quincAffected = EditorGUILayout.Toggle ("Quinc Affected", script_target.quincAffected);

		if (!script_target.quincAffected) {
			GUI.enabled = false;
		}

		script_target.pushCompatible = EditorGUILayout.Toggle ("Can be pushed", script_target.pushCompatible);
		script_target.pullCompatible = EditorGUILayout.Toggle ("Can be pulled", script_target.pullCompatible);
		script_target.cutCompatible = EditorGUILayout.Toggle ("Can be cut", script_target.cutCompatible);
		script_target.soundThrowCompatible = EditorGUILayout.Toggle ("Can be soundthrowed", script_target.soundThrowCompatible);
		script_target.coldCompatible = EditorGUILayout.Toggle ("Can be frozen", script_target.coldCompatible);
		script_target.heatCompatible = EditorGUILayout.Toggle ("Can be heat up", script_target.heatCompatible);
		script_target.blastCompatible = EditorGUILayout.Toggle ("Can be blasted", script_target.blastCompatible);
		script_target.stunCompatible = EditorGUILayout.Toggle ("Can be stunned", script_target.stunCompatible);

		GUI.enabled = true;
		EditorGUILayout.EndVertical();

		if (selection_changed (selection.item_type)) {
			script_target.itemType = (item_type)item_types_index;
			script_target.itemIndex = item_types_index;

			switch(item_types [item_types_index]){
				case "Crate":
					crate_settings.load(script_target);
					script_target.moveDist = 0f;
					script_target.startPosition = script_target.transform.position;
					script_target.hasMoved = false;
					script_target.isSnapping = false;
					break;
				case "Rope":
					rope_settings.load(script_target);
					break;
				case "Well":
					well_settings.load(script_target);
					break;
				case "Enemy":
					enemy_settings.load(script_target);
					script_target.curStunTimer = 0.0f;
					script_target.stunState = false;
					script_target.soundThrowAffected = true;
					break;
			}
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
			default:
				return false;
		}
	}
}
