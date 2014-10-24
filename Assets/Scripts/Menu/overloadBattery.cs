using UnityEngine;
using System.Collections;

public class overloadBattery : MonoBehaviour {

	public gameHUD mCam;
	
	bool liveBattTest = false;
	float testBatt = 6f;

	void Start () {
		Camera.main.GetComponent<HUDCam> ().updateBatterySmooth (78f, 10f);
	}
	void Update () {
		if(liveBattTest){
			if (testBatt <= 78f) {
				Camera.main.GetComponent<HUDCam>().updateBattery (testBatt);
				testBatt += 0.1f;
			}
			else{
				testBatt = 6f;
			}
		}
	}
}
