using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (SphereCollider))]
public class Checkpoint : MonoBehaviour
{
//START OLD CODE
    public float minDist = 0.0f; //!<Minimum distance away from the Checkpoint that CheckpointTrigger needs to be
//<<<<<<< HEAD
	public AIPath path_reference;
//=======
//END OLD CODE
//>>>>>>> checkpointTester

	void OnAwake()
	{

		//add self to list of all active checkpoints
		OnEnable();

	}//END void OnAwake()

//START OLD CODE
    void OnDrawGizmos()
	{
        Gizmos.DrawIcon (transform.position, "checkpointGizmo.png");
    }
//<<<<<<< HEAD

	public Vector3 getPosition(){
		return transform.position;
	}
//=======
//END OLD CODE

	void OnCollisionEnter(Collision col)
	{

		//compare collision collider with player collider
		if(col.collider.gameObject == QK_Character_Movement.Instance.gameObject)
		{

			//make self the most recently reached checkpoint
			CheckpointManager.LatestWorldCheckPoint = transform;
			Debug.Log("Checkpoint: " + gameObject.name + " is LatestWorldCheckpoint");

		}

	}//END void OnCollisionEnter(Collision col)

	void OnEnable()
	{

		//if this checkpoint is not on the list of active checkpoints
		if(!CheckpointManager.AllCheckpoints.Contains(transform))
		{

			//add to list
			CheckpointManager.AllCheckpoints.Add(transform);
			Debug.Log("Checkpoint: " + gameObject.name + " enabled and added to list AllCheckpoints");

		}
	}//END void OnEnable()

	void OnDisable()
	{

		//remove self from list when checkpoint is disabled or destroyed
		CheckpointManager.AllCheckpoints.Remove(transform);
		Debug.Log("Checkpoint: " + gameObject.name + " disabled and removed from list AllCheckpoints");

	}//END void OnDisable()

}
