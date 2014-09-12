using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class MasterManager
{
	//sealed attribute prevents other classes from inheriting this class

	#region singletonEnforcement 
	private static readonly MasterManager instance = new MasterManager();
	//readonly keyword modifier prevents variable from being writen except on declaration
	
	public static MasterManager Instance {
		get {
			return instance;
		}
	}
	#endregion

	public Dictionary<string,bool> scenes = new Dictionary<string,bool>();
	public bool init = false;
	private MasterManager()
	{
		scenes.Add("Menu", false);
		scenes.Add("PoP", false);
	}
	
	public void ToggleLevel(string level, bool b = true)
	{
		if (scenes.ContainsKey(level)) {
			scenes [level] = b;
		} else {
			Debug.LogWarning("Level does not exist.");
		}
	}
	
	//load all levels marked "true" in scenes Dictionary
	//uninteligent method - it will unload and reload all levels
	public void LoadLevels()
	{
		if (!scenes.ContainsValue(true)) {
			Debug.LogError("No scenes to be loaded. Please ensure valid scenes are ready to be loaded (MasterManager.scenes)");
		}
			
		List<string> levels = new List<string>();
		foreach (KeyValuePair<string,bool> k in scenes) {
			if (k.Value) {
				levels.Add(k.Key);
			}
		}
		
		for (int i = 0; i < levels.Count; i++) {
			if (i == 0) {
				Debug.Log("Loading level " + levels [i]);
				Application.LoadLevel(levels [i]);
			} else {
				Debug.Log("Loading level additive" + levels [i]);
				Application.LoadLevelAdditive(levels [i]);
			}
		}
	}
}