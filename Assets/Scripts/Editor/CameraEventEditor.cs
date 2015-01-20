using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PoPCameraEvent))]

public class CameraEventEditor : Editor {
	
	public void OnSceneGUI()
	{
		PoPCameraEvent camEvent = (PoPCameraEvent)target;
		camEvent.eventCameraFocus = Handles.PositionHandle (camEvent.eventCameraFocus, Quaternion.identity);
		camEvent.eventCameraPosition = Handles.PositionHandle (camEvent.eventCameraPosition, Quaternion.identity);
	}
}
