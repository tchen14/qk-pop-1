using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/*
 *	NodeTree is the data structure for CheckpointManager.
 *	Note that this is a tree of nodes, where the bottom-most nodes hold a List of checkpoints.
 *	There is expected to be X number of nodes, where X is: mapWidth * mapLength * 1 / characterWidth. This can easily equate to over 10000 nodes
 */
public class NodeTree
{
	private Node m_root;
	
	//Global variables used to calculate averages of entire list of nodes,then of each cartesian quadrant's nodes
	float x, z;
	int numNodes;

	//! Constructor \todo implementation + code
	public NodeTree(Dictionary<Vector3,List<Vector3>> nodes){
		numNodes = nodes.Count;
		m_root = new Node(new Vector3(x / numNodes, 0, z / numNodes), null, null, null, null, true, null);

		// Send list to recursive insert function
		RecursiveInsert(nodes);
	}

	/*
	 *	Individual nodes for NodeTree
	 */
	public class Node
	{
		public Vector3 location;	/*!<The location of the node */
		public Node child_1;
		public Node child_2;
		public Node child_3;
		public Node child_4;
		public bool isEmpty;	/*!<Indicates whether node is an empty Node because not able to set a vector to null */
		public List<Vector3> checkpoints;
		
		//node constructor
		public Node(Vector3 n_location, Node n_child_1, Node n_child_2, Node n_child_3, Node n_child_4,
		            bool n_isEmpty, List<Vector3> n_checkpoints)
		{
			location = n_location;
			child_1 = n_child_1;
			child_2 = n_child_2;
			child_3 = n_child_3;
			child_4 = n_child_4;
			isEmpty = n_isEmpty;
			checkpoints = n_checkpoints;
		}
	};
	
	//!Initial call should be made to this. Requires list of all nodes you want to put into the tree.
	private void RecursiveInsert(Dictionary<Vector3,List<Vector3>> nodes)
	{
		x = z = 0;
		// Calculate totals of x position, z position, and number of nodes
		foreach (Vector3 temp in nodes.Keys) {
			x += temp.x;
			z += temp.z;
		}

		// passing in node list, average x location, average z location, and root of tree
		RecursiveInsert(nodes, x / numNodes, z / numNodes, m_root);
	}

