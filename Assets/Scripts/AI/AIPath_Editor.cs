using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AIPath), true)]
public class AIPath_Editor : Editor {

	AIPath path_target;

	void OnEnable()
	{
		path_target = (AIPath)target;
	}
	override public void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginVertical();



		if (GUILayout.Button ("Add Checkpoint"))
		{

		}

		EditorGUILayout.EndVertical();
	}


}
