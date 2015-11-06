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
public sealed class CheckpointManager : MonoBehaviour {
	//Singleton
	public static CheckpointManager instance;
	
	public string checkpointFilePath = "/Resources/Json/checkpointData.json";
	
	//[SerializeField]
	public NodeTree checkpointTree;
	
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
}