	/*!
	 *	This should not be called directly. Call void recursiveInsert(List<GameObject>nodes) instead.
	 */
	private void RecursiveInsert(Dictionary<Vector3,List<Vector3>>nodes, float xPos, float zPos, Node parent)
	{
		// create four temp lists to match each cartesian quadrant		
		Dictionary<Vector3,List<Vector3>> quadrant_1 = new Dictionary<Vector3,List<Vector3>>();
		Dictionary<Vector3,List<Vector3>> quadrant_2 = new Dictionary<Vector3,List<Vector3>>();
		Dictionary<Vector3,List<Vector3>> quadrant_3 = new Dictionary<Vector3,List<Vector3>>();
		Dictionary<Vector3,List<Vector3>> quadrant_4 = new Dictionary<Vector3,List<Vector3>>();
		
		// make sure list passed isn't empty
		if (nodes.Count > 0) {
			// push each node into a specific list depending on nodes location related to average location of all nodes
			
			foreach (Vector3 node in nodes.Keys) {
				if (node.x >= xPos && node.z >= zPos) {
					quadrant_1.Add(node,nodes[node]);
				} else if (node.x < xPos && node.z >= zPos) {
					quadrant_2.Add(node,nodes[node]);
				} else if (node.x < xPos && node.z < zPos) {
					quadrant_3.Add(node,nodes[node]);
				} else {
					quadrant_4.Add(node,nodes[node]);
				}
			}
			// All four segements of the interal code (quadrant_1, quadrant_2, etc.) are needed as C# pointers cannot be used in a safe-only environment, Unity enforced.

			// If list has more than one node, add empty node to tree, then continue splitting nodes
			//Log.M("checkpoint", "Q1 " + quadrant_1.Count);
			#region firstChild
			if (quadrant_1.Count > 1) {
				x = 0;
				z = 0;
				numNodes = 0;
				// calculate average of this cartesian quadrant; greater greater
				foreach (Vector3 q1 in quadrant_1.Keys) {
					x += q1.x;
					z += q1.z;
					numNodes++;
				}
				// Add empty node to tree
				parent.child_1 = new Node(new Vector3(x / numNodes, 0, z / numNodes), null, null, null, null, true, null);

				// Recurse by passing in this quadrant's node list, this quadrant's average x location, this quadrant's average z location, firstChild because this is first quadrant
				RecursiveInsert(quadrant_1, x / numNodes, z / numNodes, parent.child_1);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_1.Count == 1) {
				foreach (Vector3 q1 in quadrant_1.Keys) {
					parent.child_1 = new Node(q1, null, null, null, null, false, quadrant_1[q1]);
				}
			}
			#endregion
			
			// If list has more than one node, add empty node to tree, then continue splitting nodes
			//Log.M("checkpoint", "Q2 " + quadrant_2.Count);
			#region secondChild
			if (quadrant_2.Count > 1) {
				x = 0;
				z = 0;
				numNodes = 0;
				// calculate average of this cartesian quadrant; greater less
				foreach (Vector3 q2 in quadrant_2.Keys) {
					x += q2.x;
					z += q2.z;
					numNodes++;
				}
				// Add empty node to tree
				parent.child_2 = new Node(new Vector3(x / numNodes, 0, z / numNodes), null, null, null, null, true, null);

				// Recurse by passing in this quadrant's nodes list, this quadrant's average x location, this quadrant's average z location, secondChild because this is second quadrant
				RecursiveInsert(quadrant_2, x / numNodes, z / numNodes, parent.child_2);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_2.Count == 1) {
				foreach (Vector3 q2 in quadrant_2.Keys) {
					parent.child_2 = new Node(q2, null, null, null, null, false, quadrant_2[q2]);
				}
			}
			#endregion
			
			// If list has more than one node, add empty node to tree, then continue splitting nodes
			//Log.M("checkpoint", "Q3 " + quadrant_3.Count);
			#region thirdChild
			if (quadrant_3.Count > 1) {
				x = 0;
				z = 0;
				numNodes = 0;
				// calculate average of this cartesian quadrant; less greater
				foreach (Vector3 q3 in quadrant_3.Keys) {
					x += q3.x;
					z += q3.z;		
					numNodes++;
				}
				// Add empty node to tree
				parent.child_3 = new Node(new Vector3(x / numNodes, 0, z / numNodes), null, null, null, null, true, null);

				// Recurse by passing in this quadrant's node list, this quadrant's average x location, this quadrant's average z location, secondChild because this is third quadrant
				RecursiveInsert(quadrant_3, x / numNodes, z / numNodes, parent.child_3);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_3.Count == 1) {
				foreach (Vector3 q3 in quadrant_3.Keys) {
					parent.child_3 = new Node(q3, null, null, null, null, false, quadrant_3[q3]);
				}
			}
			#endregion
			
			// If list has more than one node, add empty node to tree, then continue splitting node
			//Log.M("checkpoint", "Q4 " + quadrant_4.Count);
			#region fourthChild
			if (quadrant_4.Count > 1) {
				x = 0;
				z = 0;
				numNodes = 0;
				// calculate average of this cartesian quadrant; less less
				foreach (Vector3 q4 in quadrant_4.Keys) {
					x += q4.x;
					z += q4.z;
					numNodes++;
				}
				// Add empty node onto tree
				parent.child_4 = new Node(new Vector3(x / numNodes, 0, z / numNodes), null, null, null, null, true, null);

				// Recurse by passing in this quadrant's node list, this quadrant's average x location, this quadrant's average z location, secondChild because this is fourth quadrant
				RecursiveInsert(quadrant_4, x / numNodes, z / numNodes, parent.child_4);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_4.Count == 1) {
				foreach (Vector3 q4 in quadrant_4.Keys) {
					parent.child_4 = new Node(q4, null, null, null, null, false, quadrant_4[q4]);
				}
			}
			#endregion
		}
	}
	
	//! Search tree for closest node to the Vector3
	public List<Vector3> Search(Vector3 loc)
	{
		if (m_root != null) {;
			return Search(m_root, loc, null);
		} else {
			return null;
		}
	}

