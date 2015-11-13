using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

/*
 *	This class oversees all checkpoint related function calls. It should not contain a Unity Update() function.
 *	When the player dies, it should call the respawn function. The function find the (direct) closest node in the tree.
 *	Once the closest node is found, it looks up the node's (navemesh/pathfinding) closest checkpoints.
 *	The closest accessible (area has been discovered/unlocked) checkpoint is used.
 */
public sealed class CheckpointManager : MonoBehaviour
{

//START OLD CODE
	//Singleton
	public static CheckpointManager instance;
	
	public string checkpointFilePath = "/Resources/Json/checkpointData.json";
	
	//[SerializeField]
	public NodeTree checkpointTree;
//END OLD CODE

	public static List<Transform> AllCheckpoints = new List<Transform>();	//!<list of all active checkpoints

//START OLD CODE
	void Start(){
		//Singleton enforcement
		if(instance)
			Destroy(instance.gameObject);
		instance = this;
		
		checkpointTree = new NodeTree();
		#if BUILD
		Debug.Log("core","Loading checkpoint data");
		LoadCheckpointData();
		#endif
		
		// If in the editor (so testing, not a build), delete all the checkpoint crap that would normally not be in a build
		#if UNITY_EDITOR
		Checkpoint[] objs = GameObject.FindObjectsOfType<Checkpoint>();
//		foreach(Checkpoint go in objs)
//			Destroy(go.gameObject);
//		Destroy(GameObject.Find("_Checkpoints"));
		#endif
	}
	
	//! This function should be called when the player dies
	public Vector3 Respawn(Vector3 pos){
		#if UNITY_EDITOR
		// It's easy to forget to build the checkpoints when testing
		if(checkpointTree == null){
			Debug.Error("player","Please build checkpoint tree.");
			return Vector3.zero;
		}
		#endif
		
		//Find the closest node
		List<Vector3> checkpoint = checkpointTree.Search(pos);

		//
		//todo: detemine which checkpoints we are accessible
		//

		return checkpoint[0];
	}
/*	
	//! This uses the node tree to find the closest node and returns a list of checkpoint Vector3 positions
	private List<Vector3> FindClosestNode(Vector3 pos){
		List<Vector3> temp = new List<Vector3>();
		
		return temp;
	}

	//!Load the checkpoint data from a file
	public bool LoadCheckpointData(){
		if (checkpointTree != null)
			return checkpointTree.LoadTreeFromFile(Application.dataPath + checkpointFilePath);
		return false;
	}
*/
	//END OLD CODE


	//!returns the transform of the nearest checkpoint by straight line distance
	/*!takes one argument, the position of the player (Vector3)*/
	public Transform FindNearestCheckpoint(Vector3 player)
	{

		Transform nearest;		//the nearest checkpoint
		float nearestDistance;	//the distance to the nearest checkpoint
		float currentDistance;	//distance to the checkpoint that is currently being tested in the for loop

		//set nearest to the first element in the list
		nearest = AllCheckpoints[0];
		//set nearestDistance to the distance to the first element in the list
		nearestDistance = Vector3.Distance(AllCheckpoints[0].position, player);

		//traverse the list starting with the second element
		for(int i = 1; i < AllCheckpoints.Count; i++)
		{

			currentDistance = Vector3.Distance(AllCheckpoints[i].position, player);

			//compare distance of current element to current nearest checkpoint
			if(currentDistance < nearestDistance)
			{

				//update nearest checkpoint and distance
				nearest = AllCheckpoints[i];
				nearestDistance = currentDistance;

			}//END if(Vector3.Distance(AllCheckpoints[i].position, player) < nearestDistance)

		}//END for(int i = 1; i < AllCheckpoints.Count; i++)

		return nearest;
	}

	//!returns the nearest checkpoint by distance of shortest path by NavMesh - work in progress
	/*!takes one argument, the position of the player (Vector3)*/
	/*!uses code adapted from docs.unity3d.com/ScriptReference/NavMeshPath-corners.html for path length calculation*/
	/*!may cause performance issues if the number of checkpoints is too large due to NavMesh.CalculatePath called for each checkpoint in the list*/
	public Transform FindNearestCheckpointByPath(Vector3 player)
	{

//		NavMeshAgent agent;
		NavMeshPath path = new NavMeshPath();

		Transform nearest = AllCheckpoints[0];	//the nearest checkpoint
		float nearestDistance = -1.0f;			//the distance to the nearest checkpoint, set to negative for first array iteration
		float currentDistance = -1.0f;			//distance to the checkpoint that is currently being tested in the for loop
		float distanceSum = 0.0f;				//sum of the distances between corners making up the path between the player and the currently tested checkpoint

		//traverse the list of checkpoints
		for(int currentCheckpoint = 0; currentCheckpoint < AllCheckpoints.Count; currentCheckpoint++)
		{

			//reset the path distance
			distanceSum = 0.0f;		

			//calculate a path to the current element in the list
			NavMesh.CalculatePath(player, AllCheckpoints[currentCheckpoint].position, NavMesh.AllAreas, path);

			//check for a complete path to the current checkpoint before calculating path distance
			if(path.status == NavMeshPathStatus.PathComplete)
			{

				//check to see if currently at a checkpoint
				if(path.corners.Length < 2)
				{

					//reset the total dist
					distanceSum = 0f;

				}
				//otherwise calculate path distance
				else
				{

					Vector3 prevCorner = path.corners[0];	//corner to measure from on each iteration over corners array

					//traverse the array of path corners
					for(int cornerNumber = 1; cornerNumber < path.corners.Length; cornerNumber++)
					{

						//get the new current corner
						Vector3 currentCorner = path.corners[cornerNumber];

						//add the distance to the new corner to the total distance
						distanceSum += Vector3.Distance(prevCorner, currentCorner);

						//update the previous corner
						prevCorner = currentCorner;

					}//END for(int cornerNumber = 1; cornerNumber < path.corners.Length; cornerNumber++)

				}//END if(path.corners.Length < 2) else

			}// if(path.status == NavMeshPathStatus.PathComplete)

			//compare path lengths for shorter distance or first iteration through array
			if(nearestDistance > distanceSum || nearestDistance < 0)
			{

				//set the nearest distance to the distance of the current path
				currentDistance = distanceSum;

				//update the nearest checkpoint to the current checkpoint element
				nearest = AllCheckpoints[currentCheckpoint];

			}//END if(nearestDistance > distanceSum || nearestDistance < 0)

		}//END for(int currentCheckpoint = 0; currentCheckpoint < AllCheckpoints.Count; currentCheckpoint++)

		//return the transform of the nearest checkpoint
		return nearest;

	}//END public Transform FindNearestCheckpointByPath(Vector3 player)

}//END public sealed class CheckpointManager : MonoBehaviour
