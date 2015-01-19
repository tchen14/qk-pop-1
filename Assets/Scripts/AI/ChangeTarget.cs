using UnityEngine;
using System.Collections;

/*!
 *This code is the location and pathing for the AI. It is applied to a Node Object that should be ordered into a specific tree
 *This tree should be:
 *	WayPoints: An empty object as Parent
 *	A,B,C...etc: Any node in that path as a child to WayPoints
 *It should be checked if this system is a random path,
 *If so check randomTarget on in the inspector
 *If it is a set path then the Transform nextTarget should get the node transform of
 *The next node in the path and randomTarget is set off
 * 	
 *This code automaticly finds the parent and all the children to get random pathing
 */
public class ChangeTarget : MonoBehaviour {

	public Transform nextTarget;		/*!<the next point in a scripted path */
	public Transform cluster;			/*!<the parent of the path cluster to generate random points in mesh */
	public Transform[] clusterChildren;	/*!<the children in the cluster */
 
	public bool randomTarget;			/*!<determines if the AI will use scripted path or a random path */

	void Start()
	{
		//Gets this points parent
		cluster = transform.parent.gameObject.transform;
		//gathers all the children of the parent
		clusterChildren = cluster.GetComponentsInChildren<Transform> ();
	}
	void OnCollisionEnter(Collision col)
	{

		if (col.gameObject.name == "_NPC") 
		{
			if(col.gameObject.GetComponent<AIMain>().navPoint == transform.position)
			{
				if(randomTarget)
				{
					//loop used to insure random point is not the same point
					do
					{
						col.gameObject.GetComponent<AIMain>().ChangeNavPoint(clusterChildren[Random.Range(1,clusterChildren.Length)].transform.position);
					}while(col.gameObject.GetComponent<AIMain>().navPoint == transform.position);
				}
				else
				{
					col.gameObject.GetComponent<AIMain>().navPoint = nextTarget.position;
				}
			}
		}
	}
}
