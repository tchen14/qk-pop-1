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
	
	//!Number of nodes held at the bottom level
	public int totalNodes = 0;

	//! COnstructor with blank data
	public NodeTree()
	{
		m_root = new Node(new Vector3(0, 0, 0), null, null, null, null, null);
	}

	//! Constructor using dictionary
	public NodeTree(Dictionary<Vector3,List<Vector3>> nodes)
	{
		totalNodes = nodes.Count;
		
		float x = 0, z = 0;
		foreach(Vector3 v in nodes.Keys){
			x += v.x;
			z += v.z;
		}
		m_root = new Node(new Vector3(x / nodes.Count, 0, z / nodes.Count), null, null, null, null, null);
		
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
		public List<Vector3> checkpoints;
		
		//node constructor
		public Node(){
		
		}
		
		public Node(Vector3 n_location, Node n_child_1, Node n_child_2, Node n_child_3, Node n_child_4, List<Vector3> n_checkpoints)
		{
			location = n_location;
			child_1 = n_child_1;
			child_2 = n_child_2;
			child_3 = n_child_3;
			child_4 = n_child_4;
			checkpoints = n_checkpoints;
		}
		
		
		//! Determine if the Node is empty by determining if the checkpoints list exists
		public bool isEmpty()
		{
			return (checkpoints == null) ? true : false;
		}
	};
	
	//!Initial call should be made to this. Requires list of all nodes you want to put into the tree.
	private void RecursiveInsert(Dictionary<Vector3,List<Vector3>> nodes)
	{
		// passing in node list, average x location, average z location, and root of tree
		RecursiveInsert(nodes, m_root.location.x, m_root.location.z, m_root);
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
			float x, z;
			
			// push each node into a specific list depending on nodes location related to average location of all nodes
			foreach (Vector3 node in nodes.Keys) {
				if (node.x >= xPos && node.z >= zPos) {
					quadrant_1.Add(node, nodes [node]);
				} else if (node.x < xPos && node.z >= zPos) {
					quadrant_2.Add(node, nodes [node]);
				} else if (node.x < xPos && node.z < zPos) {
					quadrant_3.Add(node, nodes [node]);
				} else {
					quadrant_4.Add(node, nodes [node]);
				}
			}
			// All four segements of the interal code (quadrant_1, quadrant_2, etc.) are needed as C# pointers cannot be used in a safe-only environment, Unity enforced.

			// If list has more than one node, add empty node to tree, then continue splitting nodes
			#region firstChild
			if (quadrant_1.Count > 1) {
				x = 0;
				z = 0;
				// calculate average of this cartesian quadrant; greater greater
				foreach (Vector3 q1 in quadrant_1.Keys) {
					x += q1.x;
					z += q1.z;
				}
				// Add empty node to tree
				parent.child_1 = new Node(new Vector3(x / quadrant_1.Count, 0, z / quadrant_1.Count), null, null, null, null, null);

				// Recurse by passing in this quadrant's node list, this quadrant's average x location, this quadrant's average z location, firstChild because this is first quadrant
				RecursiveInsert(quadrant_1, x / quadrant_1.Count, z / quadrant_1.Count, parent.child_1);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_1.Count == 1) {
				foreach (Vector3 q1 in quadrant_1.Keys) {
					parent.child_1 = new Node(q1, null, null, null, null, quadrant_1 [q1]);
				}
			}
			#endregion
			
			// If list has more than one node, add empty node to tree, then continue splitting nodes
			#region secondChild
			if (quadrant_2.Count > 1) {
				x = 0;
				z = 0;
				// calculate average of this cartesian quadrant; greater less
				foreach (Vector3 q2 in quadrant_2.Keys) {
					x += q2.x;
					z += q2.z;
				}
				// Add empty node to tree
				parent.child_2 = new Node(new Vector3(x / quadrant_2.Count, 0, z / quadrant_2.Count), null, null, null, null, null);

				// Recurse by passing in this quadrant's nodes list, this quadrant's average x location, this quadrant's average z location, secondChild because this is second quadrant
				RecursiveInsert(quadrant_2, x / quadrant_2.Count, z / quadrant_2.Count, parent.child_2);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_2.Count == 1) {
				foreach (Vector3 q2 in quadrant_2.Keys) {
					parent.child_2 = new Node(q2, null, null, null, null, quadrant_2 [q2]);
				}
			}
			#endregion
			
			// If list has more than one node, add empty node to tree, then continue splitting nodes
			#region thirdChild
			if (quadrant_3.Count > 1) {
				x = 0;
				z = 0;
				// calculate average of this cartesian quadrant; less greater
				foreach (Vector3 q3 in quadrant_3.Keys) {
					x += q3.x;
					z += q3.z;		
				}
				// Add empty node to tree
				parent.child_3 = new Node(new Vector3(x / quadrant_3.Count, 0, z / quadrant_3.Count), null, null, null, null, null);

				// Recurse by passing in this quadrant's node list, this quadrant's average x location, this quadrant's average z location, secondChild because this is third quadrant
				RecursiveInsert(quadrant_3, x / quadrant_3.Count, z / quadrant_3.Count, parent.child_3);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_3.Count == 1) {
				foreach (Vector3 q3 in quadrant_3.Keys) {
					parent.child_3 = new Node(q3, null, null, null, null, quadrant_3 [q3]);
				}
			}
			#endregion
			
			// If list has more than one node, add empty node to tree, then continue splitting node
			#region fourthChild
			if (quadrant_4.Count > 1) {
				x = 0;
				z = 0;
				// calculate average of this cartesian quadrant; less less
				foreach (Vector3 q4 in quadrant_4.Keys) {
					x += q4.x;
					z += q4.z;
				}
				// Add empty node onto tree
				parent.child_4 = new Node(new Vector3(x / quadrant_4.Count, 0, z / quadrant_4.Count), null, null, null, null, null);

				// Recurse by passing in this quadrant's node list, this quadrant's average x location, this quadrant's average z location, secondChild because this is fourth quadrant
				RecursiveInsert(quadrant_4, x / quadrant_4.Count, z / quadrant_4.Count, parent.child_4);
			}
			// If list only has one node, push node onto tree
			else if (quadrant_4.Count == 1) {
				foreach (Vector3 q4 in quadrant_4.Keys) {
					parent.child_4 = new Node(q4, null, null, null, null, quadrant_4 [q4]);
				}
			}
			#endregion
		}
	}
	
	//! Search tree for closest node to the Vector3
	public List<Vector3> Search(Vector3 loc)
	{
		if (m_root != null) {
			;
			return Search(m_root, loc, null);
		} else {
			return null;
		}
	}

	//! This should not be called directly. Call Node Search(Vector3 loc) instead.
	private List<Vector3> Search(Node n, Vector3 loc, List<Vector3> nodeList)
	{
		//If we hit the bottom line of nodes
		if (n != null && !n.isEmpty()) {
			return n.checkpoints;
		}
		if (n.child_1 != null && loc.x >= n.location.x && loc.z >= n.location.z) {
			return Search(n.child_1, loc, nodeList);
		} else if (n.child_2 != null && loc.x < n.location.x && loc.z >= n.location.z) {
			return Search(n.child_2, loc, nodeList);
		} else if (n.child_3 != null && loc.x < n.location.x && loc.z < n.location.z) {
			return Search(n.child_3, loc, nodeList);
		} else if (n.child_4 != null && loc.x >= n.location.x && loc.z < n.location.z) {
			return Search(n.child_4, loc, nodeList);
		} else {
			return null;
		}
	}
	
	/*!
	 *	This will print the NodeTree as a .json string. The string can then be saved and used later.
	 *	Empty nodes will have a location and 1-4 children
	 *	End nodes will have a location and an array op checkpoints
	 */
	public bool SaveTreeAsJson(string path)
	{
		System.IO.File.Delete(path);
		System.IO.File.WriteAllText(path, GetTreeAsJson().ToString());
		return System.IO.File.Exists(path);
	}
	
	public string GetTreeAsJson()
	{
		//Main json body
		JSONClass json = new JSONClass();
		
		//Adding totalNodes variable
		json.Add("totalNodes",new JSONData(totalNodes));
				
		if (m_root.child_1 != null && m_root.child_2 != null && m_root.child_3 != null && m_root.child_4 != null) {
			//Add each of the nodes below m_root
			json.Add("node",GetNodeDataAsJson(m_root));
		} else {
			Log.E("checkpoint", "NodeTree empty. Please (Re)Build Checkpoints.");
		}
		
		//Log.M("checkpoint", "Checkpoint string in .json format:\n"+s);
		return json.ToString();
	}
	
	private JSONClass GetNodeDataAsJson(Node n)
	{
		JSONClass json = new JSONClass();
		
		json.Add("loc",new JSONData(n.location.ToString()));
		if (n.isEmpty()) {
			if (n.child_1 != null) {
				json.Add("c1",GetNodeDataAsJson(n.child_1));
			}
			if (n.child_2 != null) {
				json.Add("c2",GetNodeDataAsJson(n.child_2));
			}
			if (n.child_3 != null) {
				json.Add("c3",GetNodeDataAsJson(n.child_3));
			}
			if (n.child_4 != null) {
				json.Add("c4",GetNodeDataAsJson(n.child_4));
			}
		} else if (!n.isEmpty()) {
			JSONArray checkpoints = new JSONArray();
			if (n.checkpoints != null) {
				foreach (Vector3 v in n.checkpoints) {
					checkpoints.Add(new JSONData(v.ToString()));
				}
			}
			json.Add("cp",checkpoints);
		}
		return json;
	}
	
	public bool LoadTreeFromFile(string path)
	{
		if (!System.IO.File.Exists(path)) {
			Log.E("checkpoint", "File does not exist: " + path);
			return false;
		}
		
		string json = System.IO.File.ReadAllText(path);
		return LoadTreeFromJson(json);
	}
	
	public bool LoadTreeFromJson(string json)
	{
		JSONNode checkpointData = JSON.Parse(json);
		
		totalNodes = checkpointData["totalNodes"].AsInt;
		InsertNodeFromJson(m_root, checkpointData["node"]);
		
		return totalNodes == 0 ? false : true;
	}
	
	//! Returns node built from JSONNode data
	private Node InsertNodeFromJson(Node n, JSONNode nodeData){
		n.location = nodeData["loc"].AsVector3;
		
		if(nodeData["cp"] == null){
			if(nodeData["c1"] != null){
				n.child_1 = InsertNodeFromJson(new Node(), (JSONNode)nodeData["c1"]);
			}
			if(nodeData["c2"] != null){
				n.child_2 = InsertNodeFromJson(new Node(), (JSONNode)nodeData["c2"]);
			}
			if(nodeData["c3"] != null){
				n.child_3 = InsertNodeFromJson(new Node(), (JSONNode)nodeData["c3"]);
			}
			if(nodeData["c4"] != null){
				n.child_4 = InsertNodeFromJson(new Node(), (JSONNode)nodeData["c4"]);
			}
		}else{
			List<Vector3> checkpoints = new List<Vector3>();
			for(int i = 0; i < nodeData["cp"].AsArray.Count; i++){
				checkpoints.Add(nodeData["cp"].AsArray[i].AsVector3);
			}
			
			n.checkpoints = checkpoints;
		}
		
		return n;
	}
}