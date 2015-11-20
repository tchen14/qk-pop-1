using UnityEngine;
using System.Collections;

/*! \class CheckpointTester
 * \brief A tester class for testing Checkpoint.cs and CheckpointManager.cs
 *
 * This class may change drastically. Do not include in final build.
 * Currently uses GetKeyDown() to test checkpoint assignments to variables and list
 * 
 * \sa Checkpoint.cs, CheckpointManager.cs
 */
public class CheckpointTester : MonoBehaviour {


	public bool respawnTest;


	public string nearestCheckpoint;
	public string nearestCheckpointByPath;
	public string latestWorldCheckpoint;
	public string latestQuestCheckpoint;

	// Use this for initialization
	void Start()
	{

		respawnTest = false;
		
	}
	
	// Update is called once per frame
	void Update()
	{

		if(Input.GetKeyDown("b"))
		{
			Transform point;
			point = CheckpointManager.instance.NearestCheckpoint(transform.position);
			nearestCheckpoint = point.gameObject.name;
			Debug.Log("CheckpointTester: the nearest checkpoint is " + nearestCheckpoint + " at " + point.position.ToString());
		}

		if(Input.GetKeyDown("n"))
		{
			Transform point;
			point = CheckpointManager.instance.NearestCheckpointByPath(transform.position);
			nearestCheckpointByPath = point.gameObject.name;
			Debug.Log("CheckpointTester: the nearest checkpoint by path is " + nearestCheckpointByPath + " at " + point.position.ToString());
		}

		if(Input.GetKeyDown("m"))
		{
			Transform point;
			point = CheckpointManager.LatestWorldCheckpoint;
			latestWorldCheckpoint = point.gameObject.name;
			Debug.Log("CheckpointTester: LatestWorldCheckPoint is " + latestWorldCheckpoint + " at " + point.position.ToString());
		}

		if(Input.GetKeyDown(","))
		{
			CheckpointManager.instance.SetLatestQuestCheckpoint(CheckpointManager.AllCheckpoints[0]);
			Transform point;
			point = CheckpointManager.LatestQuestCheckpoint;
			latestWorldCheckpoint = point.gameObject.name;
			Debug.Log("CheckpointTester: LatestQuestCheckPoint is " + latestQuestCheckpoint + " at " + point.position.ToString());
		}

	}

}
