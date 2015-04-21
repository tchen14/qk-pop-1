using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

/*
 *  This will save player related data. The player in this context is defined as all code data.
 *  Data such as player inventory will be stored here
 */


public class PlayerSaveManager : SaveManager {

    //! Saves player data as json string to PlayerPrefs \todo pseudo code -> code
	//public void SavePlayerInventory(List <InventoryItem> inputInventory)
	//{
	//	//create new list that will be saved to store the recieved list
	//	List <InventoryItem> inventorySave = new List <InventoryItem>();
	//	inventorySave = inputInventory;

	//	//if the inventory list is empty retuen
	//	if(inventorySave == null)
	//	{
	//		return;
	//	}

	//	//create jsonclass to store the inventory into
	//	JSONClass inventoryJsonNode = new JSONClass();
	//	int temp_count = 0;
	//	//iterate through each item and add its contents to the jsonclass
	//	foreach(InventoryItem item in inventorySave)
	//	{
	//		//saves out the inventory in order the setter functions appear in the inventory item script
	//		inventoryJsonNode["Inventory"][item.GetItemName()][0] = item.GetItemName();
	//		inventoryJsonNode["Inventory"][item.GetItemName()][1] = item.GetItemType().ToString();
	//		inventoryJsonNode["Inventory"][item.GetItemName()][2] = item.GetItemDescription();
	//		inventoryJsonNode["Inventory"][item.GetItemName()][3] = item.GetItemValue().ToString();
	//		inventoryJsonNode["Inventory"][item.GetItemName()][4] = item.GetItemAmount().ToString();
	//		inventoryJsonNode["Inventory"][item.GetItemName()][5] = item.GetItemID().ToString();

	//		temp_count++;
	//	}

	//	//save the inventory to the playerprefs
	//	PlayerPrefs.SetString("PlayerInventory", inventoryJsonNode.ToString());
	//}

	////! Loads player data as json string to PlayerPrefs \todo pseudo code -> code
	//public List <InventoryItem> LoadPlayerInventory() 
	//{
	//	//create list of inventory items to hold the inventory to return
	//	List <InventoryItem> inventoryLoad = new List <InventoryItem>();

	//	//if the inventory key doesnt exist return the empty list
	//	if(!PlayerPrefs.HasKey("PlayerInventory"))
	//	{
	//		return inventoryLoad;
	//	}

	//	//obtain the inventory string from the playerprefs and fill the list of inventory items
	//	JSONNode loadedPlayerInventory = JSON.Parse(PlayerPrefs.GetString("PlayerInventory"));
	//	for(int count = 0; count < 2; count++)
	//	{
	//		//loads the inventory in the order the setter functions appear in the inventory item script
	//		string tempName = loadedPlayerInventory["Inventory"][count][0];
	//		int tempType = loadedPlayerInventory["Inventory"][count][1].AsInt;
	//		string tempDescription = loadedPlayerInventory["Inventory"][count][2];
	//		int tempValue = loadedPlayerInventory["Inventory"][count][3].AsInt;
	//		int tempAmount = loadedPlayerInventory["Inventory"][count][4].AsInt;
	//		int tempID = loadedPlayerInventory["Inventory"][count][5].AsInt;

	//		//create the inventory item and add it to the list
	//		InventoryItem new_item = new InventoryItem(tempName, tempID, tempDescription, tempValue, tempAmount, tempType);
	//		inventoryLoad.Add(new_item);
	//	}

	//	//return the list of created inventory items
	//	return inventoryLoad;
	//}
}