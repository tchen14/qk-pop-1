using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Quinc), true)]
public class Quinc_Editor : Editor {

	private Quinc quinc_target;
	private quincy_ability selected_ability;

	void OnEnable(){
		quinc_target = (Quinc)target;
		selected_ability = quincy_ability.Push;
	}

	override public void OnInspectorGUI(){
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Settings for:", GUILayout.MaxWidth(80));
		selected_ability = (quincy_ability)EditorGUILayout.EnumPopup(selected_ability);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical();
		switch (selected_ability) {
		case quincy_ability.Push:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.pushRate = EditorGUILayout.Slider(quinc_target.pushRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Force:", GUILayout.MaxWidth(80));
			quinc_target.pushDistance = EditorGUILayout.Slider(quinc_target.pushDistance, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.pushRange = EditorGUILayout.Slider(quinc_target.pushRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;

		case quincy_ability.Pull:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.pullRate = EditorGUILayout.Slider(quinc_target.pullRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Force:", GUILayout.MaxWidth(80));
			quinc_target.pullDistance = EditorGUILayout.Slider(quinc_target.pullDistance, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.pullRange = EditorGUILayout.Slider(quinc_target.pullRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;

		case quincy_ability.Cut:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.cutRate = EditorGUILayout.Slider(quinc_target.cutRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.cutRange = EditorGUILayout.Slider(quinc_target.cutRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;

		case quincy_ability.SoundThrow:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.soundRate = EditorGUILayout.Slider(quinc_target.soundRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.soundRange = EditorGUILayout.Slider(quinc_target.soundRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;

		case quincy_ability.Stun:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.stunRate = EditorGUILayout.Slider(quinc_target.stunRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.stunRange = EditorGUILayout.Slider(quinc_target.stunRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Stun Time:", GUILayout.MaxWidth(80));
			quinc_target.stunTime = EditorGUILayout.Slider(quinc_target.stunTime, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;

		case quincy_ability.Cool:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.coldRate = EditorGUILayout.Slider(quinc_target.coldRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.coldRange = EditorGUILayout.Slider(quinc_target.coldRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;

		case quincy_ability.Heat:
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Reload Time:", GUILayout.MaxWidth(80));
			quinc_target.heatRate = EditorGUILayout.Slider(quinc_target.heatRate, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Range:", GUILayout.MaxWidth(80));
			quinc_target.heatRange = EditorGUILayout.Slider(quinc_target.heatRange, 0.0f, 50.0f);
			EditorGUILayout.EndHorizontal();
			break;
		}
		EditorGUILayout.EndVertical();

		EditorGUI.EndChangeCheck();
	}
}
