#pragma warning disable 219     //Variable assigned and not used: top
#pragma warning disable 414     //Variable assigned and not used: armor, aTarget

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {

    AudioManager aTarget;
    bool showDefault = false;
    SampleAudio[] listItem;
    int inspectorWidth;

    void OnEnable() {
        listItem = new SampleAudio[3];
        listItem[0] = new SampleAudio();
        listItem[1] = new SampleAudio();
        listItem[2] = new SampleAudio();

        Reload();
    }

    void Reload() {
        aTarget = (AudioManager)target;
        inspectorWidth = Screen.width;
    }

    int armor = 0;

	public override void OnInspectorGUI() {
        EditorGUILayout.LabelField("Active Audio:");

        int top = 150;
        foreach(SampleAudio m in listItem){
            //top += 40;
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(m.time.ToString(), GUILayout.MaxWidth(inspectorWidth / 2));
            EditorGUILayout.LabelField(m.time.ToString(), GUILayout.MaxWidth(inspectorWidth / 2));


            EditorGUILayout.EndHorizontal();
            //EditorGUI.ProgressBar(GUILayoutUtility.GetRect(inspectorWidth, 50), m.time / 1000.0f, "Armor");
            //armor = EditorGUI.IntSlider(new Rect(0, 200, inspectorWidth, 50), "Armor:", armor, 0, 100);
            //EditorGUI.ProgressBar(new Rect(10, top, inspectorWidth / 2, 20), m.time / 1000.0f, "Armor");

            //EditorGUILayout.Space();
        }
        showDefault = EditorGUILayout.BeginToggleGroup("Show Default", showDefault);
        if(showDefault)
			DrawDefaultInspector();
        EditorGUILayout.EndToggleGroup();

	}
}


public class SampleAudio {
    public float time = 500;
}






/*TODO:
 *   Always show title "Active Audio"
 *   Only show type of audio if type has audio currently playing
 *   Show name, play head location(updating every 1/10th of a sec), total time - NAME - 2:30/5:40
 *   
 * TUTORIALS/INFO
 *   http://unity3d.com/learn/tutorials/modules/intermediate/editor/building-custom-inspector
 *   http://docs.unity3d.com/ScriptReference/CustomEditor.html
 *   http://docs.unity3d.com/ScriptReference/EditorGUILayout.html
 *   http://docs.unity3d.com/ScriptReference/EditorGUI.html
 * 
 * 
 * PLANS:
 *   Add buttons to pause a single song next to its name
 *   Add button to pause all of a type next to the type
 *   Add button to pause all audio at bottom
 *   Add button to do the same as above for stop
 * 
 * DONE:
 *	 Toggle button for showing default inspector window
 */

