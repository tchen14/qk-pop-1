using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof (CheckpointManager))]
public class CheckpointManagerEditor : Editor {

    const int minSpace = 1;
    private int paths = 0;

    public override void OnInspectorGUI() {
        CheckpointManager myTarget = (CheckpointManager)target;
        if (GUILayout.Button ("(Re)Build checkpoints")) {
            //find all gameobjects with the <checkpoint> and path find from each node
            Checkpoint[] cps = Object.FindObjectsOfType (typeof (Checkpoint)) as Checkpoint[];
            myTarget.checkpoints.Clear();
            int count = 0;
            foreach (Checkpoint cp in cps) {
                myTarget.checkpoints.Add (new Vector2 (cp.transform.position.x,cp.transform.position.z));
                count++;
            }
            if (count == 0)
                Log.W ("editor", "No Checkpoints in scene");
            else
                Log.M ("editor", "["+ count + " Checkpoints in scene]");
            //get max bounds of the scene
            Vector2 min = new Vector2();
            Vector2 max = new Vector2();
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() as GameObject[];
            foreach (GameObject go in allObjects)
                if (go.activeInHierarchy) {
                    Vector3 pos = go.transform.position;
                    min.x = Mathf.Min (pos.x, min.x);
                    min.y = Mathf.Min (pos.y, min.y);
                    max.x = Mathf.Max (pos.x, max.x);
                    max.y = Mathf.Max (pos.y, max.y);
                }
            Log.M ("editor","[Bounds are "+min+","+max+"]");
            //create node list minSpace width/height appart for the region of the calculated bounds
            List<Vector3> nodes = new List<Vector3>();
            for (float i = min.x; i < max.x; i+=minSpace) {
                for (float j = min.y; j < max.y; j+=minSpace)
                    nodes.Add (new Vector3 (i,0,j));
            }
            Log.M ("editor","[Nodes.count "+nodes.Count+"]");
            //Path trace from each node to each checkpoint
            /*Vector3 end = new Vector3(1,10,10);
            myTarget.agent = myTarget.GetComponent<NavMeshAgent>();
            myTarget.agent.CalculatePath(end, myTarget.path);
            Log.M("editor","[Remaining distance "+myTarget.agent.remainingDistance+"]");

            if (myTarget.path.status == NavMeshPathStatus.PathPartial) {

            }*/
            myTarget.fucks();
            //return this object to Vector3.zero
            myTarget.transform.position = Vector3.zero;
        }
        EditorGUILayout.LabelField ("Paths", paths.ToString());
        EditorGUILayout.Space();
        foreach (Vector3 v in myTarget.checkpoints) {
            //EditorGUILayout.Vector2Field("",v,GUILayout.MaxWidth(200));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField ("X",GUILayout.MaxWidth (20));
            EditorGUILayout.LabelField (v.x.ToString ("0.0"),GUILayout.MaxWidth (80));
            EditorGUILayout.LabelField ("Z",GUILayout.MaxWidth (20));
            EditorGUILayout.LabelField (v.z.ToString ("0.0"));
            EditorGUILayout.EndHorizontal();
        }
    }
}
