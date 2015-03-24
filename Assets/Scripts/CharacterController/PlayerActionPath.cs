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
		if(path.Count == 0)
			Debug.Log("safety", "ActionPath at " + transform.position + "has less than 2 positions.");
	}

	void OnGizmo() {

	}
}