using UnityEngine;
using System.Collections;

/*!
 *  Manager for all achievements
 *  Achievements are independent of the player profile (they are unlocked for all users of the device).
 */
public class AchievementManager : SerializationManager {

    const float periodicUpdateSpeed = 5.0f;

    void Start() {
        StartCoroutine ("SlowUpdate");
    }

    IEnumerator PeriodicUpdate() {
        ;
        yield return new WaitForSeconds (periodicUpdateSpeed);
    }

    //! Updates local json string with true bool value and saves to PlayerPrefs \todo pseudo code -> code
    public bool CompleteAchievement(string s) {
        //make/find data structure with all play stat data
        //format data into json string
        //save data to playerPrefs
        //return true when operation is complete
        return true;
    }

    //! Loads json string data from PlayerPrefs \todo pseudo code -> code
    public bool LoadPlayer(int id) {
        //load data from playerPrefs; if no data exists, return false
        //format data from json string
        //load data from formatted json string
        //return true when operation is complete
        return true;
    }

    //! Deletes json string data from PlayerPerfs \todo pseudo code -> code
    public bool DeletePlayerData(int id) {
        //detect if slot is not empty, else return false
        //delete playerPref data
        //return true when operation is complete
        return true;
    }
}
