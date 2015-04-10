using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem
{
    private string itemName = "";
    private int itemID;
    private string itemDescription;
    private int itemValue;
	private int itemAmount;
    private ItemType itemType;

    public InventoryItem(string Name, int ID, string Description, int Value, int Amount, int Type)
    {
        itemName = Name;
        itemID = ID;
        itemDescription = Description;
        itemValue = Value;
        itemAmount = Amount;
        itemType = (ItemType) Type;
    }

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
	
	public int GetItemID()
	{
		return itemID;
	}
}
