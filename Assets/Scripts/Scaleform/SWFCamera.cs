using UnityEngine;
using System;
using System.IO;
using System.Collections;
using Scaleform;

//SF 4.2 Intergration doc page 5. This script Extends SFCamera
public class SWFCamera : SFCamera {
	
	//Ref of the SWF to be loaded.
	public menuTest mySWF = null;
	
	////////////////
	
	new public void Awake(){
		
	}
	
	// Hides the Start function in the base SFCamera. Will be called every time the ScaleformCamera (Main Camera game object)
	// is created. Use new and not override, since return type is different from that of base::Start()
	new public  IEnumerator Start(){
		// The eval key must be set before any Scaleform related classes are loaded, other Scaleform Initialization will not 
		// take place.
		#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR) && !UNITY_WP8
		SF_SetKey("");
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
		
		return base.Start();
	}
	
	// Application specific code goes here
	new public void Update(){
		CreateHud();
		base.Update ();
	}
	
	private void CreateHud(){
		if (mySWF == null){
			SFMovieCreationParams creationParams = CreateMovieCreationParams("menuTest.swf");
			creationParams.IsInitFirstFrame = false;
			mySWF = new menuTest(this, SFMgr, creationParams);
		}
	}
}