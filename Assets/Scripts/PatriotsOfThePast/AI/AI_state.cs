using UnityEngine;
using System.Collections;

public class AI_state : MonoBehaviour {
	private float aggressionLevel = 0;
	// Use this for initialization
	void Start () {
		GetComponent<AI_main>().AI_panicTarget = GameObject.Find("Panicpoints").GetComponent<PanicTargets>().GetPanickPoint();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(GetComponent<AI_main>().AI_panic == true)
		{
			if(GetComponent<AI_main>().AI_Aggressive == true)
			{
				GetComponent<AI_main>().AI_panic =false;
			}
			else
			{
				GetComponent<AI_movement>().ChangeNavPoint(GetComponent<AI_main>().AI_panicTarget);
				GetComponent<AI_movement>().SetSpeed(GetComponent<AI_main>().AI_RunSpeed);
			}
		}
		//If the AI has a target already
		if(GetComponent<AI_main>().AI_Aggressive == true && GetComponent<AI_main>().AI_seesTarget == true && GetComponent<AI_main>().AI_attacking == false)
		{
			Debug.Log(aggressionLevel);
			if(aggressionLevel >= GetComponent<AI_main>().AI_AggessiveLimit)
				GetComponent<AI_movement>().ChangeNavPoint(GetComponent<AI_main>().AI_target.transform.position);
			else
				aggressionLevel += 1;
		}
		else
		{
			if(aggressionLevel > 0)
			{
				aggressionLevel -= 1;
			}
		}
	}
}
