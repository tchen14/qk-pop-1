using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class LadderPoint : Placeholder {
	[SerializeField]
	private GameObject upDestination;
	[SerializeField]
	private GameObject downDestination;
	
	void OnDrawGizmos() {
		//Gizmos.DrawIcon(transform.position, "sidleGizmo.png");
	}
}
