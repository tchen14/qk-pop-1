using UnityEngine;
using System.Collections;
using SimpleJSON;

<<<<<<< HEAD
public class EventManager : MonoBehaviour {
    #region singletonEnforcement

    protected static EventManager instance;

    //is this even necessary?
    /*private EventManager()
    {
    }*/

    public static EventManager Instance {
        get {
            if (instance == null)
                instance = new EventManager();
            return instance;
        }
    }
    #endregion
=======
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
>>>>>>> feature/CharacterController
}
