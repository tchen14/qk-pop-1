using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	This class oversees all checkpoint related function calls. It should not contain a Unity Update() function.
 *	When the player dies, it should call the respawn function. The function find the (direct) closest node in the tree.
 *	Once the closest node is found, it looks up the node's (navemesh/pathfinding) closest checkpoints.
 *	The closest accessible (area has been discovered/unlocked) checkpoint is used.
 */
public class CheckpointManager : MonoBehaviour {
	[SerializeField]
	public NodeTree checkpointTree;
	
	//! This function should be called when the player dies
	public Vector3 Respawn(Vector3 pos){
		//find the closest node
		List<Vector3> checkpoint = checkpointTree.Search(pos);
		
		//detemine which checkpoints we are accessible
		
		
		
		
		return checkpoint[0];
	}
	
	//! This uses the node tree to find the closest node and returns a list of checkpoint Vector3 positions
	private List<Vector3> FindClosestNode(Vector3 pos){
		List<Vector3> temp = new List<Vector3>();
		
		return temp;
	}

	//!Save the checkpoint data to a file
	public bool SaveCheckpointData(){
		return true;
	}

	//!Load the checkpoint data from a file
	public bool LoadCheckpointData(){
		return true;
	}
}
