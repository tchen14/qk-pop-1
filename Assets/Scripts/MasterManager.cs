using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Debug = FFP.Debug;

//! Master manager of the game. This manager manages all other (significant) managers.
public static class MasterManager {
	
	const string FIRSTSCENE = "Menu";
	
	[RuntimeInitializeOnLoadMethod]
	static void OnApplicationStart() {
		//Debug.Log("core", "OnApplicationStart");
		//todo: set MasterSceneManager to have the defaul input type
		//todo: set InputManager to have the defaul input type
		GameObject go;
		
		go = new GameObject();
		go.AddComponent<MasterSceneManager>();
		go.name = "_MasterSceneManager";
		ObjectManager.AddSavedObject(go.transform);
		
		go = new GameObject();
		go.AddComponent<ObjectManager>();
		go.name = "_ObjectManager";
		ObjectManager.AddSavedObject(go.transform);
		
		go = new GameObject();
		go.AddComponent<InputManager>();
		go.name = "_InputManager";
		ObjectManager.AddSavedObject(go.transform);
	}
	
	#if BUILD
	[RuntimeInitializeOnLoadMethod]
	static void OnBuiltApplicationStart() {
		MasterSceneManager.Instance.QuickLoadLevel (FIRSTSCENE);
	}
	#endif
	
	#if !BUILD
	[RuntimeInitializeOnLoadMethod]
	static void OnEditorApplicationStart() {
		ObjectManager.LoadTopObjects();
	}
	#endif
}