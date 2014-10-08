using System;
using System.Collections;
using UnityEngine;
using Scaleform;


public class mainMenu2 : Movie
{
	protected Value	swfMovie = null;
	//private SWFCamera parent = null;
	
	
	
	public mainMenu2(SWFCamera parent, SFManager sfmgr, SFMovieCreationParams cp) :base(sfmgr, cp){
		//this.parent = parent;
		SFMgr = sfmgr;
		this.SetFocus(true);
	}
	
	public void OnRegisterSWFCallback(Value swfRef){
		swfMovie = swfRef;
	}

	public void goToScene(string sceneName) {
		Application.LoadLevel (sceneName);
	}
	
}	