using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor (typeof(ActionPath))]
public class ActionPathEditor : Editor{

	ActionPath myTarget;
	float arrowSize = 1;
	
	void OnSceneGUI () {
		myTarget = target as ActionPath;
		bool XAxis = false, YAxis = false, ZAxis = false;
		
		switch(myTarget.actionType){
		case ActionPath.ActionType.Sidle:
			XAxis = ZAxis = true;
			break;
		case ActionPath.ActionType.Ladder:
			YAxis = true;
			break;
		}
		
		if(XAxis){
			Handles.color = Color.red;
			Handles.ArrowCap(0,
			                 myTarget.transform.position + new Vector3(-1,0,1),
			                 myTarget.transform.rotation,
			                 arrowSize);
		}
		if(YAxis){
		Handles.color = Color.green;
		Handles.ArrowCap(0,
		                 myTarget.transform.position + new Vector3(0,0,1),
		                 myTarget.transform.rotation,
		                 arrowSize);
		}
		if(ZAxis){
		Handles.color = Color.blue;
		Handles.ArrowCap(0,
		                 myTarget.transform.position + new Vector3(1,0,1),
		                 myTarget.transform.rotation,
		                 arrowSize);
		}                 
	}
}
