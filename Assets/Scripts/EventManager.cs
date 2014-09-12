using UnityEngine;
using System.Collections;
using SimpleJSON;

public class EventManager : MonoBehaviour
{
	#region EventManagerEnforcement 
	
	private static EventManager instance;
	
	private EventManager()
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
	
	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
