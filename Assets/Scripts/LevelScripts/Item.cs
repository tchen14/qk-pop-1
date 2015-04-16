using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
[System.Serializable]
public abstract class Item : MonoBehaviour
{

    public string itemName = "";
    public int pushCounter, pullCounter, cutCounter, soundThrowCounter, stunCounter;
    public bool quincAffected = false;
	public bool pushCompatible = false;
	public bool pullCompatible = false;
	public bool cutCompatible = false;
	public bool soundThrowCompatible = false;
	public bool stunCompatible = false; //!> Might not need this for items
  
    public enum ItemType
    {
      //! Need list of item types
      Consumable,
      Quest,
      Sellable
    }

    //!Player gathers X number of this item, (kill all enemies in area, use other item script to auto drop item into play inventory)
    protected virtual void GatherObjective (int count)
	{
		return;
	}

    //!Player arrives at the item, option with timed, use negative number for stall quest
    protected virtual void ArriveObjective (Vector3 buffer, float timer = 0.0f)
	{
		return;
	}

    //!This item is brought to target location
    protected virtual void EscortObjective (Vector3 location, Vector3 buffer)
	{
		return;
	}
}