using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]
public class SidlePoint : Placeholder
{
	[SerializeField]
	private GameObject
		leftDestination;
	[SerializeField]
	private GameObject
		rightDestination;
	#pragma warning disable 0414
	[SerializeField]
	private bool
		startPoint = true;
	#pragma warning restore 0414

	void OnDrawGizmos()
	{
		//Gizmos.DrawIcon(transform.position, "sidleGizmo.png");
	}
}
