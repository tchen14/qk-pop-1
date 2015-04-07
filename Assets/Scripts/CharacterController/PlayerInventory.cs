using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory _instance;
    private static PlayerSaveManager saveManager;
  
    public static PlayerInventory instance
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


    //! Function that adds an item to player inventory
    public void AddItem (InventoryItem item)
    {
        inventory.Add(item);
    }

    //! Function that removes an item from player inventory
    public void RemoveItem (InventoryItem item)
    {
        inventory.Remove(item);
    }

    //! Load Inventory with saved state
    public void LoadInventory()
    {
        inventory.Clear();
        inventory = saveManager.LoadPlayerInventory();
    }


    //! Function that returns the current state of player inventory to PlayerSaveManager
	public void SaveInventory()
	{
        saveManager.SavePlayerInventory(inventory);
	}
}
