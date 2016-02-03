using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;


/*! \class CheckpointManager
 * \brief CheckpointManager tracks all enabled checkpoints
 * 
 * CheckpointManager keeps a list of all enabled checkpoints. The list is updated by each checkpoint as the checkpoint
 * is enabled or disabled. It tracks the latest checkpoint reached by the player and the latest checkpoint set by the
 * event manager (intended for use with quests). CheckpointManager can calculate the nearest checkpoint based on
 * straight line distance as well as by using pathfinding.
 */
public sealed class CheckpointManager : MonoBehaviour
{

//START OLD CODE
	//Singleton
	public static CheckpointManager instance;
	
//	public string checkpointFilePath = "/Resources/Json/checkpointData.json";
	
	//[SerializeField]
//	public NodeTree checkpointTree;
//END OLD CODE

	public static Transform LatestWorldCheckpoint;							//!The most recently reached checkpoint, set by collision with player
	public static Transform LatestQuestCheckpoint;							//!<The most recently passed quest checkpoint, set by event manager
	public static List<Transform> AllCheckpoints = new List<Transform>();	//!<A list of all active checkpoints, checkpoints update this list on awake, enabled, and disabled

//TESTING
	public string latestWorldName;
	public string latestQuestName;
	public string nearestName;
	public string nearestByPath;
//END TESTING

    void Awake()
    {
        instance = null;
    }

//START OLD CODE
	void Start(){
		//Singleton enforcement
		if(instance)
			Destroy(instance.gameObject);
		instance = this;
		
//		checkpointTree = new NodeTree();
		#if BUILD
		Debug.Log("core","Loading checkpoint data");
		LoadCheckpointData();
		#endif
		
		// If in the editor (so testing, not a build), delete all the checkpoint crap that would normally not be in a build
		#if UNITY_EDITOR
//		Checkpoint[] objs = GameObject.FindObjectsOfType<Checkpoint>();
//		foreach(Checkpoint go in objs)
//			Destroy(go.gameObject);
//		Destroy(GameObject.Find("_Checkpoints"));
		#endif
	}
//END OLD CODE

	//!Set variable LatestWorldCheckpoint
	/*! 
	 * Sets LatestWorldCheckpoint
	 * Takes one argument, the transform of the \a checkpoint
	 * Created for testing purposes.
	 * 
	 * \param checkpoint the transform of the checkpoint being set as the latest world checkpoint
	 */
	public void SetLatestWorldCheckpoint(Transform checkpoint)
	{

		LatestWorldCheckpoint = checkpoint;
		Debug.Log("Checkpoint", checkpoint.gameObject.name + " is the LatestWorldCheckpoint");
//TESTING
		latestWorldName = LatestWorldCheckpoint.name;
//END TESTING
	}

	//!Set the variable LatestQuestCheckpoint
	/*! 
	 * Sets LatestQuestCheckpoint
	 * Takes one argument, the transform of the \a checkpoint
	 * Created for testing purposes
	 * 
	 * \param checkpoint the transform of the checkpoint being set as the latest quest checkpoint
	 */
	public void SetLatestQuestCheckpoint(Transform checkpoint)
	{

		LatestQuestCheckpoint = checkpoint;
		Debug.Log("checkpoint",  checkpoint.gameObject.name + " is LatestQuestCheckpoint");

//TESTING
		latestQuestName = LatestQuestCheckpoint.name;
//END TESTING

	}//END public void SetLatestQuestCheckpoint(Transform checkpoint)

	//!Find the nearest checkpoint by straight line distance
	/*!
	 * Returns the nearest checkpoint.
	 * Takes one argument, the position of the \a player
	 * 
	 * \param player Vector3 position of the player
	 * \return NearestCheckpoint transform of the nearest checkpoint
	 * \sa NearestCheckpointByPath()
	 */
	public Transform NearestCheckpoint(Vector3 player)
	{

		Transform nearest;		//the nearest checkpoint
		float nearestDistance;	//the distance to the nearest checkpoint
		float currentDistance;	//distance to the checkpoint that is currently being tested in the for loop

		//check for empty list
		if(ListEmpty())
		{
			return null;
		}

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

//TESTING
		nearestName = nearest.name;
//END TESTING
		return nearest;
	}

	//!Find the nearest checkpoint by navmesh path
	/*!
	 * Returns the nearest checkpoint by distance of shortest path by NavMesh
	 * Takes one argument, the position of the \a player
	 * Uses code adapted from docs.unity3d.com/ScriptReference/NavMeshPath-corners.html for path length calculation
	 * May cause performance issues if the number of checkpoints is too large due to NavMesh.CalculatePath called for each checkpoint in the list
	 * 
	 * \param player Vector3 position of the player
	 * \return NearestCheckpointByPath returns the transform of the nearest checkpoint by pathfinding
	 * \sa NearestCheckpoint()
	 */
	public Transform NearestCheckpointByPath(Vector3 player)
	{

		//could add distance check to only calculate checkpoints that are near


		NavMeshPath path = new NavMeshPath();	//path used for distance calculation

		Transform nearest = AllCheckpoints[0];	//the nearest checkpoint
		float nearestDistance = -1.0f;			//the distance to the nearest checkpoint, set to negative for first array iteration
		float distanceSum = 0.0f;				//sum of the distances between corners making up the path between the player and the currently tested checkpoint

		//check for empty list
		if(ListEmpty())
		{
			return null;

		}

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
				nearestDistance = distanceSum;

				//update the nearest checkpoint to the current checkpoint element
				nearest = AllCheckpoints[currentCheckpoint];

			}//END if(nearestDistance > distanceSum || nearestDistance < 0)

		}//END for(int currentCheckpoint = 0; currentCheckpoint < AllCheckpoints.Count; currentCheckpoint++)

		//return the transform of the nearest checkpoint

//TESTING
		nearestByPath = nearest.name;
//END TESTING

		return nearest;

	}//END public Transform FindNearestCheckpointByPath(Vector3 player)


	//creates debug error and returns true if list is empty, false if list contains a checkpoint
	bool ListEmpty()
	{

		if(AllCheckpoints.Count < 1)
		{

			Debug.Error("checkpoint", "list AllCheckpoints is empty");
			return true;
		}
		else
		{
			return false;
		}

	}//END bool ListEmpty()

}//END public sealed class CheckpointManager : MonoBehaviour
