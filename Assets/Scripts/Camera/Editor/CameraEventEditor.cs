using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Debug = FFP.Debug;

[CustomEditor(typeof(PoPCameraEvent))]

public class CameraEventEditor : Editor {

	bool showWaypoints = false;
    bool showfocus = false;

	public override void OnInspectorGUI()
	{
		PoPCameraEvent camEvent = (PoPCameraEvent)target;

		int columnWidth = Mathf.FloorToInt(Screen.width);
		int halfWidth = columnWidth / 2;
		int sixthWidth = columnWidth / 6;

        if (camEvent.eventPath.Count == 0) {
            if (GUILayout.Button("Create Event Path")) { 
                camEvent.Initialize();
            }
        } else {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Multiple Camera Focus");
			camEvent.multipleTargets = EditorGUILayout.Toggle(camEvent.multipleTargets, GUILayout.MaxWidth(halfWidth + sixthWidth));
			GUILayout.EndHorizontal();

			showWaypoints = EditorGUILayout.Foldout(showWaypoints, "Event Path");
			if(showWaypoints) {
                for (int i = 0; i < camEvent.eventPath.Count; i++)
                {
                    GUILayout.Label("Waypoint " + (i + 1));

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20f);
                    camEvent.eventPath[i] = EditorGUILayout.Vector3Field("Position", camEvent.eventPath[i]);
                    GUILayout.EndHorizontal();

                    //if(i < camEvent.path.Count - 1 && camEvent.multipleTargets && i < camEvent.focusPoints.Count) {
                    //    GUILayout.BeginHorizontal();
                    //    GUILayout.Space(20f);
                    //    camEvent.focusPoints[i].focus = EditorGUILayout.Vector3Field("Focus", camEvent.focusPoints[i].focus);
                    //    GUILayout.EndHorizontal();
                    //}

                    if (GUILayout.Button("Delete Waypoint")) {
                        camEvent.DeleteNode(i);
					}
				}
			}

			if(!camEvent.multipleTargets) {
				EditorGUILayout.Vector3Field("Event Focus", camEvent.singleTargetPos);
            } else {
                showfocus = EditorGUILayout.Foldout(showfocus, "Focus Points");
                if (showfocus) {
                    for (int i = 0; i < camEvent.focusPoints.Count; i++) {
                        GUILayout.Label("Focus " + (i + 1));
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20f);
                        camEvent.focusPoints[i].focus = EditorGUILayout.Vector3Field("Position", camEvent.focusPoints[i].focus);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20f);
                        camEvent.focusPoints[i].time = EditorGUILayout.FloatField("Time", camEvent.focusPoints[i].time);
                        GUILayout.EndHorizontal();

						if(GUILayout.Button("Delete Focus")) {
							camEvent.DeleteFocus(i);
						}
                    }
                }
            }

			camEvent.pathLength = EditorGUILayout.FloatField("Path Time Length", camEvent.pathLength);

            if (!showWaypoints) {
                if (GUILayout.Button("Delete Last Waypoint")) {
                    camEvent.DeleteNode(camEvent.eventPath.Count - 1);
                }
            }

			if(!showfocus) {
				if(GUILayout.Button("Delete Last Focus")) {
					camEvent.DeleteFocus(camEvent.focusPoints.Count - 1);
				}
			}

			EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Waypoint")) {
                camEvent.AddNode();
            }

			if(GUILayout.Button ("Add Focus")) {
				camEvent.AddFocus();
			}
			EditorGUILayout.EndHorizontal();

			if(GUILayout.Button("Dump Path")) {
				camEvent.DumpPath();
			}
        }

        if (GUI.changed) {
            EditorUtility.SetDirty(camEvent);
        }
	}

    public void OnSceneGUI()
    {
        PoPCameraEvent camEvent = (PoPCameraEvent)target;

        if(camEvent.eventPath.Count > 0) {
            if (camEvent.eventPath.Count == 4) {
                Handles.Label(camEvent.eventPath[0], "Cam Position Start");
                for (int i = 0; i < camEvent.eventPath.Count; i++) {
                    if(i == 1)
                        Handles.Label(camEvent.eventPath[i], "Start Bezeir");
                    else if (i == 2) 
                        Handles.Label(camEvent.eventPath[i], "End Bezier");
                    else if (i == 3)
                        Handles.Label(camEvent.eventPath[i], "Cam Position End");
                    camEvent.eventPath[i] = Handles.PositionHandle(camEvent.eventPath[i], Quaternion.identity);
                }
            } else if (camEvent.eventPath.Count > 4) {
                Handles.Label(camEvent.eventPath[0], "Start Bezier");
                for (int i = 0; i < camEvent.eventPath.Count; i++) {
                    camEvent.eventPath[i] = Handles.PositionHandle(camEvent.eventPath[i], Quaternion.identity);
                    if(i < camEvent.eventPath.Count - 2)
                        Handles.Label(camEvent.eventPath[i + 1], "Position " + (i+1));
                    else if(i < camEvent.eventPath.Count - 1)
                        Handles.Label(camEvent.eventPath[i + 1], "End Bezier");
                }
            } else if(camEvent.eventPath.Count == 3) {
                Handles.Label(camEvent.eventPath[0], "Cam Position Start");
                for(int i = 0; i < 3; i++) {
                    camEvent.eventPath[i] = Handles.PositionHandle(camEvent.eventPath[i], Quaternion.identity);
                    if(i == 0)
                        Handles.Label(camEvent.eventPath[i + 1], "Path Bezier");
                    else if(i < camEvent.eventPath.Count - 1)
                        Handles.Label(camEvent.eventPath[i + 1], "Cam Position End");
                }
            } else {
                Handles.Label(camEvent.eventPath[0], "Cam Position Start");
                for(int i = 0; i < 2; i++) {
                    camEvent.eventPath[i] = Handles.PositionHandle(camEvent.eventPath[i], Quaternion.identity);
                    if(i < 1)
                        Handles.Label(camEvent.eventPath[i + 1], "Cam Position End");
                }
            }
        }

        if (!camEvent.multipleTargets) {
            Handles.Label(camEvent.singleTargetPos, "Event Focus");
            camEvent.singleTargetPos = Handles.PositionHandle(camEvent.singleTargetPos, Quaternion.identity);
        } else {
            for(int i = 0; i < camEvent.focusPoints.Count; i++) {
                Handles.Label(camEvent.focusPoints[i].focus, "Focus " + (i + 1));
                camEvent.focusPoints[i].focus = Handles.PositionHandle(camEvent.focusPoints[i].focus, Quaternion.identity);
            }
        }

        Repaint();
    }
}
