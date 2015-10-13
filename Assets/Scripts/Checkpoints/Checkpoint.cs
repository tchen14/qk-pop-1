using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class Checkpoint : MonoBehaviour {
    public float minDist = 0.0f; //!<Minimum distance away from the Checkpoint that CheckpointTrigger needs to be
	public AIPath path_reference;

    void OnDrawGizmos() {
        Gizmos.DrawIcon (transform.position, "checkpointGizmo.png");
    }

	public Vector3 getPosition(){
		return transform.position;
	}
}
