using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public abstract class Item : MonoBehaviour {

	public string itemName = "";

	#region modifiers
	public float speedMod = 1.0f;
	public float jumpMod = 1.0f;
	#endregion

	//player gathers X number of this item, (kill all enemies in area, use other item script to auto drop item into play inventory)
	protected abstract void GatherObjective (int count);

	//player arrives at the item, option with timed, use negative number for stall quest
	protected abstract void ArriveObjective (Vector3 buffer, float timer = 0.0f);

	//this item is brought to target location
	protected abstract void EscortObjective(Vector3 location, Vector3 buffer);
}
