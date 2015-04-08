using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleJSON;

public class ScriptBuild {
	[MenuItem("Custom Tools/Build Game (Windows)")]
	public static void BuildGame ()
	{
		string[] scenes = GetSceneList();
		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
		
		// Build player.
		BuildPipeline.BuildPlayer(scenes, path + "/Quantum Keeper.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
		
		// Copy a file from the project folder to the build folder, alongside the built game.
		// eg: FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
		
		// Run the game (Process class from System.Diagnostics).
		Process proc = new Process();
		proc.StartInfo.FileName = path + "Quantum Keeper.exe";
		proc.Start();
	}
	
	private static string[] GetSceneList(){
		string text = System.IO.File.ReadAllText (Application.dataPath + "/Resources/Json/sceneList.json");
		var N = JSON.Parse (text);
		string[] scenes = new string[N.Count];
		for (int i = 0; i < N.Count; i++)
			scenes[i] = "Assets/Scenes/" + N[i].Value + ".unity";
		return scenes;
	}
}