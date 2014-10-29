/*LOCATION AND PATHING FOR THE AI
 * this code is applied to a Node Object that should be ordered into a specific tree
 * This tree should be:
 * 	WayPoints: An empty object as Parent
 *  A,B,C...etc: Any node in that path as a child to WayPoints
 * It should be checked if this system is a random path,
 * 	if so check randomTarget on in the inspector
 * 	if it is a set path then the Transform nextTarget should get the node transform of
 * 		the next node in the path and randomTarget is set off
 *
 * 	This code automaticly finds the parent and all the children to get random pathing
 *
 */

using UnityEngine;
using System.Collections;

public class ChangeTarget : MonoBehaviour {

    public Transform nextTarget;		//the next point in a scripted path
    public Transform cluster;			//the parent of the path cluster to generate random points in mesh
    public Transform[] clusterChildren;	//the children in the cluster

    public bool randomTarget;			//determines if the AI will use scripted path or a random path

    void Start() {
        //Gets this points parent
        cluster = transform.parent.gameObject.transform;
        //gathers all the children of the parent
        clusterChildren = cluster.GetComponentsInChildren<Transform> ();
    }
    void OnCollisionEnter (Collision col) {
        if (col.gameObject.name == "_NPC") {
            if (col.gameObject.GetComponent<AI_movement>().navPoint == transform) {
                if (randomTarget) {
                    //loop used to insure random point is not the same point
                    do {
                        col.gameObject.GetComponent<AI_movement>().ChangeNavPoint (clusterChildren[Random.Range (1,clusterChildren.Length)].transform);
                    } while (col.gameObject.GetComponent<AI_movement>().navPoint == transform);
                } else
                    col.gameObject.GetComponent<AI_movement>().navPoint = nextTarget;
            }
        }
    }
}
