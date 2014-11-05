using UnityEngine;
using System.Collections.Generic;

public class CheckPointTree : MonoBehaviour {
	
	private Node m_root;
	private List<GameObject> checkpoints = new List<GameObject>();
	
	// global variables used to calculate averages of entire list of checkpoints, then of each cartesian quadrant's checkpoints
	float x;
	float z;
	int numCheckpoints;
	
	
	public class Node{
		private Vector3 _location;
		private Node _parent;
		private Node _firstChild;
		private Node _secondChild;
		private Node _thirdChild;
		private Node _fourthChild;
		// indicates whether checkpoint is an empty Node because not able to set a vector to null
		private bool _NULL;
		// node constructor
		public Node(Vector3 location, Node parent, Node first, Node second, Node third, Node fourth, bool NULL)
		{
			_parent = parent;
			_firstChild = first;
			_secondChild = second;
			_thirdChild = third;
			_fourthChild = fourth;
			_location = location;
			_NULL = NULL;
		}
		// getters and setters for private data
		public Node parent
		{
			get { return _parent; }
			set { _parent = value; }
		}
		public Node firstChild
		{
			get { return _firstChild; }
			set { _firstChild = value; }
		}
		public Node secondChild
		{
			get { return _secondChild; }
			set { _secondChild = value; }
		}
		public Node thirdChild
		{
			get { return _thirdChild; }
			set { _thirdChild = value; }
		}
		public Node fourthChild
		{
			get { return _fourthChild; }
			set { _fourthChild = value; }
		}
		public Vector3 location
		{
			get { return _location; }
		}	
		public bool NULL
		{
			get { return _NULL; }
		}
	};
	
	// Use this for initialization
	void Start () 
	{
		initializeCheckpoints();
		// print closest node using search function
		Log.M ("checkpoint", "Closest Node " + search (new Vector3(5,5,5)).location);
		print("Closest Node " + search (new Vector3(5,5,5)).location);

	}
	void initializeCheckpoints()
	{
		// initialize variables
		x = 0;
		z = 0;
		numCheckpoints = 0;
		// using array because GameObject.FindGameObjectsWithTag only returns an array
		GameObject[] temps;
		// loop through all Respawn Objects in Scene and set it to array
		temps = GameObject.FindGameObjectsWithTag("Respawn");
		// calculate totals of x position, z position, and number of checkpoints
		foreach(GameObject temp in temps)
		{
			x += temp.transform.position.x;
			z += temp.transform.position.z;
			numCheckpoints++;
		}
		// add each checkpoint from array to list
		for(int i =0; i < numCheckpoints; i++)
		{
			checkpoints.Add(temps[i]);
		}
		Log.M ("checkpoint", "Number of checkpoints in list " + checkpoints.Count);
		// set m_root to empty node; a node with no location value indicated by boolean
		m_root = new Node(Vector3.zero,null,null,null,null,null,true);
		// send list to recursive insert function
		recursiveInsertHelper(checkpoints);
	}
	
	
	// helper function
	void recursiveInsertHelper(List<GameObject>checkpoints)
	{
		// passing in checkpoint list, average x location, average z location, and root of tree
		recursiveInsert(checkpoints,x/numCheckpoints,z/numCheckpoints,m_root);
	}
	
