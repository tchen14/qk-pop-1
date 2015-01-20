using UnityEngine;
using System.Collections;

/*!
 *	Manager class for all quests
 *	Missions are distinguishable from quests where only 1 mission can be active at a time (think instance dungeons). This is a quest manager.
 */
public class QuestManager : EventManager {
	public string activeMission = ""; //!< The id of the active mission
	
    //assigned by phone, time based, quest based
    //talking to people starts quests or finding an item
    //proximity

    //extra shit below (to-do later)
    //quest based on number of kills/knock outs
    //quest based on time played
}
