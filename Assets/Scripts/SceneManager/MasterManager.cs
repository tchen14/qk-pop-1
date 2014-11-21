using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

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

    public Dictionary<string,bool> scenes = new Dictionary<string,bool>();
    bool _init = false;
    public bool init {
        get {
            return _init;
        } set {
            _init = true;
        }
    }
    private MasterManager() {
        string text = System.IO.File.ReadAllText (Application.dataPath + "/Resources/Json/sceneList.json");
        var N = JSON.Parse (text);
        scenes.Clear();
        for (int i = 0; i < N.Count; i++)
            scenes.Add (N [i].Value, false);
        Log.M ("core", "Scenes dictionary loaded");
    }

    private void ClearAllLevels() {
        List<string> buffer = new List<string> (scenes.Keys);
        foreach (string key in buffer)
            scenes [key] = false;
    }

    public void ToggleLevel (string level, bool b = true) {
        if (scenes.ContainsKey (level))
            scenes [level] = b;
        else
            Log.W ("core", "Level does not exist.");
    }

    //load all levels marked "true" in scenes Dictionary
    //uninteligent method - it will unload and reload all levels
    public void LoadLevels() {
        if (!init)
            init = true;
        if (!scenes.ContainsValue (true))
            Log.M ("core", "No scenes to be loaded. Please ensure valid scenes are ready to be loaded in Assets/Resources/Json/sceneList.json");
        List<string> levels = new List<string>();
        foreach (KeyValuePair<string,bool> k in scenes) {
            if (k.Value)
                levels.Add (k.Key);
        }
        for (int i = 0; i < levels.Count; i++) {
            if (i == 0) {
                Log.M ("core", "Loading level " + levels [i]);
                Application.LoadLevel (levels [i]);
            } else {
                Log.M ("core", "Loading level additive" + levels [i]);
                Application.LoadLevelAdditive (levels [i]);
            }
        }
        ClearAllLevels();
    }
}