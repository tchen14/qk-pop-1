using UnityEngine;
using System.Collections;

/*
 *  This will save player related data. The player in this context is defined as all code data.
 *  Data such as player inventory will be stored here
 */
public class PlayerSaveManager : SaveManager {

    public string playerName = "";

    //! Unity Start function
    void Start() {
    }

    //! Saves player data as json string to PlayerPrefs \todo pseudo code -> code
    public bool SavePlayer() {
        //make/find data structure with all play stat data
        //format data into json string
        //save data to playerPrefs
        //return true when operation is complete
        return true;
    }

    //! Loads player data as json string to PlayerPrefs \todo pseudo code -> code
    public bool LoadPlayer() {
        //load data from playerPrefs; if no data exists, return false
        //interperate data from json string
        //load data from formatted json string
        //return true when operation is complete
        return true;
    }

    //! Removes player data in PlayerPrefs \todo pseudo code -> code
    public bool DeletePlayerData() {
        //detect if slot is not empty, else return false
        //delete playerPref data
        //return true when operation is complete
        return true;
    }
}
