using System;
using System.Collections;
using UnityEngine;
using Scaleform;



public class mainMenu2 : Movie
{
	protected Value	swfMovie = null;


	//private SWFCamera parent = null;
	
	
	public class  options {
		public double gameVolume { get; set; }
	}
	public mainMenu2(SWFCamera parent, SFManager sfmgr, SFMovieCreationParams cp) :
		base(sfmgr, cp){
		//this.parent = parent;
		SFMgr = sfmgr;
		this.SetFocus(true);
	}
	
	public void OnRegisterSWFCallback(Value swfRef){
		swfMovie = swfRef;


		Debug.Log ("SWF Callback!");

	}

	public void goToScene() {
		Application.LoadLevel ("testHUD");
	}

	public void updateSoundSettings(double v1, double v2, double v3){
		PlayerPrefs.SetFloat ("gameVolume", (float)v1);
		PlayerPrefs.SetFloat ("musicVolume", (float)v2);
		PlayerPrefs.SetFloat ("effectsVolume", (float)v3);
	}

	public void updateGameSettings(string diff){
		PlayerPrefs.SetString ("difficulty", diff);
	}

	public void updateVideoSettings(double FoV, double quality){


		PlayerPrefs.SetFloat ("gameFoV", (float)FoV);
		PlayerPrefs.SetInt ("videoQuality", (int)quality);

		Camera.main.fieldOfView = (PlayerPrefs.GetFloat ("gameFoV") * 5) + 40;
		QualitySettings.SetQualityLevel (PlayerPrefs.GetInt ("videoQuality"), true);
	}

	public void updateSWFOptions () {
		//options csOptions = swfMovie.ConvertFromASObject(typeof(options)) as options;
		//csOptions.gameVolume  = 10;
		swfMovie.Invoke ("updateDiff", PlayerPrefs.GetString("difficulty"));
		swfMovie.Invoke ("updateSound", (double)PlayerPrefs.GetFloat("gameVolume"), 
		                				(double)PlayerPrefs.GetFloat("musicVolume"), 
		                				(double)PlayerPrefs.GetFloat("effectsVolume"));
		swfMovie.Invoke ("updateVideo", (double)PlayerPrefs.GetFloat("gameFoV"), (double)PlayerPrefs.GetInt("videoQuality"));

	}
	
}	