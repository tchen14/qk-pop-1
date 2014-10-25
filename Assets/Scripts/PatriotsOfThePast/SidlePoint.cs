using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class SidlePoint : Placeholder {
	[SerializeField]
	private GameObject leftDestination;
	[SerializeField]
	private GameObject rightDestination;
    [SerializeField]
    private bool startPoint = true;

    void OnDrawGizmos() {
        //Gizmos.DrawIcon(transform.position, "sidleGizmo.png");
    }
}
