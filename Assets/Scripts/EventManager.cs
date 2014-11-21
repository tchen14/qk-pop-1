using UnityEngine;
using System.Collections;
using SimpleJSON;

public class EventManager : MonoBehaviour
{
	#region EventManagerEnforcement 
	
	private static EventManager instance;
	
	public EventManager()
	{
	}
	
	public static EventManager Instance {
		get {
			if (instance == null) {
				instance = new EventManager();
			}
			return instance;
		}
	}
	#endregion
}
