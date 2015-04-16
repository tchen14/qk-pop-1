using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

public sealed class ObjectManager : MonoBehaviour {

	#region singletonEnforcement
	private static ObjectManager instance;

	public static ObjectManager Instance {
		get {
			return instance;
		}
	}

	void Start() {
		if (instance == null)
			instance = this;
		else {
			Destroy(this.gameObject);
			Debug.Error("core", "Second ObjectManager detected. Deleting gameOject.");
			return;
		}
	}
	#endregion

	public static Dictionary<string, Transform> TopObjects = new Dictionary<string, Transform>();
	public static Dictionary<string, Transform> SavedObjects = new Dictionary<string, Transform>();

	public static void LoadTopObjects() {
		Transform[] gameObjects = GameObject.FindObjectsOfType<Transform>();
		foreach (Transform t in gameObjects) {
			if (t.parent == null && !TopObjects.ContainsKey(t.name) && !SavedObjects.ContainsKey(t.name)) {
				TopObjects.Add(t.name, t);
			}
		}

		//Print Object to be loaded
#if !BUILD
		/*foreach (string s in TopObjects.Keys) {
			Debug.Log("core", TopObjects[s].name);
		}*/
#endif
	}

	public static void RemoveTopObject(string s) {
		if (TopObjects.ContainsKey(s))
			GameObject.Destroy(TopObjects[s].gameObject);
		else
			Debug.Warning("core", "TopObjects does not contain object named \'" + s + "\'");
	}

	public static void RemoveAllTopObjects() {
		foreach (string s in TopObjects.Keys) {
			GameObject.Destroy(TopObjects[s].gameObject);
		}
	}
	public static void AddSavedObject(Transform go) {
		if (go.parent == null && !SavedObjects.ContainsKey(go.name)) {
			SavedObjects.Add(go.name, go);
		}
	}
}
