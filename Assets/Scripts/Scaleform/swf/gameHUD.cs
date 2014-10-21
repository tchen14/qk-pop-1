using System;
using System.Collections;
using UnityEngine;
using Scaleform;



public class gameHUD : Movie
{
	protected Value	swfMovie = null;
	
	
	//private SWFCamera parent = null;
	

	public gameHUD(SWFCamera parent, SFManager sfmgr, SFMovieCreationParams cp) :
	base(sfmgr, cp){
		//this.parent = parent;
		SFMgr = sfmgr;
		this.SetFocus(true);
	}
	
	public void OnRegisterSWFCallback(Value swfRef){
		swfMovie = swfRef;
		
		
		Debug.Log ("SWF Callback!");
	}
	

	
}	