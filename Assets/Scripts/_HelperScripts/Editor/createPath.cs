using UnityEngine;
using UnityEditor;
using System.Collections;

public class createPath : EditorWindow {

	[MenuItem("Custom Tools/Create Path")]
	static void create(){
		GameObject go;
		go = new GameObject();
		go.AddComponent<AIPath>();
		go.name = "path";
	}
}
