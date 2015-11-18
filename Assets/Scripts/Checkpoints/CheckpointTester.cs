using UnityEngine;
using System.Collections;

public class CheckpointTester : MonoBehaviour {


	public bool respawnTest;
	bool isLoaded = false;

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
			point = CheckpointManager.instance.FindNearestCheckpoint(transform.position);
			Debug.Log("CheckpointTester: the nearest checkpoint is " + point.name);
		}

		if(Input.GetKeyDown("n"))
		{
			Transform point;
			point = CheckpointManager.instance.FindNearestCheckpointByPath(transform.position);
			Debug.Log("CheckpointTester: the nearest checkpoint by path is " + point.name);
		}

		if(Input.GetKeyDown("m"))
		{
			Transform point;
			point = CheckpointManager.LatestWorldCheckPoint;
			Debug.Log("CheckpointTester: LatestWorldCheckPoint is " + point.name);
		}

	}

}
