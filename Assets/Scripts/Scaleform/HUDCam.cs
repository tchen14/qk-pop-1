using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using Scaleform;

//SF 4.2 Intergration doc page 5. This script Extends SFCamera
public class HUDCam : SFCamera {
	
	//Ref of the SWF to be loaded.
	public gameHUD mySWF = null;
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
		if (mySWF == null){
			SFMovieCreationParams creationParams = CreateMovieCreationParams("gameHUD.swf");
			creationParams.TheScaleModeType = ScaleModeType.SM_ShowAll;
			creationParams.IsInitFirstFrame = false;
			//mySWF = new gameHUD(this, SFMgr, creationParams);
		}
		
	}
}