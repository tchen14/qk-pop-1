using UnityEngine;
using System.Collections;

public class MenuSceneManager : SceneManager
{
	
	void Start()
	{
		//this code is to load the initial scene. Because Unity uses the first scene in the Build Settings, this code isn't really needed unless MasterManager.cs is changed to properly utilize additive scene loading
		if (!MasterManager.Instance.init) {
			MasterManager.Instance.ToggleLevel("Menu");
			MasterManager.Instance.LoadLevels();
		}
	}

	//testing code until we get Scaleform UI fully working
	void OnGUI () {
		// Make a background box
		GUI.Box(new Rect(10,10,100,90), "Testing Menu");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,80,20), "Load PoP")) {
			StartPoPScene();
		}
	}

	void Update()
	{
	
	}
	
	void StartPoPScene()
	{
		MasterManager.Instance.ToggleLevel("PoP");
		MasterManager.Instance.LoadLevels();
	}
}
