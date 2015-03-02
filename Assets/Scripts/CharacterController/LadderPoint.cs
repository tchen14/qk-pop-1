using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class LadderPoint : PlayerAction {
	[SerializeField]
	private GameObject upDestination;
	[SerializeField]
	private GameObject downDestination;
	
	void OnDrawGizmos() {
		//Gizmos.DrawIcon(transform.position, "sidleGizmo.png");
	}
}
