using System;
using System.Collections;
using UnityEngine;
using Scaleform;



public class gameHUD : Movie
{
	protected Value	swfMovie = null;
	public float health;
	
	//private SWFCamera parent = null;
	

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

	public void updateBattery () {
		Value bBar = swfMovie.GetMember ("bBar");
		if (bBar != null) {
			SFDisplayInfo dInfo = bBar.GetDisplayInfo();

			if(dInfo == null) return;
			dInfo.XScale = health;

		}
	}
}	