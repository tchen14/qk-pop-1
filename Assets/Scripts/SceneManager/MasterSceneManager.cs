using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Debug = FFP.Debug;

//! Master manager of the game. This manager manages all other (significant) managers.
public sealed class MasterSceneManager : MonoBehaviour {
	
	const string SCENELISTPATH = "/Resources/Json/sceneList.json";
	private static List<string> scenes = new List<string>();				//<! Holds all avaiable scenes
	
	#region singletonEnforcement
	private static MasterSceneManager instance;
	
	public static MasterSceneManager Instance {
		get {
			return instance;
		}
	}
	#endregion
	
	void Start() {
		#region singletonEnforcement
		if (instance == null)
			instance = this;
		else {
			Destroy(this.gameObject);
			Debug.Error("core", "Second MasterSceneManager detected. Deleting gameOject.");
			return;
		}
		#endregion
		
		string text = System.IO.File.ReadAllText(Application.dataPath + SCENELISTPATH);
		var N = JSON.Parse(text);
		scenes.Clear();
		for (int i = 0; i < N.Count; i++)
			scenes.Add(N[i].Value);
		Debug.Log("core", "Scenes dictionary loaded");
	}
	
	//! This will load a single level if it exists in the json file. This will clear all other objects
	public void QuickLoadLevel(string levelName) {
		ObjectManager.RemoveAllTopObjects();
		Instance.StartCoroutine(COLoadLevel(levelName, false));
	}
	
	//! Load level if it exists in the json file.
	public void LoadLevel(string levelName) {
		if (!scenes.Contains(levelName))
			Debug.Log("core", "No scenes to be loaded. Please ensure valid scenes are ready to be loaded in Assets/Resources/Json/sceneList.json");
		Instance.StartCoroutine(COLoadLevel(levelName, true));
	}
	
	IEnumerator COLoadLevel(string levelName, bool additive = true) {
		if (additive)
			Application.LoadLevelAdditiveAsync(levelName);
		else
			Application.LoadLevelAsync(levelName);
		
		yield return new WaitForEndOfFrame();
		ObjectManager.LoadTopObjects();
		Debug.Log("core", "Level " + levelName + " loaded\nAdditive = " + additive + ".");
	}
}
