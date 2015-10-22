using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPath : MonoBehaviour {

	public List<GameObject> checkpoints;
	public int[] types = {0, 1, 2};

	// temporary
	public int PathType = 0;
	public int NofLoops = 0;

	private GameObject instance = Resources.Load("checkpoint") as GameObject;

	public void addCheckpoint(GameObject new_point)
	{
		Checkpoint c = new_point.GetComponent<Checkpoint>();
		c.path_reference = this;
		checkpoints.Add(new_point);
	}

	public void clearList(){
		foreach (GameObject g in checkpoints) {
			DestroyImmediate(g);
		}
		checkpoints.Clear();
	}

	public List<Vector3> getPoints(){
		List<Vector3> points = new List<Vector3>();

		foreach(GameObject c in checkpoints){

			if(c == null){
				checkpoints.Remove(c);
			}
			else{
				Checkpoint data = c.GetComponent<Checkpoint>();
				points.Add(data.getPosition());
			}
		}
		return points;
	}

	void OnDrawGizmosSelected()
	{
		List<Vector3> points = getPoints();
		int length = points.Count;
		Gizmos.color = Color.red;
		
		for (int i = 0; i < length; i++) {
			if(i-1 >= 0){
				Vector3 start = points[i-1];
				Vector3 end = points[i];
				Gizmos.DrawLine(start, end);
			}
		}
	}
}
