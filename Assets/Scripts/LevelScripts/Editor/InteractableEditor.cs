using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Interactable))]

public class InteractableEditor : Editor 
{
	Interactable thisObject;

	public override void OnInspectorGUI()
	{
		thisObject = (Interactable)target;
		thisObject.Type = (Interactable.ObjectType)EditorGUILayout.EnumPopup ("Type: ", thisObject.Type);

		switch (thisObject.Type) 
		{
			case Interactable.ObjectType.Ladder:
				if(thisObject.ladderStart == Vector3.zero && thisObject.ladderEnd == Vector3.zero)
				{
					thisObject.ladderStart = thisObject.transform.position;
					thisObject.ladderEnd = thisObject.transform.position + Vector3.up;
				}

				thisObject.ladderStart = EditorGUILayout.Vector3Field("Start: ", thisObject.ladderStart);
				thisObject.ladderEnd = EditorGUILayout.Vector3Field("End: ", thisObject.ladderEnd);
				break;
		}

		if (GUI.changed) {
			EditorUtility.SetDirty(thisObject);
		}

	}

	public void OnSceneGUI()
	{
		thisObject = (Interactable)target;

		switch (thisObject.Type) 
		{
			case Interactable.ObjectType.Ladder:
				// Show Position Handles and Labels
				Handles.Label(thisObject.ladderStart, "Start");
				thisObject.ladderStart = Handles.PositionHandle(thisObject.ladderStart, Quaternion.identity);
				Handles.Label (thisObject.ladderEnd, "End");
				thisObject.ladderEnd = Handles.PositionHandle(thisObject.ladderEnd, Quaternion.identity);
				break;
		}

		Repaint ();
	}
}
