using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using SimpleJSON;
using Debug = FFP.Debug;

[CustomEditor (typeof(CheckpointManager))]
public class CheckpointManagerEditor : Editor
{
	CheckpointManager myTarget;
	string checkpointDataPath = Application.dataPath;
	
	//! Minimum space between nodes
	const int minSpace = 10;
	bool foldout = false;
	bool findClosest = false;
	
	//For marking closest checkpoints
	public List<Vector3> checkpointList = new List<Vector3>();
	Ray ray;
	RaycastHit hit;
	public int maxTrace = 7; //!<Maximum checkpoints to draw handles to
	
	//! Unity OnEnable function
	void OnEnable()
	{
		//Set reference to CheckpointManager class
		myTarget = (CheckpointManager)target;
		
		foldout = EditorPrefs.GetBool("CheckpointManagerEditor.foldout");
		findClosest = false;
		
		//find all gameobjects with the <checkpoint> and place (positions) into List<Vector3>
		Checkpoint[] cps = Object.FindObjectsOfType(typeof(Checkpoint)) as Checkpoint[];
		checkpointList.Clear();
		foreach (Checkpoint cp in cps) {
			checkpointList.Add(new Vector3(cp.transform.position.x, 0, cp.transform.position.z));
		}
	}
	
	//! Unity OnDisable function
	void OnDisable()
	{
		EditorPrefs.SetBool("CheckpointManagerEditor.foldout", foldout);
		EditorPrefs.SetString("CheckpointManagerEditor.checkpointFilePath", myTarget.checkpointFilePath);
	}
	
	
	/*!
	 * 	Make a button to (Re)Build all checkpoints. This means calculating all walkable nodes, then calculating the distance from each node to each checkpoint.
	 *	Make a button to save NodeTree data to a json string and write it to a file
	 */
	public override void OnInspectorGUI()
	{		
		//Set checkpointFilePath text field
		myTarget.checkpointFilePath = GUILayout.TextField(myTarget.checkpointFilePath);
		
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
			if (!System.IO.File.Exists(checkpointDataPath + myTarget.checkpointFilePath)) {
				SaveCheckpoints(myTarget);
			} else if (EditorUtility.DisplayDialog("Continue save from file",
			                                       "File data already exists, this will override older data.",
			                                       "Save",
			                                       "Cancel")) {
				SaveCheckpoints(myTarget);
			}
		}
		
