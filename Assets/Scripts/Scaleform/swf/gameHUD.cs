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
		updateObjective ("Goal: Fix the SVN");
		updateBattery (78f);
		setCompassP1 (250f);
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

	//hard sets x of ability bar
	public void slideAbility (bool right, float newX) {
		Value bBar = swfMovie.GetMember ("abilityBar");
		if (bBar != null) {
			SFDisplayInfo dInfo = bBar.GetDisplayInfo();
			
			if(dInfo == null) return;
			
			if(right){
				dInfo.X  += 45;
			}
			else{
				dInfo.X  -= 45;
			}
			bBar.SetDisplayInfo(dInfo);
			
		}
	}

	//for lerping ability bar x
	public void slideAbilitySmooth (float newX) {
		Value bBar = swfMovie.GetMember ("abilityBar");
		if (bBar != null) {
			SFDisplayInfo dInfo = bBar.GetDisplayInfo();
			
			if(dInfo == null) return;
			
			dInfo.X = newX;
			bBar.SetDisplayInfo(dInfo);
			
		}
	}

	//call this to move minimap
	public void updateMap (float mapX, float mapY) {
		Value miniMap = swfMovie.GetMember ("miniMap");
		if (miniMap != null) {
			SFDisplayInfo dInfo = miniMap.GetDisplayInfo();
			
			if(dInfo == null) return;
			
			//this sets the x and y position of the minimap
			dInfo.X = (double)mapX;
			dInfo.Y = (double)mapY;
			miniMap.SetDisplayInfo(dInfo);
			
		}
	}

	//call this to update objective display text at top of the HUD, the passed string becomes the new objective
	public void updateObjective (string objective) {
		swfMovie.Invoke ("updateObjective", objective);
	}

	//when this is called the little charge "bloop" plays around the charge gauge indicating the level has changed
	public void chargeGraphic () {
		swfMovie.Invoke ("chargePulse");
	}

	//set compass objective indicator 1
	public void setCompassP1 (float x) {
		Value p1 = swfMovie.GetMember ("P1");
		float y = 59;

		if (p1 != null) {
			SFDisplayInfo dInfo = p1.GetDisplayInfo();
			
			if(dInfo == null) return;
			
			//this sets x value of the position of compass point 1, 150 is the farthest left it should go and 400 is the farthest right
			//float xBegin = 150f;
			//float xEnd = 400f;
			float curX = 0;
			if(x < 250f){
				curX = 250 - x;
				curX = curX/125;
				y = 41 + 18*curX;


			}
			if(x >= 250f){
				curX = x - 250;
				curX = curX/125;
				y = 41 + 18*curX;
			}
			dInfo.X = (double)x;
			dInfo.Y = (double)y;
			p1.SetDisplayInfo(dInfo);
			
		}
	}


}	