	//! This should not be called directly. Call Node Search(Vector3 loc) instead.
	private List<Vector3> Search(Node n, Vector3 loc, List<Vector3> nodeList)
	{
		//If we hit the bottom line of nodes
		if (n != null && n.isEmpty == false) {
			return n.checkpoints;
		}
		if (n.child_1 != null && loc.x >= n.location.x && loc.z >= n.location.z) {
			return Search (n.child_1, loc, nodeList);
		} else if (n.child_2 != null && loc.x < n.location.x && loc.z >= n.location.z) {
			return Search (n.child_2, loc, nodeList);
		} else if (n.child_3 != null && loc.x < n.location.x && loc.z < n.location.z) {
			return Search (n.child_3, loc, nodeList);
		} else if (n.child_4 != null && loc.x >= n.location.x && loc.z < n.location.z) {
			return Search (n.child_4, loc, nodeList);
		} else {
			return null;
		}
	}
	
	public string SaveTreeAsJson(){
		string s = "";
		//Opening json bracket and enter numNodes
		s+="{\"numNodes\":"+ numNodes +",";
		
		if(m_root.child_1 != null && m_root.child_2 != null && m_root.child_3 != null && m_root.child_4 != null){
			s+= GetNodeData(m_root);
		}else{
			Log.E("checkpoint","NodeTree empty. Please (Re)Build Checkpoints.");
		}
		
		//Close out json bracket
		s+="}";
		return s;
	}
	
	private string GetNodeData(Node n){
		string s = "";
		bool comma = false;	//This is used to tell the next segment to make a comma (if there was a previous segment)
		//While node has children
		Debug.Log("cunt");
		s+="\"node\":{";
		comma = false;
		
		while(n.isEmpty == true){
			s+="\"location\":\"null\",";
			if(n.child_1 != null){
				s+=GetNodeData(n.child_1);
				comma = true;
			}
			if(n.child_2 != null){
				if(comma == true){
					s+=",";
					comma = false;
				}
				s+=GetNodeData(n.child_2);
				
				comma = true;
			}
			if(n.child_3 != null){
				if(comma == true){
					s+=",";
					comma = false;
				}
				s+=GetNodeData(n.child_3);
				
				comma = true;
			}
			if(n.child_4 != null){
				if(comma == true){
					s+=",";
					comma = false;
				}
				s+=GetNodeData(n.child_4);
				
				comma = true;
			}
			s+="\"isEmpty\":"+n.isEmpty.ToString();
		}
		while(n.isEmpty == false){
			if(n.child_1 != null){
				s+=CreateNodeBaseStr(1, n.child_1);
				comma = true;
			}
			if(n.child_2 != null){
				if(comma == true){
					s+=",";
					comma = false;
				}
				s+=CreateNodeBaseStr(2, n.child_2);
				
				comma = true;
			}
			if(n.child_3 != null){
				if(comma == true){
					s+=",";
					comma = false;
				}
				s+=CreateNodeBaseStr(3, n.child_3);
				
				comma = true;
			}
			if(n.child_4 != null){
				if(comma == true){
					s+=",";
					comma = false;
				}
				s+=CreateNodeBaseStr(4, n.child_4);
				
				comma = true;
			}
		}
		s+="}";
		return s;
	}
	
	private string CreateNodeBaseStr(int i, Node n){
		string s = "\"child_"+i+"\":{";
		s+="\"location\":\""+n.location.ToString()+"\",";
		s+="\"isEmpty\":"+n.isEmpty.ToString()+",";
		s+="\"checkpoints\":[";
		bool once = true;
		foreach(Vector3 v in n.checkpoints){
			if(once){
				s+="\""+v.ToString()+"\"";
			}else{
				s+=",\""+v.ToString()+"\"";
			}
		}
		s+="]";
		
		s+= "}";
		return s;
	}
	
	public void LoadStringFromJson(){
		string text = System.IO.File.ReadAllText (Application.dataPath + "/Resources/Json/checkpointData.json");
		var N = JSON.Parse (text);
//		for (int i = 0; i < N.Count; i++)
//			logStrings.Add (N [i].Value);
			
		m_root = new Node(new Vector3(x / numNodes, 0, z / numNodes), null, null, null, null, true, null);
	}
}