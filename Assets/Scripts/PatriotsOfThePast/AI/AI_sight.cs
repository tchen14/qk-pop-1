/*AI SIGHT CONTROLLER
 * 
 * this script controls the line of sight and the target finding program
 * it will check every N frames so as not to check every frame
 * it will check to see if a character or set of characters are viable targets
 * it will check for the closest of those targets
 * it will check if can see that target
 * it will return the closest target that it can see
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class AI_sight : MonoBehaviour {

	private RaycastHit hit;		//takes information from RayCast
	private GameObject[] viableTargets;	//all the available targets

	// Update is called once per frame
	void Update () 
	{
		//if there is no target then look for a target
		if(GetComponent<AI_main>().AI_target == null)
		{
			StartCoroutine("CheckForTargets");

		}
	}
	//called every tenth of a second
	public IEnumerator CheckForTargets()
	{

		yield return new WaitForSeconds(.1f);
		GetTargets();

	}
	//Sends Raycast and sets up AI_Main
	private void GetTargets()
	{
		//Gathers proabable enemy targets
		GameObject[] viableTargets = GameObject.FindGameObjectsWithTag(GetComponent<AI_main>().AI_seekTag);

		for(int i = 0; i < viableTargets.Length; i++)
		{
			Debug.DrawRay(transform.position,viableTargets[0].transform.position - transform.position);
			if(Physics.Raycast(transform.position,viableTargets[i].transform.position-transform.position,out hit,GetComponent<AI_main>().AI_SightDistance))
			{
				if(hit.collider.tag == viableTargets[i].tag)
				{
					GetComponent<AI_main>().AI_target = hit.collider.gameObject;
					GetComponent<AI_movement>().ChangeNavPoint(GetComponent<AI_main>().AI_target.transform);
					GetComponent<AI_main>().AI_seesTarget = true;
				}
			}

		}
	}
}
