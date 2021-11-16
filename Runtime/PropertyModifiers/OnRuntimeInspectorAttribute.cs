using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.dgn.UnityAttributes
{
    public class OnRuntimeInspectorAttribute : PropertyModifierAttribute
    {
#if UNITY_EDITOR
        public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
        {
            return visible && Application.isPlaying == true;
        }

        public override float GetHeight(SerializedProperty property, GUIContent label, float height)
        {
            if (Application.isPlaying == true)
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
}