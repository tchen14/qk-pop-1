using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

/*!
 *  This will save player quest and mission related data. This script is only called if a quest or mission has been started and not completed when the game is exited.
 *  This includes both quests and missions.
 *  Missions are distinguishable from quests where only 1 mission can be active at a time (think instance dungeons).
 */
 
public class QuestSaveManager : SaveManager {

	
	private List <string> currentQuests = new List <string>(); //List of current quest names to be saved to playerprefs
	private List <string> currentMission = new List <string>(); //List of current mission name parts to be saved to playerprefs

    public float checkUpdateSpeed = 1.0f;

    void Start() 
	{
        StartCoroutine ("SlowUpdate");
    }

    IEnumerator SlowUpdate() 
	{
        yield return new WaitForSeconds (checkUpdateSpeed);
        StartCoroutine ("SlowUpdate");
    }

	//!Saves a Quest with parts that is in progress to the playerprefs
    public void SaveQuest(string questName, string[] questPartsName, bool[] questPartsState) 
	{
		//Iterates through passed in quests and saves their current states to playerprefs
		int questPartsNameSize = questPartsName.Length;
		for(int count = 0; count < questPartsNameSize; count++)
		{
			//Quests are saved to playerprefs by a string of the "quest name" + "quest part name" + "a number (the iterator count)"
			//Quest states are saved to playerprefs with 1 being "true" and 0 being "false"
			if(questPartsState[count] == true)
				PlayerPrefs.SetInt(questName + "|" + questPartsName[count], 1);
			else
				PlayerPrefs.SetInt(questName + "|" + questPartsName[count], 0);

			currentQuests.Add(questName + "|" + questPartsName[count]);
		}
		SaveCurrentQuests();
    }

	//!Saves a Quest with parts and a timer that is in progress to the playerprefs
	public void SaveQuestWithTimer(string questName, string[] questPartsName, bool[] questPartsState, string questCurrentTimer)
	{
		//Iterates through passed in quests and saves their current states to playerprefs
		int questPartsNameSize = questPartsName.Length;
		for(int count = 0; count < questPartsNameSize; count++)
		{
			//Quests are saved to playerprefs by a string of the "quest name" + "quest part name" + "a number (the iterator count)"
			//Quest states are saved to playerprefs with 1 being "true" and 0 being "false"
			if(questPartsState[count] == true)
				PlayerPrefs.SetInt(questName + "|" + questPartsName[count], 1);
			else
				PlayerPrefs.SetInt(questName + "|" + questPartsName[count], 0);

			currentQuests.Add(questName + "|" + questPartsName[count]);
		}
		PlayerPrefs.SetString(questName + "|" + "Timer" + "|", questCurrentTimer);
		SaveCurrentQuests();
	}

	//!Saves a Mission with parts that is in progress to the playerprefs
	public void SaveMission(string missionName, string[] missionPartsName, bool[] missionPartsState) 
	{
		//Iterates through passed in mission and saves their current states to playerprefs
		int missionPartsNameSize = missionPartsName.Length;
		for(int count = 0; count < missionPartsNameSize; count++)
		{
			//Missions are saved to playerprefs by a string of the "mission name" + "mission part name" + "a number (the iterator count)"
			//Mission states are saved to playerprefs with 1 being "true" and 0 being "false"
			if(missionPartsState[count] == true)
				PlayerPrefs.SetInt(missionName + "|" + missionPartsName[count], 1);
			else
				PlayerPrefs.SetInt(missionName + "|" + missionPartsName[count], 0);

			currentMission.Add(missionName + "|" + missionPartsName[count]);
		}
		SaveCurrentMission();
	}
	
	//!Saves a Mission with parts and a timer that is in progress to the playerprefs
	public void SaveMissionWithTimer(string missionName, string[] missionPartsName, bool[] missionPartsState, string missionCurrentTimer) 
	{
		//Iterates through passed in mission and saves their current states to playerprefs
		int missionPartsNameSize = missionPartsName.Length;
		for(int count = 0; count < missionPartsNameSize; count++)
		{
			//Missions are saved to playerprefs by a string of the "mission name" + "mission part name" + "a number (the iterator count)"
			//Mission states are saved to playerprefs with 1 being "true" and 0 being "false"
			if(missionPartsState[count] == true)
				PlayerPrefs.SetInt(missionName + "|" + missionPartsName[count], 1);
			else
				PlayerPrefs.SetInt(missionName + "|" + missionPartsName[count], 0);

			currentMission.Add(missionName + "|" + missionPartsName[count]);
		}
		PlayerPrefs.SetString(missionName + "|" + "Timer" + "|", missionCurrentTimer);
		SaveCurrentMission();
	}

