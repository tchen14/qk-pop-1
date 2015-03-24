using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class Checkpoint : MonoBehaviour {
    public float minDist = 0.0f; //!<Minimum distance away from the Checkpoint that CheckpointTrigger needs to be

    void OnDrawGizmos() {
        Gizmos.DrawIcon (transform.position, "checkpointGizmo.png");
    }
}