	void recursiveInsert(List<GameObject>checkpoints, float xPos, float zPos, Node parent)
	{
		// create four temp lists to match each cartesian quadrant
		List<GameObject> greaterGreater = new List<GameObject>();
		List<GameObject> greaterLess = new List<GameObject>();
		List<GameObject> lessGreater = new List<GameObject>();
		List<GameObject> lessLess = new List<GameObject>();
		
		// make sure list passed isn't empty
		if(checkpoints.Count > 0)
		{
			// push each checkpoint into a specific list depending on checkpoints location related to average location of all checkpoints
			foreach(GameObject checkpoint in checkpoints)
			{
				if(checkpoint.transform.position.x > xPos && checkpoint.transform.position.z > zPos)
				{
					greaterGreater.Add(checkpoint);
				}
				else if(checkpoint.transform.position.x > xPos && checkpoint.transform.position.z < zPos)
				{
					greaterLess.Add(checkpoint);
				}
				else if(checkpoint.transform.position.x < xPos && checkpoint.transform.position.z > zPos)
				{
					lessGreater.Add(checkpoint);
				}
				else
				{
					lessLess.Add(checkpoint);
				}
			}
			// if list has more than one checkpoint, add empty node to tree, then continue splitting checkpoints
			#region firstChild
			if(greaterGreater.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				// calculate average of this cartesian quadrant; greater greater
				foreach(GameObject gG in greaterGreater)
				{
					x += gG.transform.position.x;
					z += gG.transform.position.z;
					numCheckpoints++;
				}
				// add empty node to tree
				parent.firstChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
				Log.M ("checkpoint", "gG " + greaterGreater.Count);
				// recurse by passing in this quadrant's checkpoint list, this quadrant's average x location, this quadrant's average z location, firstChild because this is first quadrant
				recursiveInsert(greaterGreater, x/numCheckpoints, z/numCheckpoints,parent.firstChild);
			}
			// if list only has one checkpoint, push checkpoint onto tree
			else if (greaterGreater.Count == 1)
			{
				foreach(GameObject gG in greaterGreater)
				{
					parent.firstChild = new Node(gG.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
			
			// if list has more than one checkpoint, add empty node to tree, then continue splitting checkpoints
			#region secondChild
			if(greaterLess.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				// calculate average of this cartesian quadrant; greater less
				foreach(GameObject gL in greaterLess)
				{
					x += gL.transform.position.x;
					z += gL.transform.position.z;
					numCheckpoints++;
				}
				// add empty node to tree
				parent.secondChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
				Log.M ("checkpoint", "gL" + greaterLess.Count);
				// recurse by passing in this quadrant's checkpoint list, this quadrant's average x location, this quadrant's average z location, secondChild because this is second quadrant
				recursiveInsert(greaterLess, x/numCheckpoints, z/numCheckpoints,parent.secondChild);
			}
			// if list only has one checkpoint, push checkpoint onto tree
			else if (greaterLess.Count == 1)
			{
				foreach(GameObject gL in greaterLess)
				{
					parent.secondChild = new Node(gL.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
			
			// if list has more than one checkpoint, add empty node to tree, then continue splitting checkpoints
			#region thirdChild
			if(lessGreater.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				// calculate average of this cartesian quadrant; less greater
				foreach(GameObject lG in lessGreater)
				{
					x += lG.transform.position.x;
					z += lG.transform.position.z;		
					numCheckpoints++;
				}
				// add empty node to tree
				parent.thirdChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
				Log.M ("checkpoint", "lG" + lessGreater.Count);
				// recurse by passing in this quadrant's checkpoint list, this quadrant's average x location, this quadrant's average z location, secondChild because this is third quadrant
				recursiveInsert(lessGreater, x/numCheckpoints, z/numCheckpoints, parent.thirdChild);
			}
			// if list only has one checkpoint, push checkpoint onto tree
			else if (lessGreater.Count == 1)
			{
				foreach(GameObject lG in lessGreater)
				{
					parent.thirdChild = new Node(lG.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
			
			// if list has more than one checkpoint, add empty node to tree, then continue splitting checkpoints
			#region fourthChild
			if(lessLess.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				// calculate average of this cartesian quadrant; less less
				foreach(GameObject lL in lessLess)
				{
					x += lL.transform.position.x;
					z += lL.transform.position.z;
					numCheckpoints++;
				}
				// add empty node onto tree
				parent.fourthChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
				Log.M ("checkpoint", "lL" + lessLess.Count);
				// recurse by passing in this quadrant's checkpoint list, this quadrant's average x location, this quadrant's average z location, secondChild because this is fourth quadrant
				recursiveInsert(lessLess, x/numCheckpoints, z/numCheckpoints, parent.fourthChild);
			}
			// if list only has one checkpoint, push checkpoint onto tree
			else if (lessLess.Count == 1)
			{
				foreach(GameObject lL in lessLess)
				{
					parent.fourthChild = new Node(lL.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
		}
	}
	int timesSearched = 0;
	
	// search tree for closest node to Vector3 passed in
	public Node search(Vector3 loc)
	{
		if(m_root != null)
			return searchHelper(m_root, loc, null, 10000000);
		else
			return null;
	}
	// recursively searches through tree
	private Node searchHelper(Node other, Vector3 loc, Node closestNode, float distance)
	{
		Log.M ("checkpoint","timesSearched" + ++timesSearched);
		// if node is not empty node designated by bool
		if(other.NULL == false)
		{
			// if distance between this node and loc is less than previous closest distance
			if(Vector3.Distance(other.location, loc) < distance)
			{	
				// set this as closest node
				closestNode =  other;
				// set distance between this node and loc as the new closest distance
				distance = Vector3.Distance(other.location, loc);
			}
		}
		// if node is not null and this is the closest path or next node isempty node designated by bool
		if(other.firstChild != null && ((Vector3.Distance(other.firstChild.location, loc) < distance) || other.firstChild.NULL == true))
		{
			// recursively search and update distance
			closestNode = searchHelper(other.firstChild,loc,closestNode,distance);
			distance = Vector3.Distance(closestNode.location, loc);
		}
		// if node is not null and this is the closest path or next node is empty node designated by bool
		if(other.secondChild != null && ((Vector3.Distance(other.secondChild.location, loc) < distance)|| other.secondChild.NULL == true))
		{
			// recursively search and update distance
			closestNode = searchHelper(other.secondChild,loc,closestNode,distance);
			distance = Vector3.Distance(closestNode.location, loc);
		}
		// if node is not null and this is the closest path or next node is empty node designated by bool
		if(other.thirdChild != null && ((Vector3.Distance(other.thirdChild.location, loc) < distance) || other.thirdChild.NULL == true))
		{
			// recursively search and update distance
			closestNode = searchHelper(other.thirdChild,loc,closestNode,distance);		
			distance = Vector3.Distance(closestNode.location, loc);
		}
		// if node is not null and this is the closest path or next node is empty node designated by bool
		if(other.fourthChild != null && ((Vector3.Distance(other.fourthChild.location, loc) < distance) || other.fourthChild.NULL == true))
		{
			// recursively search and update distance
			closestNode = searchHelper(other.fourthChild,loc,closestNode,distance);
			distance = Vector3.Distance(closestNode.location, loc);
		}
		return closestNode;
	}
}