		//Load checkpoints button
		if (GUILayout.Button("Load checkpoints from file")) {
			if (myTarget.checkpointTree == null) {
				LoadCheckpoints(myTarget);
			} else if (EditorUtility.DisplayDialog("Continue load from file",
			                                       "Checkpoint data already exists, this will override older data.",
			                                       "Load",
			                                       "Cancel")) {
				LoadCheckpoints(myTarget);
			}
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		s = !findClosest ? "Find closest checkpoint" : "Turn off \"Find closest checkpoint\"";
		if (GUILayout.Button(s) /*&& !EditorApplication.isPlaying*/) {
			if(findClosest != true){
				if(myTarget.checkpointTree == null)
					LoadCheckpoints(myTarget);
				if (checkpointList != null && checkpointList.Count != 0) {
					findClosest = true;
					Debug.Log("checkpoint", "Drawing closest checkpoints...");
				}
				SceneView.RepaintAll();
			}else if(findClosest == true){
				findClosest = false;
				Debug.Log("checkpoint", "No longer drawing closest checkpoints");
				SceneView.RepaintAll();
			} else{
				findClosest = false;
			}
		}
		EditorGUILayout.HelpBox("Will draw lines to closest checkpoints", MessageType.Info);
		
		EditorGUILayout.Space();
		
		//Display all checkpoint locations
		EditorGUILayout.LabelField("Checkpoints", checkpointList.Count.ToString());
		EditorGUILayout.Space();
		foldout = EditorGUILayout.Foldout(foldout, "Display checkpoints");
		if (foldout) {
			foreach (Vector3 v in checkpointList) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("X", GUILayout.MaxWidth(20));
				EditorGUILayout.LabelField(v.x.ToString("0.0"), GUILayout.MaxWidth(80));
				EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(20));
				EditorGUILayout.LabelField(v.z.ToString("0.0"));
				EditorGUILayout.EndHorizontal();
			}
		}
	}
	
	//!Unity Editor function
	void OnSceneGUI(){
		if (myTarget != null && myTarget.checkpointTree != null && findClosest) {
			ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0F)){
				List<Vector3> cpList = myTarget.checkpointTree.Search(hit.point);
				if(cpList != null){
					int min = Mathf.Min(7, cpList.Count);
					for (int i = 0; i < min; i++) {
						Handles.color = Color.Lerp(Color.red, Color.gray, (float)i/min);
						Handles.DrawDottedLine(hit.point + Vector3.up,cpList[i] + Vector3.up, i*i*i+i*i+1);
					}
				}
				SceneView.RepaintAll();
			}
		}
	}
	
	void SaveCheckpoints(CheckpointManager myTarget)
	{
		if (myTarget.checkpointTree.SaveTreeAsJson(checkpointDataPath + myTarget.checkpointFilePath)) {
			Debug.Log("checkpoint", "Checkpoints saved.");
		} else {
			Debug.Warning("checkpoint", "Saving checkpoints failed.");
		}
	}
	
	void LoadCheckpoints(CheckpointManager myTarget)
	{
		if(myTarget == null){
			Debug.Error("checkpoint","Logical error, code missing? \"myTarget\" should have been set in onEnable()");
		}
		myTarget.checkpointTree = new NodeTree();
		if (myTarget.LoadCheckpointData()) {
			Debug.Log("checkpoint", "Checkpoints loaded.");
		} else {
			Debug.Warning("checkpoint", "Loading checkpoints failed.");
		}
	}
	
	void BuildNodeTree(CheckpointManager myTarget)
	{
		//find all gameobjects with the <checkpoint> and place (positions) into List<Vector3>
		Checkpoint[] cps = Object.FindObjectsOfType(typeof(Checkpoint)) as Checkpoint[];
		checkpointList.Clear();
		foreach (Checkpoint cp in cps) {
			checkpointList.Add(new Vector3(cp.transform.position.x, 0, cp.transform.position.z));
		}
		if (checkpointList.Count == 0) {
			Debug.Warning("editor", "No Checkpoints in scene");
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
			foreach (Vector3 c in checkpointList) {
				tempFloat = 0.0f; //use to measure disatance here
				NavMesh.CalculatePath(n, c, -1, path);
				
				if (path.corners.Length != 0) {
					tempV3 = path.corners [0]; //used to hold last V3 value here
					foreach (Vector3 v in path.corners) {
						tempFloat = tempFloat + Vector3.Distance(tempV3, v);
						
						tempV3 = v;
						//Debug.Log ("editor","[Remaining distance "+v+"]");
					}
					checkpointDict.Add(c, tempFloat);
				}
			}
			
			//Sort checkpoint list by shortest distance
			List<Vector3> cpList = new List<Vector3>();	//Temporary list of checkpoints sorted by shortest distance from the node
			for (int i = 0; i <checkpointDict.Count; i++) {
				tempFloat = float.MaxValue; //used to find smallest value in dictionary here
				foreach (KeyValuePair<Vector3, float> p in checkpointDict) {
					if (!cpList.Contains(p.Key) && p.Value < tempFloat) {
						tempFloat = p.Value;
						tempV3 = p.Key; //used to hold key
					}
				}
				cpList.Add(tempV3);
			}
			
			//Add sorted list of checkpoints to the nodeList
			nodeList.Add(cpList);
		}
		
		EditorApplication.isPlaying = false;
		
		//Put all node data and checkpoint paths into NodeTree
		Dictionary<Vector3,List<Vector3>> dict = new Dictionary<Vector3,List<Vector3>>();
		for (int i=0; i<nodes.Count; i++) {
			dict.Add(nodes [i], nodeList [i]);
		}
		
		myTarget.checkpointTree = new NodeTree(dict);
		
		//Return this object to Vector3.zero
		myTarget.transform.position = Vector3.zero;
		
		//Pull up dialogue box with results
		FinishDialogueBox(checkpointList.Count, min, max, nodes.Count);
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