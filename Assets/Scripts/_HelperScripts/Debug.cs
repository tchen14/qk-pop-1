using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*!
    \brief Class is for displaying selective debug logging.
    Using a .json file (located at /Assets/Scripts/_HelperScripts/logKeys.json) for key values, the editor window can be has toggle values that will show the log messages from each category.
    Place the code "using Debug = CSGS.Debug;" at the top of a script to use this version of Debug
*/
namespace FFP {
#if UNITY_EDITOR
    [SerializeField]
    public class Debug : EditorWindow {
#else
	public class Debug {
#endif
    	//! Path to the json file required to display debug logs
		const string JSONPATH = "/Scripts/_HelperScripts/debugKeys.json";
		
		//! \cond doxygen_ignore
        public List<bool> logBools = new List<bool>();
		[SerializeField]
        public static bool enabled = true, all = true;
    	//! \endcond
#if UNITY_EDITOR
			
		//! \cond doxygen_ignore
		static List<string> logStrings = new List<string>();
		//! \endcond
			
        // Add menu named "My Window" to the Window menu
        [MenuItem ("Custom Tools/DLM")]
        static void Init() {
            EditorWindow.GetWindow (typeof (Debug)).title = "Debug Log";
        }

        // This function is called any time the play button is pressed, after the OnDisable function
        void OnEnable() {
            enabled = EditorPrefs.GetBool ("enabled");
            all = EditorPrefs.GetBool ("all");
            if (logStrings.Count == 0) {
    			string text = System.IO.File.ReadAllText (Application.dataPath + JSONPATH);
                var N = JSON.Parse (text);
                logStrings.Clear();
                for (int i = 0; i < N.Count; i++)
                    logStrings.Add (N [i].Value);
            }
            logBools.Clear();
            for (int i = 0; i < logStrings.Count; i++)
                logBools.Add (EditorPrefs.GetBool (logStrings [i]));
        }

        // This function is called any time the play button is pressed, before the OnEnable function
        void OnDisable() {
            EditorPrefs.SetBool ("enabled", enabled);
            EditorPrefs.SetBool ("all", all);
            for (int i = 0; i < logStrings.Count; i++)
                EditorPrefs.SetBool (logStrings [i], logBools [i]);
        }

        // This function is called anytime the window is closed
        void OnDestroy() {
            for (int i = 0; i < logStrings.Count; i++)
                EditorPrefs.SetBool (logStrings [i], logBools [i]);
        }

        void OnGUI() {
            EditorGUILayout.Space();
            if (enabled)
                enabled = EditorGUILayout.ToggleLeft ("Logs are Enabled", enabled, EditorStyles.boldLabel);
            else
                enabled = EditorGUILayout.ToggleLeft ("Logs are Disabled", enabled, EditorStyles.boldLabel);
            EditorGUILayout.Space();
            if (enabled) {
                if (all)
                    all = EditorGUILayout.BeginToggleGroup ("Showing Selected Logs", all);
                else
                    all = EditorGUILayout.BeginToggleGroup ("Showing All Logs", all);
                EditorGUILayout.Space();
                if (logStrings.Count == 0) {
                    string text = System.IO.File.ReadAllText (Application.dataPath + "/Editor/logKeys.json");
                    var N = JSON.Parse (text);
                    logStrings.Clear();
                    logBools.Clear();
                    for (int i = 0; i < N.Count; i++) {
                        logStrings.Add (N [i].Value);
                        logBools.Add (false);
                    }
                }
                for (int i = 0; i < logBools.Count; i++)
                    logBools [i] = EditorGUILayout.Toggle (logStrings [i], logBools [i]);
                EditorGUILayout.EndToggleGroup();
            }
        }
        
		//! Function will call UnityEngine.Debug.ClearDeveloperConsole
		public static void ClearDeveloperConsole(){
			UnityEngine.Debug.ClearDeveloperConsole ();
		}
		
		//! Function will call UnityEngine.Debug.Break
		public static void Break(){
			UnityEngine.Debug.Break ();
		}
		
		//! Function will call UnityEngine.Debug.DrawLine with the message
		public static void DrawLine (string key, Vector3 start, Vector3 end, Color color = default(Color),
		                             float duration = 0.0f, bool depthTest = true) {
			if (!enabled)
				return;
			if ((!all) || (logStrings.Contains (key.ToLower()) && EditorPrefs.GetBool (key.ToLower())))
				UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
		}
		
		//! Function will call UnityEngine.Debug.DrawRay with the message
		public static void DrawRay (string key, Vector3 start, Vector3 end, Color color = default(Color),
		                            float duration = 0.0f, bool depthTest = true) {
			if (!enabled)
				return;
			if ((!all) || (logStrings.Contains (key.ToLower()) && EditorPrefs.GetBool (key.ToLower())))
				UnityEngine.Debug.DrawRay(start, end, color, duration, depthTest);
		}
		
		//! Function will call UnityEngine.Debug.Log with the message
		public static void Log (string key, object message, UnityEngine.Object context = null) {
            if (!enabled)
                return;
			if ((!all) || (logStrings.Contains (key.ToLower()) && EditorPrefs.GetBool (key.ToLower()))){
				if(context)
					UnityEngine.Debug.Log (message, context);
				else
					UnityEngine.Debug.Log (message);
			}
		}
		
		//! Function will call UnityEngine.Debug.LogError with the message
		public static void Error (string key, object message, UnityEngine.Object context = null) {
			if (!enabled)
				return;
			if ((!all) || (logStrings.Contains (key.ToLower()) && EditorPrefs.GetBool (key.ToLower()))){
				if(context)
					UnityEngine.Debug.LogError (message, context);
				else
					UnityEngine.Debug.LogError (message);
			}
		}
		
		//! Function will call UnityEngine.Debug.LogError with the message
		public static void Exception (string key, Exception exception, UnityEngine.Object context = null) {
			if (!enabled)
				return;
			if ((!all) || (logStrings.Contains (key.ToLower()) && EditorPrefs.GetBool (key.ToLower()))){
				if(context)
					UnityEngine.Debug.LogException (exception, context);
				else
					UnityEngine.Debug.LogException (exception);
			}
		}
		
		//! Function will call UnityEngine.Debug.LogWarning with the message
		public static void Warning (string key, object message, UnityEngine.Object context = null) {
			if (!enabled)
				return;
			if ((!all) || (logStrings.Contains (key.ToLower()) && EditorPrefs.GetBool (key.ToLower()))){
				if(context)
					UnityEngine.Debug.LogWarning (message, context);
				else
					UnityEngine.Debug.LogWarning (message);
			}
		}
    	
#else
		public static void ClearDeveloperConsole(){
			;
		}
		public static void Break(){
			;
		}
		public static void DrawLine (string key, Vector3 start, Vector3 end, Color color = default(Color),
		                             float duration = 0.0f, bool depthTest = true) {
			;
		}
		public static void DrawRay (string key, Vector3 start, Vector3 end, Color color = default(Color),
		                            float duration = 0.0f, bool depthTest = true) {
			;
		}
		public static void Log (string key, string message, UnityEngine.Object context = null) {
            ;
        }
		public static void Error (string key, string message, UnityEngine.Object context = null) {
    		;
		}
		public static void Exception (string key, Exception exception, UnityEngine.Object context = null) {
			;
		}
		public static void Warning (string key, string message, UnityEngine.Object context = null) {
			;
		}
#endif
    }
}
