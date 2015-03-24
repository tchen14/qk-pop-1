using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class ActionPath : MonoBehaviour {
	
	public enum ActionType{Sidle, Ladder}
	public ActionType actionType;
	
	[ReadOnly]
	public List<Vector3> path = new List<Vector3>();
}