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


		if(!isLoaded)
		{
			if(CheckpointManager.instance.LoadCheckpointData())
			{
				print("CheckpointTester: Checkpoint data loaded.");
			}
			else
			{
				print("CheckpointTester: Checkpoint data did not load.");
			}

			isLoaded = true;
		}

		if(respawnTest)
		{

			print("CheckpointTester: Calling Respawn");

			CheckpointManager.instance.Respawn(transform.position);
			respawnTest = false;

		}
	}
}
