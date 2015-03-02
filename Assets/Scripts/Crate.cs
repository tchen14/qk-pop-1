using UnityEngine;
using System.Collections;
using Debug=FFP.Debug;

public class Crate : Item
{
	// Use this for initialization
	void Start ()
	{
		itemName = "Crate";
		pushCompatible = true;
		pullCompatible = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//!Player gathers X number of this item, (kill all enemies in area, use other item script to auto drop item into play inventory)
	protected override void GatherObjective (int count)
	{
		Debug.Log("player", "Test GatherObjective");
	}
	
	//!Player arrives at the item, option with timed, use negative number for stall quest
	protected override void ArriveObjective (Vector3 buffer, float timer = 0.0f)
	{
		Debug.Log("player", "Test ArriveObjective");
	}
	
	//!This item is brought to target location
	protected override void EscortObjective (Vector3 location, Vector3 buffer)
	{
		Debug.Log("player", "Test EscortObjective");
	}
}