	//!Saves the current quest names as references for loading
	protected void SaveCurrentQuests()
	{
		//Iterates through the list of quests and adds their names to the current quests list
		int tempCount = 0;
		foreach(string quest in currentQuests)
		{
			PlayerPrefs.SetString("currentQuest" + tempCount, quest);
			tempCount++;
		}
	}

	//!Saves the current mission name as references for loading
	protected void SaveCurrentMission()
	{
		//Iterates through the list of quests and adds their names to the current quests list
		int tempCount = 0;
		foreach(string mission in currentMission)
		{
			PlayerPrefs.SetString("currentMission" + tempCount, mission);
			tempCount++;
		}
	}

	//!Takes in a quest that will be set to complete for the player in playerprefs
	public bool CompleteQuest(string inputQuest)
	{
		int tempCount = 0;
		//get the next iteration of the quest name to set to complete
		//this is for when the player completes multiple instances of the same quest name
		while(PlayerPrefs.HasKey(inputQuest + tempCount))
		{
			tempCount++;
		}

		//save data to playerPrefs and set vale to true(1)
		PlayerPrefs.SetInt (inputQuest + tempCount, 1);
		//return true when operation is complete
		return true;
	}

	//!Takes in a mission that will be set to complete for the player in playerprefs
	public bool CompleteMission(string inputMission)
	{
		//check to see if input string has a key in playerprefs
		if(!PlayerPrefs.HasKey(inputMission))
		{
			Debug.Error ("serialization", "Could Not Complete Achievement " + inputMission + " does not exist in PlayerPrefs");
			return false;
		}
		
		//save data to playerPrefs and set vale to true(1)
		PlayerPrefs.SetInt (inputMission, 1);
		//return true when operation is complete
		return true;
	}

	//!Loads current quests from playeprefs to load when game is started up
	//To load quests you need to:
	//	1. find the current quests by searching playerprefs for the key using GetString ("currentQuest" + (an iterator starting at 0))
	//  2. take that key and get the string associated with that key from playerprefs using GetString (using the string you received from step 1)
	//  3. split that string by a pipe delimiter into an array
	//	Optional 4. if there is a timer for the quest search for the key with GetString ((split string array index 0) + "|" + "Timer" + "|")
	//  5. starting with the second index(this will be the first quest part name) of the array find the state (complete (0) or not (1)) of the quest's parts in playerprefs 
	//		using the name of the quest part as the key with GetInt (split string array index 1 and iterate through the array until the end)
	//The returned list will be will contain all of the current quests that the player has in this format
	//	"Quest"(The actual word) , "QuestName" , "QuestPartName" , "0" or "1" (0 if the quest is not complete and 1 if the quest is complete) 
	//			, then repeats with each quest part and its completion status
	//  If the quest has a timer the word "Timer" will come after the name of the quest and the next item in the list will be the time string, it will then continue with the quest parts
	//	Each new quest in the list will start with the word "Quest" which will allow you to fine where a new quest starts in the list
	public List <string> LoadCurrentQuests()
	{
		//List to hold all the loaded quests from playerprefs
		List <string> loadedQuests = new List <string>();
		int tempCount = 0;

		//While there are quests in the playerprefs iterate through to load them into the list
		while(PlayerPrefs.HasKey("currentQuest" + tempCount))
		{
			//Gets the string for the current quest key and splits it with a pipe delimiter
			string tempQuest = PlayerPrefs.GetString("currentQuest" + tempCount);
			//The first element in the array will be the quest name and the second one will be the quest part name
			string[] questArray = tempQuest.Split('|');

			//Bool to determine whether a new quest is being parted out to add its name to the list
			bool newQuestName = true;
			//Iterate through the current list of loaded quests to see if the current quest is in the list already or not
			foreach(string questName in loadedQuests)
			{
				if(questName == questArray[0])
					newQuestName = false;
			}

			//If the current quest is not in the list then add its name to the list
			if(newQuestName)
			{
				//Adds the term "Quest" before the quest name to know when there is a quest in the list
				loadedQuests.Add("Quest");
				loadedQuests.Add(questArray[0]);
				newQuestName = true;
			}

			//Check to see if the quest has a timer using the key of the quest name
			if(PlayerPrefs.HasKey(questArray[0] + "|" + "Timer" + "|"))
			{
				//Adds the term "Timer" before the timer element to know when there is a timer for a quest in the list
				loadedQuests.Add("Timer");
				loadedQuests.Add(PlayerPrefs.GetString(questArray[0] + "|" + "Timer" + "|"));

				//Delete the key for the time after it is added to the list
				PlayerPrefs.DeleteKey(questArray[0] + "|" + "Timer" + "|");
			}

			//Iterate through the keys of the quest to add their part names and their states (0 if it is completed or 1 if it is not completed) to the list
			for(int count = 1; count < questArray.Length; count++)
			{
				//Adds the name of the quest part to the list
				loadedQuests.Add(questArray[count]);
				string tempString = PlayerPrefs.GetInt(tempQuest).ToString();
				//Adds the quest part state to the list as a string (0 if it is completed or 1 if it is not completed)
				loadedQuests.Add(tempString);

			}

			//Delete each quest part key after it is added to the list
			for(int count = 0; count < questArray.Length; count++)
			{
				PlayerPrefs.DeleteKey(questArray[count]);
			}

			//Delete the current quest key from playerprefs
			PlayerPrefs.DeleteKey("currentQuest" + tempCount);
			tempCount++;
		}

		//Return the list of loaded quests
		return loadedQuests;
	}

