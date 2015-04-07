using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(PlayerActionPath))]
public class PlayerActionPathEditor : Editor {
	PlayerActionPath myTarget;
	SphereCollider col1;
	BoxCollider col2;

	void OnEnable() {
		myTarget = target as PlayerActionPath;
		col1 = myTarget.GetComponent<SphereCollider>();
		col2 = myTarget.GetComponent<BoxCollider>();
	}
	void OnDisable() {
		col1.center = Vector3.zero;
		col2.center = myTarget.transform.position + myTarget.path[myTarget.path.Count - 1];
	}
	
	void OnSceneGUI() {
		if (myTarget.actionType == PlayerActionPath.PlayerAction.sidle)
			Handles.color = Color.cyan;
		else
			Handles.color = Color.red;

		Handles.Label(myTarget.transform.position, "start");
		Vector3[] v = new Vector3[1 + myTarget.path.Count];
		v[0] = myTarget.transform.position;
		for(int i = 0; i < myTarget.path.Count; i++) {
			v[i + 1] = myTarget.path[i];
			/*if(i == 0)
				Handles.DrawLine(myTarget.transform.position, myTarget.path[i]);
			else
				Handles.DrawLine(myTarget.path[i-1], myTarget.path[i]);*/
			myTarget.path[i] = Handles.PositionHandle(myTarget.path[i], Quaternion.identity);
			if(myTarget.path.Count - i > 1) {
				Handles.Label(myTarget.path[i], (i + 1).ToString());
			} else {
				Handles.Label(myTarget.path[i], "end");
				col2.center = myTarget.path[i] - myTarget.transform.position;
			}
		}
		Handles.DrawAAPolyLine(20,v);
	}
}
