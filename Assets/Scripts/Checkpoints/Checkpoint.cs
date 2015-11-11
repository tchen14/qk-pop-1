using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (SphereCollider))]
public class Checkpoint : MonoBehaviour
{
	//old code
    public float minDist = 0.0f; //!<Minimum distance away from the Checkpoint that CheckpointTrigger needs to be
	//end old code

	private static Transform _LatestWorldCheckpoint;						//the most recently reached checkpoint
	public static Transform LatestQuestCheckpoint;							//!<the most recently passed quest checkpoint, set by event manager
	public static List<Transform> AllCheckpoints = new List<Transform>();	//!<list of all active checkpoints

	public static Transform LatestWorldCheckpoint/*!<the most recently reached checkpoint (read only)*/
	{
		get
		{
			return _LatestWorldCheckpoint;
		}
	}

	void OnAwake()
	{

		//add self to list of all active checkpoints
		OnEnable();

	}

	//old code
    void OnDrawGizmos()
	{
        Gizmos.DrawIcon (transform.position, "checkpointGizmo.png");
    }
	//end old code

	void OnCollisionEnter(Collision col)
	{

//need player reference to compare to col
		if(true)
		{

			//make self the most recently reached checkpoint
			_LatestWorldCheckpoint = transform;

		}

	}

	void OnEnable()
	{

		//if this checkpoint is not on the list of active checkpoints
		if(!AllCheckpoints.Contains(transform))
		{
			//add to list
			AllCheckpoints.Add(transform);
		}
	}

	void OnDisable()
	{

		//remove self from list when checkpoint is disabled or destroyed
		AllCheckpoints.Remove(transform);

	}

	//!returns the transform of the nearest checkpoint by straight line distance
	public Transform NearestCheckpoint(Vector3 player)
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
	public Transform FindNearestCheckpointByPath(Vector3 player)
	{

		NavMeshAgent agent;
		NavMeshPath path = new NavMeshPath();

		return transform;

	}

}
