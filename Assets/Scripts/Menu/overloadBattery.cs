using UnityEngine;
using System.Collections;

public class overloadBattery : MonoBehaviour {

	public gameHUD mCam;
	
	bool liveBattTest = false;
	float testBatt = 6f;

	void Awake () {
		StartCoroutine (waitTest());
	}
	void Update () {

		if (Input.GetKeyDown("1")){
			Camera.main.GetComponent<HUDCam>().slideAbilityBar(true,HUDMaster.Instance.curAbility - 1);
		}

		if (Input.GetKeyDown("2")){
			Camera.main.GetComponent<HUDCam>().slideAbilityBar(false,HUDMaster.Instance.curAbility + 1);
		}

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

	IEnumerator waitTest () {
		yield return new WaitForSeconds (2f);
		Camera.main.GetComponent<HUDCam> ().updateBatterySmooth (2, 8f, 78f);
		Camera.main.GetComponent<HUDCam> ().setCompass (2,250,150);
		yield return null;
	}
}
