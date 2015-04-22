using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory _instance;
  
    public static PlayerInventory Instance
    {
      get
      {
        if(_instance == null)
        {
          _instance = GameObject.FindObjectOfType<PlayerInventory>();
        }
        return _instance;
      }
    }

    public List<InventoryItem> inventory = new List<InventoryItem>();

    //! Helper function used to search for an already existing item in PlayerInventory
    public int FindItemIndex(InventoryItem item)
    {
        for (int index = 0; index < inventory.Count; index++)
        {
            if(item.GetItemName() == inventory[index].GetItemName())
            {
                return index;
            }
        }
        return -1;
    }

    //! Function that adds an item to player inventory
    public bool AddItem (InventoryItem item)
    {
        if (item.GetItemAmount() < 1)
        {
            // Item amount invalid, return false and error
            return false;
        }

        int index = FindItemIndex(item);

        if (index == -1) // Item not found
        {
            inventory.Add(item);
            return true;
        }
        else
        {
            int value = inventory[index].GetItemAmount();
            value += item.GetItemAmount();
            inventory[index].SetItemAmount(value);
            return true;
        }
    }

    //! Function that removes an item from player inventory, need to specify itemAmount to remove a certain # of items
    public bool RemoveItem (InventoryItem item)
    {
        if (item.GetItemAmount() < 1)
        {
            // Item amount invalid, return false and error
            return false;
        }

        int index = FindItemIndex(item);

        if (index == -1)
        {
            // Item not found, so return error?
            return false;
        }
        else
        {
            int value = inventory[index].GetItemAmount();
            value -= item.GetItemAmount();

            if (value < 0)
            {
                // Error because you tried to remove more items than the player currently has
                return false;
            }

            inventory[index].SetItemAmount(value);
            return true;
        }
    }

    //! Load Inventory with saved state
    public void LoadInventory()
    {
        inventory.Clear();
       // inventory = PlayerSaveManager.Instance.LoadPlayerInventory();
    }


    //! Function that returns the current state of player inventory to PlayerSaveManager
	public void SaveInventory()
	{
       // PlayerSaveManager.Instance.SavePlayerInventory(inventory);
	}
}
