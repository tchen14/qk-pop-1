#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AddPopEvent : EditorWindow {

    [MenuItem("Custom Tools/(+) Add Pop Event")]
    static public void AddComponent() {
        if (UnityEditor.Selection.objects.Length == 1) {
            GameObject go = UnityEditor.Selection.objects[0] as GameObject;
            go.AddComponent<PopEvent>();
        }
    }
}
#endif