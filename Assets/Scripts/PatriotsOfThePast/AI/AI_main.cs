/*AI MAIN CONTROLLER
 * 
 * this code holds all of the variables and collects all the required script components of an AI
 * This script should be the only edited script and it will start other scripts to operate the AI
 * but this script itself is not in control of any component except to hold the variables of the AI
 * 
 */

using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AI_movement))]
[RequireComponent(typeof(AI_attack))]
[RequireComponent(typeof(AI_state))]
[RequireComponent(typeof(AI_sight))]
[RequireComponent(typeof(AI_health))]

public class AI_main : MonoBehaviour {

	public bool AI_Aggressive 		= false;			//if the NPC will attack the player
	public int AI_HP 				= 100;				//Health of the NPC
	public float AI_SightDistance 	= 20;				//The distance the NPC is capable of seeing
	public float AI_attackDistance  = 3;				//The distance the NPC stands away from the target and attacks

	public Transform AI_StartPoint 	= null;				//Sets the first navPoint, defaults to stationary
	public float AI_Speed 			= 5;				//Casual Speed of the NPC
	public float AI_RunSpeed 		= 8;				//Scared, Charging, Aggressive Speed of the NPC



	public bool AI_seesTarget 		= false;			//if the Player has been spotted
	public GameObject AI_target		= null;				//the transform of the player
	public string AI_seekTag 		= "Player";			//The enemy Tag of this NPC
	public bool AI_attacking = false;					//if the AI is attacking






}
