using UnityEngine;
using UnityEditor;

#region CheckPointEditor
public class LevelDesignCheckPointEditor : EditorWindow {

	GameObject[] temps;
	string editorStatus = "";
	string checkpointContents = "";
	int numCheckpoints = 0;
	
	bool groupEnabled2;
	CheckPointTree myTree;
	bool waitForTree = true;
	float timeSinceLevelLoaded = 0;
	string x = "";
	string y = "";
	string z = "";
	string closestCheckpoint = "";
	MessageType info1;
	
	bool groupEnabled1;
	bool destroy = false;
	MessageType info2;
	
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/CheckpointEditor")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(LevelDesignCheckPointEditor));
	}
	
	void OnGUI()
	{
		if (!EditorApplication.isPlaying)
		{
			editorStatus = "Editor is Not Playing";
			numCheckpoints = 0;
			checkpointContents = numCheckpoints.ToString();
			timeSinceLevelLoaded = 0;
			waitForTree = true;
			
			if(destroy)
			{
				temps = GameObject.FindGameObjectsWithTag("Respawn");
				foreach(GameObject temp in temps)
				{
					DestroyImmediate(temp);
				}
				destroy = !destroy;
			}
			
			if(x.Length == 0 || y.Length ==0 || z.Length ==0)
			{
				closestCheckpoint = "Error: Enter all coordinates!";
				waitForTree = false;
			}
			
			Repaint();
		} 
		else
		{
			editorStatus = "Editor is Playing";
			
			if (numCheckpoints == 0)
			{
				temps = GameObject.FindGameObjectsWithTag("Respawn");
				foreach(GameObject temp in temps)
				{
					numCheckpoints++;
				}
			}
			if(waitForTree)
			{
				waitForTree = false;
				printNearest();
			}
			
			checkpointContents = numCheckpoints.ToString();
			Repaint();
		}
		
		
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		editorStatus = EditorGUILayout.TextField ("Editor editorStatus", editorStatus);
		checkpointContents = EditorGUILayout.TextField ("Number of Checkpoints", checkpointContents);
		
		groupEnabled2 = EditorGUILayout.BeginToggleGroup ("Find Closest Checkpoint", groupEnabled2);
		EditorGUILayout.HelpBox("Will Color Closest Checkpoint Red",info1);
		x = EditorGUILayout.TextField ("Enter x value of Vector", x);
		y = EditorGUILayout.TextField ("Enter y value of Vector", y);
		z = EditorGUILayout.TextField ("Enter z value of Vector", z);
		closestCheckpoint = EditorGUILayout.TextField ("Closest Checkpoint", closestCheckpoint);
		EditorGUILayout.EndToggleGroup ();
		
		groupEnabled1 = EditorGUILayout.BeginToggleGroup ("Destroy Control", groupEnabled1);
		EditorGUILayout.HelpBox("Will Destroy All Checkpoints: Be Careful",info2);
		destroy = EditorGUILayout.Toggle ("Destroy All Checkpoints", destroy);
		EditorGUILayout.EndToggleGroup ();
	}
	
	void printNearest()
	{
		float x1 = System.Convert.ToSingle(x);
		float y1 = System.Convert.ToSingle(y);
		float z1 = System.Convert.ToSingle(z);
		
		Vector3 tempVec = new Vector3(x1,y1,z1);
		Vector3 closest = CheckPointTree.search (tempVec).location;
		closestCheckpoint = closest.ToString();
		
		foreach(GameObject temp in temps)
		{
			if(Vector3.Distance(closest, temp.transform.position) < 0.1f)
			{
				temp.renderer.material.color = Color.red;
			}
		}
		
	}
}
#endregion
