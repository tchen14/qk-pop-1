using UnityEngine;
using UnityEditor;
using System.Collections;

/*
 * What still needs to be in the scene:
 * 	- mainHUD (prefab)
 *  - ability wheel (prefab?)
 *  - ability wheel enchor (prefab?)
 *  - worldMapCanvas (prefab?)
 *  - mapBG ?
 *  - testObjectiveCanvas ?
 *  - pauseMenu ?
 */

public class Scene_Setup : EditorWindow {

	[MenuItem("Custom Tools/Setup Scene")]
	static void setup(){
		GameObject go;
		GameObject objManager = GameObject.Find ("_ObjectManager");
		GameObject masterManager = GameObject.Find ("_MasterSceneManager");
		GameObject inputManager = GameObject.Find ("_InputManager");
		GameObject audioManager = GameObject.Find ("_AudioManager");

		GameObject player = GameObject.Find ("_Player");
		GameObject camera = GameObject.Find("_Main Camera");

		if (objManager == null) {
			go = new GameObject();
			go.AddComponent<ObjectManager>();
			go.name = "_ObjectManager";
			objManager = GameObject.Find ("_ObjectManager");
			ObjectManager.AddSavedObject(go.transform);
		}

		if (masterManager == null) {
			go = new GameObject();
			go.AddComponent<MasterSceneManager>();
			go.name = "_MasterSceneManager";
			masterManager = GameObject.Find ("_MasterSceneManager");
			MasterSceneManager manager = masterManager.GetComponent<MasterSceneManager>();
			ObjectManager.AddSavedObject(go.transform);
			manager.InitScenesDictionary();
		}

		if (inputManager == null) {
			go = new GameObject();
			go.AddComponent<InputManager>();
			go.name = "_InputManager";
			inputManager = GameObject.Find ("_InputManager");
			ObjectManager.AddSavedObject(go.transform);
		}

		if (audioManager == null) {
			go = new GameObject();
			go.AddComponent<AudioManager>();
			go.name = "_AudioManager";
			audioManager = GameObject.Find ("_AudioManager");
			ObjectManager.AddSavedObject(go.transform);
		}

		if (player == null) {
			Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/_Player.prefab", typeof(GameObject));
			go = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
			go.name = "_Player";
			player = GameObject.Find ("_Player");
			ObjectManager.AddSavedObject(go.transform);
		}

		if (camera == null) {
			go = new GameObject();
			go.AddComponent<Camera>();
			go.AddComponent<PoPCamera>();
			PoPCamera popc = go.GetComponent<PoPCamera>();
			popc.target = player.transform;
			go.AddComponent<GameHUD>();
			go.name = "_Main Camera";
			camera = GameObject.Find("_Main Camera");
			ObjectManager.AddSavedObject(go.transform);
		}
	}

}
