using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Debug = FFP.Debug;

/*
 *  Master manager of the game. This manager manages all other (significant) managers.
 */
public sealed class MasterManager {
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

	/*!
	 *	Holds all pending scenes to be activated.
	 *	Keys are based on json file (/Assets/Resources/Json/sceneList.json) and are set in the constructor.
	 *	Values are false by default.
	 */
    public Dictionary<string,bool> scenes = new Dictionary<string,bool>();
    bool _init = false;
    public bool init {
        get {
            return _init;
        } set {
            _init = true;
        }
    }

	//! Class constructor; populates Dictionary variable scenes with values (all false) with keys based on .json file (/Assets/Resources/Json/sceneList.json)
    private MasterManager() {
        string text = System.IO.File.ReadAllText (Application.dataPath + "/Resources/Json/sceneList.json");
        var N = JSON.Parse (text);
        scenes.Clear();
        for (int i = 0; i < N.Count; i++)
            scenes.Add (N [i].Value, false);
        Debug.Log ("core", "Scenes dictionary loaded");
    }

	//! Clears Dictionary variable scenes of all values
    private void ClearAllLevels() {
        List<string> buffer = new List<string> (scenes.Keys);
        foreach (string key in buffer)
            scenes [key] = false;
    }

	//! Toggles the level on/off in Dictionary variable scenes
    public void ToggleLevel (string level, bool b = true) {
        if (scenes.ContainsKey (level))
            scenes [level] = b;
        else
            Debug.Warning ("core", "Level does not exist.");
    }

    //! This will load a single level. There is no additive level loading option.
    public void QuickLoadLevel(string level) {
        if (!init)
            init = true;
        Debug.Log ("core", "Loading level " + level);
        Application.LoadLevel (level);
        ClearAllLevels();
    }

    /*
     * Load all levels marked "true" in Dictionary variable scenes.
     * Uninteligent method - it will unload and reload all levels.
     * This function can load multiple levels additively.
     */
    public void LoadLevels() {
        if (!init)
            init = true;
        if (!scenes.ContainsValue (true))
            Debug.Log ("core", "No scenes to be loaded. Please ensure valid scenes are ready to be loaded in Assets/Resources/Json/sceneList.json");
        List<string> levels = new List<string>();
        foreach (KeyValuePair<string,bool> k in scenes) {
            if (k.Value)
                levels.Add (k.Key);
        }
        for (int i = 0; i < levels.Count; i++) {
            if (i == 0) {
                Debug.Log ("core", "Loading level " + levels [i]);
                Application.LoadLevel (levels [i]);
            } else {
                Debug.Log ("core", "Loading level additive" + levels [i]);
                Application.LoadLevelAdditive (levels [i]);
            }
        }
        ClearAllLevels();
    }
}