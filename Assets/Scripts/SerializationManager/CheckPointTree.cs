
using UnityEngine;
using System.Collections.Generic;

public class newTree : MonoBehaviour {
	
	private Node m_root;
	public List<GameObject> checkpoints = new List<GameObject>();
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
		private bool _NULL;

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
			set { _location = value; }
		}	
		public bool NULL
		{
			get { return _NULL; }
			set { _NULL = value; }
		}
	};
	
	// Use this for initialization
	void Start () 
	{
		x = 0;
		float y = 0;
		z = 0;
		numCheckpoints = 0;
		GameObject[] temps;
		temps = GameObject.FindGameObjectsWithTag("Respawn");
		foreach(GameObject temp in temps)
		{
			x += temp.transform.position.x;
			y += temp.transform.position.y;
			z += temp.transform.position.z;
			numCheckpoints++;
		}
		for(int i =0; i < numCheckpoints; i++)
		{
			checkpoints.Add(temps[i]);
		}
//		print(checkpoints.Count);
//		print(x);
		m_root = new Node(Vector3.zero,null,null,null,null,null,true);
		recursiveHelper(checkpoints);
		print(search (new Vector3(1,1,1)).location);
	}
	void recursiveHelper(List<GameObject>checkpoints)
	{
		recursiveInsert(checkpoints,x/numCheckpoints,z/numCheckpoints,m_root);
	}
	void recursiveInsert(List<GameObject>checkpoints, float xPos, float zPos, Node parent)
	{
		List<GameObject> greaterGreater = new List<GameObject>();
		List<GameObject> greaterLess = new List<GameObject>();
		List<GameObject> lessGreater = new List<GameObject>();
		List<GameObject> lessLess = new List<GameObject>();
		
		if(checkpoints.Count > 0)
		{
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
			#region firstChild
			if(greaterGreater.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				foreach(GameObject gG in greaterGreater)
				{
					x += gG.transform.position.x;
					z += gG.transform.position.z;
					numCheckpoints++;
				}
				parent.firstChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
//				print("gG" + greaterGreater.Count);
				recursiveInsert(greaterGreater, x/numCheckpoints, z/numCheckpoints,parent.firstChild);
			}
			else if (greaterGreater.Count == 1)
			{
				foreach(GameObject gG in greaterGreater)
				{
					parent.firstChild = new Node(gG.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
			
			#region secondChild
			if(greaterLess.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				foreach(GameObject gL in greaterLess)
				{
					x += gL.transform.position.x;
					z += gL.transform.position.z;
					numCheckpoints++;
				}
				parent.secondChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
//				print("gL" + greaterLess.Count);;
				recursiveInsert(greaterLess, x/numCheckpoints, z/numCheckpoints,parent.secondChild);
			}
			else if (greaterLess.Count == 1)
			{
				foreach(GameObject gL in greaterLess)
				{
					parent.secondChild = new Node(gL.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
			
			#region thirdChild
			if(lessGreater.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				foreach(GameObject lG in lessGreater)
				{
					x += lG.transform.position.x;
					z += lG.transform.position.z;		
					numCheckpoints++;
				}
				parent.thirdChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
//				print("lG" + lessGreater.Count);
				recursiveInsert(lessGreater, x/numCheckpoints, z/numCheckpoints, parent.thirdChild);
			}
			else if (lessGreater.Count == 1)
			{
				foreach(GameObject lG in lessGreater)
				{
					parent.thirdChild = new Node(lG.transform.position,parent,null,null,null,null,false);
				}
			}
			#endregion
			
			#region fourthChild
			if(lessLess.Count > 1)
			{
				x = 0;
				z = 0;
				numCheckpoints = 0;
				foreach(GameObject lL in lessLess)
				{
					x += lL.transform.position.x;
					z += lL.transform.position.z;
					numCheckpoints++;
				}
				parent.fourthChild = new Node(Vector3.zero,parent,null,null,null,null,true);
				
//				print("lL" + lessLess.Count);
				recursiveInsert(lessLess, x/numCheckpoints, z/numCheckpoints, parent.fourthChild);
			}
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

	public Node search(Vector3 loc)
	{
		return searchHelper(m_root, loc, null, 10000000);
	}
	// recursively searches for nodes with matching key
	private Node searchHelper(Node other, Vector3 loc, Node closestNode, float distance)
	{
		// print ("timesSearched" + ++timesSearched);
		// print ("nodes"+other.location);
		if(other.NULL == false)
		{
			if(Vector3.Distance(other.location, loc) < distance)
			{	
				closestNode =  other;
				distance = Vector3.Distance(other.location, loc);
			}
		}
		if(other.firstChild != null && (Vector3.Distance(other.firstChild.location, loc) < distance))
		{
			closestNode = searchHelper(other.firstChild,loc,closestNode,distance);
			distance = Vector3.Distance(closestNode.location, loc);
		}
		if(other.secondChild != null && (Vector3.Distance(other.secondChild.location, loc) < distance))
		{
			closestNode = searchHelper(other.secondChild,loc,closestNode,distance);
			distance = Vector3.Distance(closestNode.location, loc);
		}
		if(other.thirdChild != null && (Vector3.Distance(other.thirdChild.location, loc) < distance))
		{
			closestNode = searchHelper(other.thirdChild,loc,closestNode,distance);		
			distance = Vector3.Distance(closestNode.location, loc);
		}
		if(other.fourthChild != null && (Vector3.Distance(other.fourthChild.location, loc) < distance))
		{
			closestNode = searchHelper(other.fourthChild,loc,closestNode,distance);
			distance = Vector3.Distance(closestNode.location, loc);
		}
		return closestNode;
	}
}

