using UnityEngine;
using UnityEditor;
using System.Collections;

/*
 * Create Path
 * 
 * Creates an empty object in the scene used to spawn checkpoints and create paths.
 */

public class createPath : EditorWindow {

	[MenuItem("Custom Tools/Create Path")]
	static void create(){
		GameObject go;
		go = new GameObject();
		go.AddComponent<AIPath>();
		go.name = "path";
	}
}
