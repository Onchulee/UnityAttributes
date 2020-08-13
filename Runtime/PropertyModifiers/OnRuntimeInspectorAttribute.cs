using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OnRuntimeInspectorAttribute : PropertyModifierAttribute
{
#if UNITY_EDITOR

    public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
    {
        return visible && Application.isPlaying;
    }

    public override float GetHeight(SerializedProperty property, GUIContent label, float height)
    {
        if (Application.isPlaying)
        {
            return height;
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

#endif

}
