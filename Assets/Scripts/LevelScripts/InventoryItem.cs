using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem
{
    [SerializeField]
    private string itemName = ""; //!<

    [SerializeField]
    private int itemID; //!<

    [SerializeField]
    private string itemDescription; //!<

    [SerializeField]
    private int itemValue; //!<

    [SerializeField]
    private int itemAmount; //!<

    [SerializeField]
    private ItemType itemType; //!<


    public InventoryItem(string Name, int ID, string Description, int Value, int Amount, int Type)
    {
        itemName = Name;
        itemID = ID;
        itemDescription = Description;
        itemValue = Value;
        itemAmount = Amount;
        itemType = (ItemType) Type;
    }

    /*public InventoryItem(string Name, string Description, int Value, int Amount, int Type)
    {
        InventoryItem(Name, 0, Description, Value, Amount, Type);
    }*/

    private enum ItemType
    {
        //! Need list of item types, Quest is the only one being used
        Consumable,
        Sellable,
        Quest
    }
	
	public int GetItemType()
	{
        return (int)itemType;
	}
	
	public string GetItemName()
	{
		return itemName;
	}
	
	public string GetItemDescription()
	{
		return itemDescription;
	}
	
	public int GetItemValue()
	{
		return itemValue;
	}
	
	public int GetItemAmount()
	{
		return itemAmount;
	}

    public void SetItemAmount(int value)
    {
        itemAmount = value;
    }
	
	public int GetItemID()
	{
		return itemID;
	}
}
