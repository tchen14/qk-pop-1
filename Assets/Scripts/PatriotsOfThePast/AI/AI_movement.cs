/* AI MOVEMENT
 * 
 * this script collects the NavMesh for the AI and controls its movement through the mesh
 * it will automaticly adjust the speed based off of the state of the AI and the AI speeds for that state
 * it will automaticly adjust the target on the NavMesh by taking in a transform
 * 
 */


using UnityEngine;
using System.Collections;
[RequireComponent(typeof(NavMeshAgent))]	//creates the component navmesh

public class AI_movement : MonoBehaviour {


	private NavMeshAgent mesh = null;			//contains the component to use the navmesh
	public Vector3 navPoint;			//contains the point to move in the navmesh

	//Discovers the NavMesh
	void Start () 
	{
		//if no navMesh has been given to this AI then it will create one, AI needs navMesh
		mesh = GetComponent<NavMeshAgent>();
		ChangeNavPoint(GetComponent<AI_main>().AI_StartPoint.position);
		SetSpeed (GetComponent<AI_main> ().AI_Speed);
	}
	//always update the Nav Mesh to insure most dynamic AI movement
	void Update()
	{
		mesh.SetDestination(navPoint);
	}
	//sets a new destination for the AI and can be publicly accessed
	public void ChangeNavPoint(Vector3 T)
	{
		navPoint = T;
	}
	public void SetSpeed(float Spd)
	{
		mesh.speed = Spd;
	}
	public void Pause()
	{
		Debug.Log ("W");
		mesh.Stop();
	}

}
