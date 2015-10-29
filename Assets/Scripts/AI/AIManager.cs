using UnityEngine;
using System.Collections;

public class AIManager : MonoBehaviour {
	Transform[] AI_children;
	int AI_children_length;

	// Start Gathers all children that this manager is in charge of
	void Start () 
	{
		AI_children = GetComponentsInChildren<Transform> ();
		AI_children_length = AI_children.Length;
	}
	//Tells all AI in group to head to given point
	void SetNav (string target, Vector3 position) 
	{
		for (int i = 0; i < AI_children_length; i++) 
		{
			AI_children[i].GetComponent<AIMain>().ChangeNavPoint(target,position);		
		}
	}
	//Tells all AI in group to panic
		//Aggressive AI will not panic - use on non_aggressive NPC
	void SetPanic ()
	{
		for (int i = 0; i < AI_children_length; i++) 
		{
			AI_children[i].GetComponent<AIMain>().panic = true;
		}
	}
	//Tells all AI in group to become aggressive
	void SetAggression ()
	{
		for (int i = 0; i < AI_children_length; i++) 
		{
			AI_children[i].GetComponent<AIMain>().aggressive = true;
		}
	}
	//AI become immediatly aggressive and will hunt and attack the player
	void AttackPlayer ()
	{
		for (int i = 0; i < AI_children_length; i++) 
		{
			AI_children[i].GetComponent<AIMain>().aggressive = true;
			AI_children[i].GetComponent<AIMain>().aggressionLevel = AI_children[i].GetComponent<AIMain>().aggressionLimit;
			AI_children[i].GetComponent<AIMain>().seesTarget = true;
		}
	}
	//Delete all AI children
	void Destroy()
	{
		for (int i = 0; i < AI_children_length; i++) 
		{
			Destroy(AI_children[i]);
		}
	}
		
}
