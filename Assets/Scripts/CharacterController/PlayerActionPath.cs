using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
public sealed class PlayerActionPath : MonoBehaviour {
	public enum PlayerAction { sidle, ladder };
	public PlayerAction actionType = PlayerAction.sidle;

	public List<Vector3> path = new List<Vector3>();

	void Start() {
		if (path.Count == 0) {
			Debug.Log("safety", "ActionPath at " + transform.position + " has less than 2 positions.");
			Destroy(this.gameObject);
		}
	}

	void OnDrawGizmos() {
		if (actionType == PlayerAction.sidle)
			Gizmos.color = Color.cyan;
		else if (actionType == PlayerAction.ladder)
			Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, path[0]);
		for(int i = 0; i < path.Count - 1; i++){
			Gizmos.DrawLine(path[i], path[i+1]);
		}
	}

#if UNITY_EDITOR
	public bool BuildCheck(){
		if (path.Count == 0) {
			Debug.Log("safety", "ActionPath at " + transform.position + " is invalid.");
			return false;
		}
		return true;
	}
#endif
}