	//!Loads current mission from playeprefs to load when game is started up
	//Same instructions as above from LoadCurrentQuests
	public List <string> LoadCurrentMission()
	{
		//List to hold all the loaded missions from playerprefs
		List <string> loadedMission = new List <string>();
		int tempCount = 0;

		//While there are missions in the playerprefs iterate through to load them into the list
		while(PlayerPrefs.HasKey("currentMission" + tempCount))
		{
			//Gets the string for the current mission key and splits it with a pipe delimiter
			string tempMission = PlayerPrefs.GetString("currentMission" + tempCount);
			string[] missionArray = tempMission.Split('|');

			//Bool to determine whether a new mission is being parted out to add its name to the list
			bool newMissionName = true;
			//Iterate through the current list of loaded missions to see if the current mission is in the list already or not
			foreach(string missionName in loadedMission)
			{
				if(missionName == missionArray[0])
					newMissionName = false;
			}

			//If the current mission is not in the list then add its name to the list
			if(newMissionName)
			{
				//Adds the term "Mission" before the mission name to know when there is a mission in the list
				loadedMission.Add("Mission");
				loadedMission.Add(missionArray[0]);
				newMissionName = true;
			}

			//Check to see if the mission has a timer using the key of the mission name
			if(PlayerPrefs.HasKey(missionArray[0] + "|" + "Timer" + "|"))
			{
				//Adds the term "Timer" before the timer element to know when there is a timer for a mission in the list
				loadedMission.Add("Timer");
				loadedMission.Add(PlayerPrefs.GetString(missionArray[0] + "|" + "Timer" + "|"));
				
				//Delete the key for the time after it is added to the list
				PlayerPrefs.DeleteKey(missionArray[0] + "|" + "Timer" + "|");
			}

			//Iterate through the keys of the mission to add their part names and their states (0 if it is completed or 1 if it is not completed) to the list
			for(int count = 1; count < missionArray.Length; count++)
			{
				//Adds the name of the mission part to the list
				loadedMission.Add(missionArray[count]);
				string tempString = PlayerPrefs.GetInt(tempMission).ToString();
				//Adds the mission part state to the list as a string (0 if it is completed or 1 if it is not completed)
				loadedMission.Add(tempString);
				
			}
			
			//Delete each mission part key after it is added to the list
			for(int count = 0; count < missionArray.Length; count++)
			{
				PlayerPrefs.DeleteKey(missionArray[count]);
			}

			//Delete the current mission key from playerprefs
			PlayerPrefs.DeleteKey("currentMission" + tempCount);
			tempCount++;
		}

		//Return the list of loaded missions
		return loadedMission;
	}

