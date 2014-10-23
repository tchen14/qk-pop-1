using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]
public class SidlePoint : Placeholder {
	[SerializeField]
	private GameObject destination;
	[SerializeField]
	private bool startPoint = true;

	void OnDrawGizmos()
	{
		//Gizmos.DrawIcon(transform.position, "sidleGizmo.png");
	}
}
