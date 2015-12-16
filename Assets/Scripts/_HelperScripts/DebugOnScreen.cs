using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugOnScreen : MonoBehaviour{
	private static List<string> debugMessage = new List<string>();
	private static List<float> debugTime = new List<float>();
	
	public static void Log(string newMessage)
	{
		debugMessage.Add(newMessage);
		debugTime.Add(Time.time + 10);
		Debug.Log(debugMessage);
	}
	
	public static void LogError(string newMessage)
	{
		debugMessage.Add(newMessage);
		debugTime.Add(Time.time + 10);
		Debug.Log(debugMessage);
	}
	
	public void Update(){
		if (Input.GetKeyDown(KeyCode.F12))
		{
			PlayerPrefs.DeleteAll();
		}
	}
	
	private void OnGUI()
	{
		if (debugMessage.Count > 0)
		{
			if (Time.time > debugTime[0])
			{
				debugMessage.RemoveAt(0);
				debugTime.RemoveAt(0);
			}
            GUI.Box(new Rect(0, 0, 400, debugMessage.Count * 20), "");
        }
		for (int i = 0; i < debugMessage.Count; i++)
		{
			GUI.Label(new Rect(5,i * 20,1000,200), debugMessage[i]);
		}
	}
}
