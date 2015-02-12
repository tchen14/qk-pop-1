using UnityEngine;
using System.Collections;
using SimpleJSON;
using Debug = FFP.Debug;

/*!
 *  Manager for all achievements
 *  Achievements are independent of the player profile (they are unlocked for all users of the device).
 */

public class AchievementManager : SerializationManager {

    const float periodicUpdateSpeed = 5.0f;

    void Start() {

		LoadAchievementsFromJson();
       //StartCoroutine ("SlowUpdate");
    }

    IEnumerator PeriodicUpdate() {
        ;
        yield return new WaitForSeconds (periodicUpdateSpeed);
    }

    //! Sets one inputed key (achievement) to ture(1) as completed
	public bool CompleteAchievement(string inputAchievement) {
		//check to see if input string has a key in playerprefs
		if(!PlayerPrefs.HasKey(inputAchievement))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputAchievement + " does not exist in PlayerPrefs");
			return false;
		}

        //save data to playerPrefs and set vale to true(1)
		PlayerPrefs.SetInt (inputAchievement, 1);
        //return true when operation is complete
        return true;
    }

	//! Sets two inputed keys (achievement) to ture(1) as completed
	public bool CompleteAchievement(string inputAchievement_1, string inputAchievement_2) {
		//check to see if input string has a key in playerprefs
		if(!PlayerPrefs.HasKey(inputAchievement_1))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputAchievement_1 + " does not exist in PlayerPrefs");
			return false;
		}
		if(!PlayerPrefs.HasKey(inputAchievement_2))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputAchievement_2 + " does not exist in PlayerPrefs");
			return false;
		}

		//save data to playerPrefs and set vale to true(1)
		PlayerPrefs.SetInt (inputAchievement_1, 1);
		PlayerPrefs.SetInt (inputAchievement_2, 1);
		//return true when operation is complete
		return true;
	}

	//! Checks to see if input string(achievement) is a completed event (true(1)) or not (false(0))
	public bool CheckAchievement(string inputAchievement)
	{
		//check to see if input string has a key in playerprefs
		if(!PlayerPrefs.HasKey(inputAchievement))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputAchievement + " does not exist in PlayerPrefs");
			return false;
		}

		//check to see if input string as a key is set to true(1) or false(0) in playerprefs
		if(PlayerPrefs.GetInt(inputAchievement) == 1)
			return true;
		else
			return false;
	}

	//! Checks to see if input strings(achievement) are completed events (true(1)) or not (false(0))
	public bool CheckAchievement(string inputAchievement_1, string inputAchievement_2)
	{
		//check to see if input string has a key in playerprefs
		if(!PlayerPrefs.HasKey(inputAchievement_1))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputAchievement_1 + " does not exist in PlayerPrefs");
			return false;
		}
		if(!PlayerPrefs.HasKey(inputAchievement_2))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputAchievement_2 + " does not exist in PlayerPrefs");
			return false;
		}
		
		//check to see if input string as a key is set to true(1) or false(0) in playerprefs
		if(PlayerPrefs.GetInt(inputAchievement_1) == 1 && PlayerPrefs.GetInt(inputAchievement_2) == 1)
			return true;
		else
			return false;
	}

    //! Sets all achievements to false(0)
    public bool ClearAchievements() {
		//Checking to see if achievement json file exists if it doesnt return
		if(!System.IO.File.Exists(Application.dataPath + "/Resources/Json/achievementJson"))
		{
			Debug.Error ("serilaization", "Could not find Achievement JSON file");
			return false;
		}
		
		//read in achievement json file
		string jsonRead = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Json/achievementJson");
		JSONNode jsonParsed = JSON.Parse (jsonRead);
		
		//get total achievements in json file
		int totalAchievements = jsonParsed["numberOfAchievements"].AsInt;
		//iterate through json file and load each key and its value into to check against playerpref keys and set them to false(0)
		for(int count = 0; count < totalAchievements; count = count + 1)
		{
			string loadedAchievement = jsonParsed["achievements"][count];
			PlayerPrefs.SetInt(loadedAchievement, 0);
		}
		
		//save to playerprefs just in case
		PlayerPrefs.Save();

        //return true when operation is complete
        return true;
    }

	//! Load achievements into playerprefs, all loaded achievements are set to false(0) for value by defualt
	protected void LoadAchievementsFromJson()
	{
		//Checking to see if achievement json file exists if it doesnt return
		if(!System.IO.File.Exists(Application.dataPath + "/Resources/Json/achievementJson"))
		{
			Debug.Error ("serilaization", "Could not find Achievement JSON file");
			return;
		}

		//read in achievement json file
		string jsonRead = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Json/achievementJson");
		JSONNode jsonParsed = JSON.Parse (jsonRead);
		
		//get total achievements in json file
		int totalAchievements = jsonParsed["numberOfAchievements"].AsInt;
		//iterate through json file and load each key and its value into playerprefs
		//all achievements are set to false(0) by defualt
		for(int count = 0; count < totalAchievements; count = count + 1)
		{
			string loadedAchievement = jsonParsed["achievements"][count];
			PlayerPrefs.SetInt(loadedAchievement, 0);
		}

		//save to playerprefs just in case
		PlayerPrefs.Save();
	}
}
