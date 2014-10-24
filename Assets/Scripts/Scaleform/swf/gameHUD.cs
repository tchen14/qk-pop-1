/*---------------------------------*/
/*do not access these public functions to change HUD Info, call the ones in HUDCam instead */
/*---------------------------------*/

using System;
using System.Collections;
using UnityEngine;
using Scaleform;



public class gameHUD : Movie
{

	protected Value	swfMovie = null;
	//private SWFCamera parent = null;
	public bool liveBattTest = true;


	public gameHUD(HUDCam parent, SFManager sfmgr, SFMovieCreationParams cp) :
	base(sfmgr, cp){
		//this.parent = parent;
		SFMgr = sfmgr;
		this.SetFocus(true);
	}
	
	public void OnRegisterSWFCallback(Value swfRef){
		swfMovie = swfRef;
		//Debug.Log ("SWF Callback!");
	}

	//used for testing all updates with default values
	public void testUpdates () {
		updateObjective ("Goal: Give Anthony 20 dollars");
		updateBattery (78f);
	}

	//call this to update battery display
	public void updateBattery (float battLevel) {
		Value bBar = swfMovie.GetMember ("bBar");
		if (bBar != null) {
			SFDisplayInfo dInfo = bBar.GetDisplayInfo();

			if(dInfo == null) return;

			//this sets the scale on the x axis(the width) of the battery bar,the max width is 78, adjust math acoordingly
			dInfo.XScale = (double)battLevel;
			bBar.SetDisplayInfo(dInfo);

		}
	}

	//call this to update objective display text at top of the HUD, the passed string becomes the new objective
	public void updateObjective (string objective) {
		swfMovie.Invoke ("updateObjective", objective);
	}

	public void chargeGraphic () {
		swfMovie.Invoke ("showChargeGraphic");
	}


}	