using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class Checkpoint : Placeholder
{
	public float minDist = 0.0f; //this is the minimum distance away from the Checkpoint that CheckpointTrigger needs to be
		
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "checkpointGizmo.png");
	}
}
