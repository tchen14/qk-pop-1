using UnityEngine;
using System.Collections;

public class AI_attack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Vector3.Distance(transform.position, GetComponent<AI_main>().AI_target.transform.position));
		if(GetComponent<AI_main>().AI_target != null)
		{
		   	if (GetComponent<AI_main>().AI_attackDistance >= Vector3.Distance(transform.position, GetComponent<AI_main>().AI_target.transform.position))
			{
				GetComponent<AI_main>().AI_attacking = true;
				//GetComponent<AI_movement>().ChangeNavPoint(transform);
				Debug.Log ("ATTACK!");
				GetComponent<AI_main> ().AI_target = null;
			}
		}
	}
	public IEnumerator EndAttack()
	{
		yield return new WaitForSeconds (1);

	}
}