	/******
	 * 
	//Template for loading and receiving quests in another script
	 *
	/******
	void ReceiveLoadedQuests()
	{
		//Create a list to catch the return for calling LoadCurrentQuests
		//myQuestSaveManager would be your instance of a QuestSaveManager
		List <string> receivedQuests = myQuestSaveManager.LoadCurrentQuests();
		
		//Iterate through the the receivedQuests list in order to get the name of quests and their parts
		//Each time the for loop iterates it will be a new quest that is in the receivedQuests list
		int receivedQuestsLength = receivedQuests.Count;
		int count = 0;
		while(count < receivedQuestsLength)
		{
			//if the current iteration is the word "Quest" then the next index will be the name of the quest
			if(count < receivedQuestsLength && receivedQuests[count] == "Quest")
			{
				//This index will be the quest name
				count++;
				myQuestName = receivedQuests[count];
				
				//Do something here with the quest name like find the class for that quest name

				count++;
				//If the quest has a timer the next index will be the keyword "Timer" meaning the next index after that will be the time
				if(count < receivedQuestsLength && receivedQuests[count] == "Timer")
				{
					//This index will be the quest timer as a string
					count++;
					myQuestTimer = receivedQuests[count];
					count++;
					//Do something here with the quest timer like send the timer to the quest class
				}
				
				//If the quest does not have a timer the next index will be a quest part name
				//Keep iterating through the receivedQuests list until the next quest is found or the end of the list occurs
				while(count < receivedQuestsLength && receivedQuests[count] != "Quest")
				{
					//This index will be the quest part name
					myQuestPartName = receivedQuests[count];
					count++;
					if(count < receivedQuestsLength)
					{
						//This index will be the quest part state as a string (0 if not completed 1 if completed)
						myQuestPartNameState = receivedQuests[count];
					}
					count++;
					
					//do something here with the quest part name and the state (0 if not completed 1 if completed) of the quest part
				}
			}
		}
	}
	
	/******
	 * 
	//Template for loading and receiving missions in another script
	 *
	/******
	void ReceiveLoadedMission()
	{
		//Create a list to catch the return for calling LoadCurrentMission
		//myMissionSaveManager would be your instance of a MissionSaveManager
		List <string> receivedMission = myQuestSaveManager.LoadCurrentMission();
		
		//Iterate through the the receivedMission list in order to get the name of quests and their parts
		//Each time the for loop iterates it will be a new quest that is in the receivedMission list
		int receivedMissionLength = receivedMission.Count;
		int count = 0;
		while(count < receivedMissionLength)
		{
			//if the current iteration is the word "Mission" then the next index will be the name of the quest
			if(count < receivedMissionLength && receivedMission[count] == "Mission")
			{
				//This index will be the quest name
				count++;
				myMissionName = receivedMission[count];
				
				//Do something here with the quest name like find the class for that quest name
				
				count++;
				//If the quest has a timer the next index will be the keyword "Timer" meaning the next index after that will be the time
				if(count < receivedMissionLength && receivedMission[count] == "Timer")
				{
					//This index will be the quest timer as a string
					count++;
					myMissionTimer = receivedMission[count];
					count++;
					//Do something here with the quest timer like send the timer to the quest class
				}
				
				//If the quest does not have a timer the next index will be a quest part name
				//Keep iterating through the receivedMission list until the next quest is found or the end of the list occurs
				while(count < receivedMissionLength && receivedMission[count] != "Mission")
				{
					//This index will be the quest part name
					myMissionPartName = receivedMission[count];
					count++;
					if(count < receivedMissionLength)
					{
						//This index will be the quest part state as a string (0 if not completed 1 if completed)
						myMissionPartNameState = receivedMission[count];
					}
					count++;
					
					//do something here with the quest part name and the state (0 if not completed 1 if completed) of the quest part
				}
			}
		}
	}
	******/
}