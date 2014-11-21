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
	[SerializeField]
	#pragma warning disable
	private bool
		startPoint = true;
	#pragma warning restore

	void OnDrawGizmos()
	{
		//Gizmos.DrawIcon(transform.position, "sidleGizmo.png");
	}
}
