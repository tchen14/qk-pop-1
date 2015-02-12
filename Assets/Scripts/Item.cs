using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
public abstract class Item : MonoBehaviour {

    public string itemName = "";

    #region modifiers
    public float speedMod = 1.0f;
    public float jumpMod = 1.0f;
    #endregion

	#region quinc compatiblities
	public bool pushCompatible = false;
	public bool pullCompatible = false;
	public bool cutCompatible = false;
	public bool soundThrowCompatible = false;
	public bool stunCompatible = false; // Might not need this for items
	#endregion

    //!Player gathers X number of this item, (kill all enemies in area, use other item script to auto drop item into play inventory)
    protected abstract void GatherObjective (int count);

    //!Player arrives at the item, option with timed, use negative number for stall quest
    protected abstract void ArriveObjective (Vector3 buffer, float timer = 0.0f);

    //!This item is brought to target location
    protected abstract void EscortObjective (Vector3 location, Vector3 buffer);
}