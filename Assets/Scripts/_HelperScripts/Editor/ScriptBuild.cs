using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleJSON;
using System.IO;

public class ScriptBuild {

	static string path = Application.dataPath.Remove(Application.dataPath.Length - 8, 7) + "_build";
	
	[MenuItem("Custom Tools/Build Game (Mac)")]
	public static void BuildGameMac() {
		BuildGame(BuildTarget.StandaloneOSXIntel64, BuildOptions.None);
		
		// Run the game (Process class from System.Diagnostics)
		Process proc = new Process();
		proc.StartInfo.FileName = path + "/" + StringManager.BUILDNAME + ".app";
		proc.Start();
	}

	[MenuItem("Custom Tools/Build Game (Windows)")]
	public static void BuildGameWindows() {
		BuildGame(BuildTarget.StandaloneWindows, BuildOptions.None);
		
		// Run the game (Process class from System.Diagnostics)
		Process proc = new Process();
		proc.StartInfo.FileName = path + "/" + StringManager.BUILDNAME + ".exe";
		proc.Start();
	}
	
	public static void BuildGame(BuildTarget buildTarget, BuildOptions buildOptions = BuildOptions.None) {
		if (BuildPipeline.isBuildingPlayer == false) {
			UnityEngine.Debug.ClearDeveloperConsole();
			
			var smcsFile = Path.Combine( Application.dataPath, "smcs.rsp" );
			var gmcsFile = Path.Combine( Application.dataPath, "gmcs.rsp" );
			
			// -define:debug;poop
			File.WriteAllText( smcsFile, "-define:BUILD" );
			File.WriteAllText( gmcsFile, "-define:BUILD" );
			
			AssetDatabase.Refresh();
			
			string[] scenes = GetSceneList();
			//string path = Application.dataPath.Remove(Application.dataPath.Length - 8, 7) + "_build";
			
			// Build player.
			BuildPipeline.BuildPlayer(scenes, path + "/" + StringManager.BUILDNAME + ".exe", buildTarget, buildOptions);
			
			// Copy a file from the project folder to the build folder, alongside the built game. eg:
			// FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
			
			
			//remove build thingies
			File.WriteAllText( smcsFile, "" );
			File.WriteAllText( gmcsFile, "" );
		}
	}

	private static string[] GetSceneList() {
		string text = System.IO.File.ReadAllText(Application.dataPath + "/Resources/sceneList.json");
		var N = JSON.Parse(text);
		string[] scenes = new string[N.Count];
		for (int i = 0; i < N.Count; i++){
			scenes[i] = "Assets/Scenes/" + N[i].Value + ".unity";
		}
		return scenes;
	}
}