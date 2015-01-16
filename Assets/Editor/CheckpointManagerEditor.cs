using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof(CheckpointManager))]
public class CheckpointManagerEditor : Editor
{
	
	//! Minimum space between nodes
	const int minSpace = 100;
	bool foldout = false;
	
	/*!
	 * 	Make a button to (Re)Build all checkpoints. This means calculating all walkable nodes,
	 * 	then calculating the distance from each node to each checkpoint.
	 */
	void OnEnable()
	{
		foldout = EditorPrefs.GetBool("CheckpointManagerEditor.foldout");
		//findClosest = EditorPrefs.GetBool("CheckpointManagerEditor.findClosest");
	}
	
	void OnDisable()
	{
		EditorPrefs.SetBool("CheckpointManagerEditor.foldout", foldout);
		//EditorPrefs.SetBool("CheckpointManagerEditor.findClosest", findClosest);
	}
	
	public override void OnInspectorGUI()
	{
		//Set reference to CheckpointManager class
		CheckpointManager myTarget = (CheckpointManager)target;
		
		//(Re)Build checkpoints button
		string s = (myTarget.checkpointTree == null) ? "Build checkpoints" : "Rebuild checkpoints";
		if (GUILayout.Button(s)) {
			BuildNodeTree(myTarget);
		}
		
		//Save checkpoints button
		if (myTarget.checkpointTree == null) {
			GUI.enabled = false;
			GUILayout.Button("Please build checkpoints first");
			GUI.enabled = true;
		} else if (GUILayout.Button("Save checkpoints to file")) {
			if (myTarget.checkpointTree.SaveTreeAsJson(Application.dataPath + "/Resources/Json/checkpointData.json")) {
				Log.M("checkpoint", "Checkpoints saved.");
			} else {
				Log.M("checkpoint", "Saving checkpoints failed.");
			}
		}
		
		//Load checkpoints button
		if (GUILayout.Button("Load checkpoints from file")) {
			if (myTarget.checkpointTree == null) {
				LoadCheckpoints(myTarget);
			} else if (EditorUtility.DisplayDialog("Continue load from file",
			                                       "Checkpoint data already exists. Old data will be wiped.",
			                                       "Continue",
			                                       "Cancel")) {
				LoadCheckpoints(myTarget);
			}
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.HelpBox("Will Color Closest Checkpoint Red", MessageType.Info);
		myTarget.pos = EditorGUILayout.Vector3Field("Pos", myTarget.pos);
		if (GUILayout.Button("Find closest checkpoint") && !EditorApplication.isPlaying) {
			List<Vector3> tpmp = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>().checkpointTree.Search(myTarget.pos);
			
			if (tpmp != null && tpmp.Count != 0) {
				myTarget.closestCheckpoint = tpmp [0];
				myTarget.findClosest = true;
				Log.M("checkpoint", "Closest checkpoint: " + myTarget.closestCheckpoint);
				foreach (Vector3 v in tpmp) {
					//Log.M ("checkpoint", ""+v);
				}
			} else {
				Log.M("checkpoint", "Un-walkable position.");
			}
			SceneView.RepaintAll();
			
		}
		
		//Display all checkpoint locations
		EditorGUILayout.LabelField("Checkpoints", myTarget.checkpointList.Count.ToString());
		EditorGUILayout.Space();
		foldout = EditorGUILayout.Foldout(foldout, "Display checkpoints");
		if (foldout) {
			foreach (Vector3 v in myTarget.checkpointList) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("X", GUILayout.MaxWidth(20));
				EditorGUILayout.LabelField(v.x.ToString("0.0"), GUILayout.MaxWidth(80));
				EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(20));
				EditorGUILayout.LabelField(v.z.ToString("0.0"));
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	void LoadCheckpoints(CheckpointManager myTarget)
	{
		myTarget.checkpointTree = new NodeTree();
		if (myTarget.checkpointTree.LoadTreeFromFile(Application.dataPath + "/Resources/Json/checkpointData.json")) {
			Log.M("checkpoint", "Checkpoints loaded.");
		} else {
			Log.M("checkpoint", "Loading checkpoints failed.");
		}
	}
		
	void BuildNodeTree(CheckpointManager myTarget)
	{
		//find all gameobjects with the <checkpoint> and place (positions) into List<Vector3>
		Checkpoint[] cps = Object.FindObjectsOfType(typeof(Checkpoint)) as Checkpoint[];
		myTarget.checkpointList.Clear();
		foreach (Checkpoint cp in cps) {
			myTarget.checkpointList.Add(new Vector3(cp.transform.position.x, 0, cp.transform.position.z));
		}
		if (myTarget.checkpointList.Count == 0) {
			Log.W("editor", "No Checkpoints in scene");
			return;
		}
		
		//get max bounds of the scene. Used to calculate area nodes must be placed
		Vector2 min = new Vector2();
		Vector2 max = new Vector2();
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() as GameObject[];
		foreach (GameObject go in allObjects) {
			if (go.activeInHierarchy) {
				if (go.GetComponent<Renderer>()) {
					Bounds pos = go.GetComponent<Renderer>().bounds;
					min.x = Mathf.Min(pos.min.x, min.x);
					min.y = Mathf.Min(pos.min.z, min.y);
					max.x = Mathf.Max(pos.max.x, max.x);
					max.y = Mathf.Max(pos.max.z, max.y);
				} else {
					Vector3 pos = go.transform.position;
					min.x = Mathf.Min(pos.x, min.x);
					min.y = Mathf.Min(pos.z, min.y);
					max.x = Mathf.Max(pos.x, max.x);
					max.y = Mathf.Max(pos.z, max.y);
				}
			}
		}
		
		//Create node list minSpace width/height appart (using above bounds)
		//todo: include ability to filer out regions that are not walk/playable
		List<Vector3> nodes = new List<Vector3>();
		for (float i = min.x; i < max.x; i+=minSpace) {
			for (float j = min.y; j < max.y; j+=minSpace) {
				nodes.Add(new Vector3(i, 0, j));
			}
		}
		
		//Calculate the distances frome each checkpoint to each node. Unity (player) must be running to use NavMesh functions
		EditorApplication.isPlaying = true;
		
		//Below variables will be used thousands of times. Initialized up here for caching
		NavMeshPath path = new NavMeshPath();
		List<List<Vector3>> nodeList = new List<List<Vector3>>();	//Final list of all checkpoints. Corresponds with List<Vector3> nodes
		Dictionary<Vector3, float> checkpointDict = new Dictionary<Vector3, float>();	//Temporary dictionary holding checkpoint location and distance to the node
		float tempFloat;
		Vector3 tempV3 = new Vector3();

		foreach (Vector3 n in nodes) {
			//Calculate distance from node to each checkpoint
			checkpointDict.Clear();
			foreach (Vector3 c in myTarget.checkpointList) {
				tempFloat = 0.0f; //use to measure disatance here
				NavMesh.CalculatePath(n, c, -1, path);
				
				if (path.corners.Length != 0) {
					tempV3 = path.corners [0]; //used to hold last V3 value here
					foreach (Vector3 v in path.corners) {
						tempFloat = tempFloat + Vector3.Distance(tempV3, v);
						
						tempV3 = v;
						//Log.M ("editor","[Remaining distance "+v+"]");
					}
					checkpointDict.Add(c, tempFloat);
				}
			}

			//Sort checkpoint list by shortest distance
			List<Vector3> checkpointList = new List<Vector3>();	//Temporary list of checkpoints sorted by shortest distance from the node
			for (int i = 0; i <checkpointDict.Count; i++) {
				tempFloat = float.MaxValue; //used to find smallest value in dictionary here
				foreach (KeyValuePair<Vector3, float> p in checkpointDict) {
					if (!checkpointList.Contains(p.Key) && p.Value < tempFloat) {
						tempFloat = p.Value;
						tempV3 = p.Key; //used to hold key
					}
				}
				checkpointList.Add(tempV3);
			}
			
			//Add sorted list of checkpoints to the nodeList
			nodeList.Add(checkpointList);
		}
		
		EditorApplication.isPlaying = false;
		
		Vector3 fucks = new Vector3();
		//Put all node data and checkpoint paths into NodeTree
		Dictionary<Vector3,List<Vector3>> dict = new Dictionary<Vector3,List<Vector3>>();
		for (int i=0; i<nodes.Count; i++) {
			dict.Add(nodes [i], nodeList [i]);
		}

		myTarget.checkpointTree = new NodeTree(dict);

		//Return this object to Vector3.zero
		myTarget.transform.position = Vector3.zero;
		
		//Pull up dialogue box with results
		FinishDialogueBox(myTarget.checkpointList.Count, min, max, nodes.Count);
	}
	
	void FinishDialogueBox(int checkpoints, Vector2 min, Vector2 max, int nodes)
	{
		if (EditorUtility.DisplayDialog("(Re)Building Checkpoints complete!",
		                                "[Checkpoints count is " + checkpoints + "]\n" +
			"[Max bounds are " + max + "]\n" +
			"[Min bounds are " + min + "]\n" +
			"[Node count is " + nodes + "]\n" +
			"[Total connections count is " + nodes * checkpoints + "]",
		                                "Continue")) {
		}
		Repaint();
	}
}

//Left here as example code
/*public class CheckpointManagerEditorWindow : EditorWindow {
	int numCheckpoints = 0;
	
	bool findClosest;
	NodeTree myTree;
	Vector3 pos;
	string closestCheckpoint = "";

	[MenuItem("Custom/CheckpointEditor")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(CheckpointManagerEditorWindow)).title = "Checkpoints";
	}

	void OnGUI()
	{

		if (GUILayout.Button ("Count checkpoints: " + numCheckpoints)) {
			numCheckpoints = 0;
			GameObject[] temps = GameObject.FindGameObjectsWithTag("Checkpoint");
			for (int i = 0; i < temps.Length; i++) {
				numCheckpoints++;
			}
			Repaint();
		}
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		findClosest = EditorGUILayout.BeginToggleGroup("Find Closest Checkpoint", findClosest);
		EditorGUILayout.HelpBox("Will Color Closest Checkpoint Red", MessageType.Info);
		pos = EditorGUILayout.Vector3Field("Pos", pos);

		closestCheckpoint = EditorGUILayout.TextField("Closest Checkpoint", closestCheckpoint);
		EditorGUILayout.EndToggleGroup();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		if (GUILayout.Button ("Delete all checkpoints") && !EditorApplication.isPlaying) {
			DeleteAllCheckpoints();
		}
	}

	void DeleteAllCheckpoints(){
		if(EditorUtility.DisplayDialog("Delete all checkpoints?",
		                            "Are you sure you want to delete all GameObjects with the Checkpoint component?",
		                            "Ba-boom!!",
		                            "Oh wait, nevermind.")){
			GameObject[] temps = GameObject.FindGameObjectsWithTag("Checkpoint");
			foreach (GameObject temp in temps) {
				DestroyImmediate(temp);
			}
		}
		numCheckpoints = 0;
		Repaint();
	}
}*/