using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/*!
 *	No sure how this really works, but this class along with the CustomPropertyDrawer below allow the [Readonly] property tag
 *	to make inspector fields read only (greyed out and not editable).
 */
public class ReadOnlyAttribute : PropertyAttribute
{
	
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property,
	                                        GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}
	
	public override void OnGUI(Rect position,
	                           SerializedProperty property,
	                           GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}
#endif