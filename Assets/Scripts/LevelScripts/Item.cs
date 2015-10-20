using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]
[System.Serializable]
public abstract class Item : MonoBehaviour
{

    public string itemName = "";
    [EventVisible("Pushed X Times")]
    public int pushCounter;
    [EventVisible("Pulled X Times")]
    public int pullCounter;
    [EventVisible("Cut X Times")]
    public int cutCounter;
    [EventVisible("Sound Thrown X Times")]
    public int soundThrowCounter;
    [EventVisible("Stunned X Times")]
    public int stunCounter;
    [EventVisible("Affected by QuinC")]
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
}