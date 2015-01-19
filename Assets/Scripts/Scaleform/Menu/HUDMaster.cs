//This is the master for the in game UI
//this is used to store variables and manage HUD changes

using UnityEngine;
using System.Collections;

public class HUDMaster : MonoBehaviour {

	public static HUDMaster Instance;

	public bool hudActive;
	public float curBattery;
	public float curCompassPos;
	public float curMapX = 55;
	public float curMapY = 340;
	public int curAbility = 1;
	public bool abilitySwapping = false;

	void Awake () {
		Instance = this;
		hudActive = false;
	}
	

}
