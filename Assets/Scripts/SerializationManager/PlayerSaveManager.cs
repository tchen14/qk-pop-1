using UnityEngine;
using System.Collections;

public class PlayerSaveManager : SaveManager {

    void Start() {
    }

    public bool SavePlayer() {
        //make/find data structure with all play stat data
        //format data into json string
        //save data to playerPrefs
        //return true when operation is complete
        return true;
    }

    public bool LoadPlayer() {
        //load data from playerPrefs; if no data exists, return false
        //format data from json string
        //load data from formatted json string
        //return true when operation is complete
        return true;
    }

    public bool DeletePlayerData() {
        //detect if slot is not empty, else return false
        //delete playerPref data
        //return true when operation is complete
        return true;
    }
}
