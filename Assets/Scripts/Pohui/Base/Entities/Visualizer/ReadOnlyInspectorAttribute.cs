using System;
using UnityEngine;
using Assets.Scripts.Entities.Visualizer;


#if UNITY_EDITOR_64

using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var previousGUIState = GUI.enabled;
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label);
		GUI.enabled = previousGUIState;
	}
}

namespace Assets.Scripts.Entities.Visualizer
{
	public class ReadOnlyInspectorAttribute : PropertyAttribute
	{
	}
}


#endif
