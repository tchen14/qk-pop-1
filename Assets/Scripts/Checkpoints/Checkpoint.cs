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


	public static Transform LatestWorldCheckpoint/*!<the most recently reached checkpoint (read only)*/
	{

		get
		{
			return _LatestWorldCheckpoint;
		}

	}//END public static Transform LatestWorldCheckpoint

	void OnAwake()
	{

		//add self to list of all active checkpoints
		OnEnable();

	}//END void OnAwake()

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

	}//END void OnCollisionEnter(Collision col)

	void OnEnable()
	{

		//if this checkpoint is not on the list of active checkpoints
		if(!CheckpointManager.AllCheckpoints.Contains(transform))
		{
			//add to list
			CheckpointManager.AllCheckpoints.Add(transform);
		}
	}//END void OnEnable()

	void OnDisable()
	{

		//remove self from list when checkpoint is disabled or destroyed
		CheckpointManager.AllCheckpoints.Remove(transform);

	}//END void OnDisable()

	//!sets LatestQuestCheckpoint
	/*!takes one argument, the transform of the checkpoint (Transform)*/
	public void SetLatestQuestCheckpoint(Transform checkpoint)
	{

		LatestQuestCheckpoint = checkpoint;

	}//END public void SetLatestQuestCheckpoint(Transform checkpoint)

}
