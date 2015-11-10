using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Ai Path:
 * Holds the location and order of each nodes of a path. This is used by the AI.
 * This object is also used to 'draw' paths in the editor.
 * 
 */

public class AIPath : MonoBehaviour {

	public List<GameObject> checkpoints;
<<<<<<< HEAD
	public int[] types = {0, 1, 2};

	// temporary
	public int PathType = 0;
	public int NofLoops = 0;

	private GameObject instance = Resources.Load("checkpoint") as GameObject;

=======
	private GameObject instance;

	// Loads the checkpoint prefab so we can have an icon in the editor.
    void Awake()
    {
        instance = Resources.Load("checkpoint") as GameObject;
		if (instance == null) {
			Debug.LogError("Checkpoint object not found in Ressources.");
		}
    }
	
>>>>>>> 7707cf16a518310be9b91aa6944815814bebcc54
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

	// returns am ordered list of positions, this is used by the AI to navigate to each positions.
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

	// Draws the red line between each nodes to show each nodes that belong to the path and the order.
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
