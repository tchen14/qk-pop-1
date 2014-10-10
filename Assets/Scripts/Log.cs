using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
#if UNITY_EDITOR
using UnityEditor;
#endif

[SerializeField]
public class Log : EditorWindow
{
	[SerializeField]
	public static List<string>
		logStrings = new List<string>();
	
	//[SerializeField]
	public List<bool>
		logBools = new List<bool>();
	
	[SerializeField]
	public static bool
		enabled = true, all = true;
	
	#if UNITY_EDITOR
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Debug Log Manager")]
	static void Init()
	{
		EditorWindow.GetWindow(typeof(Log)).title = "Debug Log Messages";
	}
	
	//this function is called any time the play button is pressed, after the OnDisable function
	void OnEnable()
	{
		enabled = EditorPrefs.GetBool("enabled");
		all = EditorPrefs.GetBool("all");

		if (logStrings.Count == 0) {
			string text = System.IO.File.ReadAllText(Application.dataPath + "/Editor/logKeys.json");
			var N = JSON.Parse(text);
			
			logStrings.Clear();
			for (int i = 0; i < N.Count; i++) {
				logStrings.Add(N [i].Value);
			}
		}
		
		logBools.Clear();
		for (int i = 0; i < logStrings.Count; i++) {
			logBools.Add(EditorPrefs.GetBool(logStrings [i]));
		}
	}
	
	//this function is called any time the play button is pressed, before the OnEnable function
	void OnDisable()
	{
		EditorPrefs.SetBool("enabled", enabled);
		EditorPrefs.SetBool("all", all);

		for (int i = 0; i < logStrings.Count; i++) {
			EditorPrefs.SetBool(logStrings [i], logBools [i]);
		}
	}
	
	//this function is called anytime the window is closed
	void OnDestroy()
	{
		for (int i = 0; i < logStrings.Count; i++) {
			EditorPrefs.SetBool(logStrings [i], logBools [i]);
		}
	}
	
	void OnGUI()
	{
		EditorGUILayout.Space();
		if (enabled) {
			enabled = EditorGUILayout.ToggleLeft("Logs are Enabled", enabled, EditorStyles.boldLabel);
		} else {
			enabled = EditorGUILayout.ToggleLeft("Logs are Disabled", enabled, EditorStyles.boldLabel);
		}
		EditorGUILayout.Space();
		
		if (enabled) {
			if (all) {
				all = EditorGUILayout.BeginToggleGroup("Showing Selected Logs", all);
			} else {
				all = EditorGUILayout.BeginToggleGroup("Showing All Logs", all);
			}
			EditorGUILayout.Space();
			
			if (logStrings.Count == 0) {
				string text = System.IO.File.ReadAllText(Application.dataPath + "/Editor/logKeys.json");
				var N = JSON.Parse(text);
				
				logStrings.Clear();
				logBools.Clear();
				for (int i = 0; i < N.Count; i++) {
					logStrings.Add(N [i].Value);
					logBools.Add(false);
				}
			}
			
			for (int i = 0; i < logBools.Count; i++) {
				logBools [i] = EditorGUILayout.Toggle(logStrings [i], logBools [i]);
			}
			EditorGUILayout.EndToggleGroup();
		}
		
		//enabled = EditorGUILayout.BeginToggleGroup ("Enable Debug Logs", enabled);
		//EditorGUILayout.EndToggleGroup ();
		//myString = EditorGUILayout.TextField ("Text Field", myString);
		//myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
	}
	#endif
	
	public static void M(string key, string message)
	{
		Message(key, message);
	}
	
	public static void W(string key, string message)
	{
		Warning(key, message);
	}
	
	public static void E(string key, string message)
	{
		Error(key, message);
	}
	
	#if UNITY_EDITOR
	public static void Message(string key, string message)
	{
		if (!enabled) {
			return;
		}
		
		if ((!all) || (logStrings.Contains(key) && EditorPrefs.GetBool(key))) {
			Debug.Log(message);
		}
	}
	
	public static void Warning(string key, string message)
	{
		if (!enabled) {
			return;
		}
		
		if ((!all) || (logStrings.Contains(key) && EditorPrefs.GetBool(key))) {
			Debug.LogWarning(message);
		}
	}
	
	public static void Error(string key, string message)
	{
		if (!enabled) {
			return;
		}
		
		if ((!all) || (logStrings.Contains(key) && EditorPrefs.GetBool(key))) {
			Debug.LogError(message);
		}
	}
	#else
	public static void Message(string key, string message){
		;
	}
	
	public static void Warning(string key, string message){
		;
	}
	
	public static void Error(string key, string message){
		;
	}
	#endif
}
