using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {

	[SerializeField]
	public List<Vector2> checkpoints = new List<Vector2>();
	public NavMesh mesh;
	public NavMeshPath path;
	public NavMeshAgent agent;

	public void fucks(){
		Log.M("editor", "[fucks]");
		path = new NavMeshPath();
		NavMesh.CalculatePath(transform.position, new Vector3(1,10,100), -1, path);
		Log.M("editor", ""+path);
		Vector3[] a = path.corners;
		Log.M("editor", ""+a.Length);
		foreach (Vector3 v in path.corners) {
			
			Log.M("editor","[Remaining distance "+v+"]");
		}

		/*
		agent = GetComponent<NavMeshAgent>();

		NavMesh.CalculatePath(transform.position, end, -1, path);
		//agent.CalculatePath(end, path);
		agent.SetPath(path);
		Log.M("editor","[Remaining distance "+agent.remainingDistance+"]");
		
		if (path.status == NavMeshPathStatus.PathPartial) {
			
		}*/
	}
}
