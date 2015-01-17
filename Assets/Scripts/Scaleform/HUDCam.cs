using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using Scaleform;

//SF 4.2 Intergration doc page 5. This script Extends SFCamera
public class HUDCam : SFCamera {
	
	//Ref of the SWF to be loaded.
	public gameHUD hudRef = null;
	public string swfMovie;
	////////////////



	new public void Awake(){
		
	}
	
	// Hides the Start function in the base SFCamera. Will be called every time the ScaleformCamera (Main Camera game object)
	// is created. Use new and not override, since return type is different from that of base::Start()
	new public  IEnumerator Start(){
		// The eval key must be set before any Scaleform related classes are loaded, other Scaleform Initialization will not 
		// take place.
		#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WP8
		SF_SetKey("MC6PZR3PFKB4RWWTNRCUNF2EA4BCMJ8ZO5GAP7NQLSN0BCJSROC8R7DR1DTCMS5");
		#elif UNITY_IPHONE
		SF_SetKey("");
		#elif UNITY_ANDROID
		SF_SetKey("");
		#elif UNITY_WP8
		sf_setKey("");
		#endif
		//For GL based platforms - Sets a number to use for Unity specific texture management.  Adjust this number if
		//you start to experience black and/or mssing textures.
		#if UNITY_WP8 
		sf_setTextureCount(500);
		#else
		SF_SetTextureCount(500);
		#endif
		
		InitParams.TheToleranceParams.Epsilon = 1e-5f;
		InitParams.TheToleranceParams.CurveTolerance = 1.0f; 
		InitParams.TheToleranceParams.CollinearityTolerance = 10.0f;
		InitParams.TheToleranceParams.IntersectionEpsilon = 1e-3f;
		InitParams.TheToleranceParams.FillLowerScale = 0.0707f;
		InitParams.TheToleranceParams.FillUpperScale = 100.414f;
		InitParams.TheToleranceParams.FillAliasedLowerScale = 10.5f;
		InitParams.TheToleranceParams.FillAliasedUpperScale = 200.0f;
		InitParams.TheToleranceParams.StrokeLowerScale = 10.99f;
		InitParams.TheToleranceParams.StrokeUpperScale = 100.01f;
		InitParams.TheToleranceParams.HintedStrokeLowerScale = 0.09f;
		InitParams.TheToleranceParams.HintedStrokeUpperScale = 100.001f;
		InitParams.TheToleranceParams.Scale9LowerScale = 10.995f;
		InitParams.TheToleranceParams.Scale9UpperScale = 100.005f;
		InitParams.TheToleranceParams.EdgeAAScale = 0.95f;
		InitParams.TheToleranceParams.MorphTolerance = 0.001f;
		InitParams.TheToleranceParams.MinDet3D = 10.001f;
		InitParams.TheToleranceParams.MinScale3D = 10.05f;
		InitParams.UseSystemFontProvider = false;
		return base.Start();
	}
	
	// Application specific code goes here
	new public void Update(){
		CreateHud();
		base.Update ();

	}
	
	private void CreateHud(){
		if (hudRef == null){
			SFMovieCreationParams creationParams = CreateMovieCreationParams("gameHUD.swf");
			creationParams.TheScaleModeType = ScaleModeType.SM_ShowAll;
			creationParams.IsInitFirstFrame = false;
			hudRef = new gameHUD(this, SFMgr, creationParams);

			// easy indicator for if the HUD is loaded
			Camera.main.GetComponent<HUDMaster>().hudActive = true;
		}
		
	}

	//CALL THESE FUNCTIONS TO UPDATE HUD INFORMATION
	//IMPORTANT Never call any of these in Start or Awake from a script that instantiates at the same
	//time as the main camera ex: anything in the scene at the start, the swf won't be loaded in time and
	//you will get a null reference exception

	//this is for testing, use updateBatterySmooth in final scripts
	public void updateBattery(float batt){
		hudRef.updateBattery (batt);
	}

	//use this when you want to change what percent the bettery bar is displaying
	//pass curBattery from HUDmaster as curBatt
	public void updateBatterySmooth (float duration, float curBatt, float newBatt) {
		StartCoroutine (lerpBattery (duration, curBatt, newBatt));
	}

	//use this to update the objective text
	public void updateObjective (string objective) {
		hudRef.updateObjective (objective);
	}

	// use this to set the compass objective indicator's position
	public void setCompass (float duration, float curPos, float newPos) {
		StartCoroutine (setCompassP1 (duration, curPos, newPos));
	}

	//use this to set map X and Y
	public void updateMapPos (float x, float y) {
		hudRef.updateMap (x, y);
	}

	//use this to slide ability bar left or right
	public void slideAbilityBar (bool right, int ability) {
		//checks if ability is changing
		if(HUDMaster.Instance.abilitySwapping == false){
			HUDMaster.Instance.abilitySwapping = true;
			//finds new x position for the ability bar
			float newX = 335 + ((float)ability * 45);
			float curX = 335 + ((float)HUDMaster.Instance.curAbility * 45);

			StartCoroutine(lerpAbility(.5f,curX, newX));
			HUDMaster.Instance.curAbility = ability;
		}
	}



	//These coroutines are called by their coresponding functions, this is for convenience, don't call these
	private IEnumerator lerpBattery (float duration, float startP, float endP) {

		if (endP > startP){
			duration *= (endP - startP) / 72;
		}
		else {
			duration *= (startP - endP) / 72;
		}

		float start = Time.time;
		float elapsed = Time.time - start;
		do
		{  
			elapsed = Time.time - start;
			float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
			float newBatt = Mathf.Lerp(startP, endP, normalisedTime);
			hudRef.updateBattery(newBatt);
			HUDMaster.Instance.curBattery = newBatt;
			yield return null;
		}
		while(elapsed < duration);
		hudRef.chargeGraphic();
	}



	private IEnumerator setCompassP1( float duration, float startP, float endP)
	{  
		if (endP > startP){
			duration *= (endP - startP) / 250;
		}
		else {
			duration *= (startP - endP) / 250;
		}

		float start = Time.time;
		float elapsed = Time.time - start;
		do
		{  
			elapsed = Time.time - start;
			float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
			float newPos = Mathf.Lerp(startP, endP, normalisedTime);
			hudRef.setCompassP1(newPos);
			HUDMaster.Instance.curCompassPos = newPos;
			yield return null;
		}
		while(elapsed < duration);
	}

	private IEnumerator lerpAbility (float duration, float startP, float endP) {

		float start = Time.time;
		float elapsed = Time.time - start;
		do
		{  
			elapsed = Time.time - start;
			float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
			float newX = Mathf.Lerp(startP, endP, normalisedTime);
			hudRef.slideAbilitySmooth(newX);
			yield return null;
		}
		while(elapsed < duration);
		HUDMaster.Instance.abilitySwapping = false;

	}
	


}




