using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.dgn.UnityAttributes
{
    public class OnEditorModeInspectorAttribute : PropertyModifierAttribute
    {
        public bool HideOnRuntime = false;

#if UNITY_EDITOR
        bool wasEnabled;

        public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
        {
            wasEnabled = GUI.enabled;
            GUI.enabled = (Application.isPlaying == false);
            return visible && (Application.isPlaying == false || HideOnRuntime == false);
        }
        public override void AfterGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = wasEnabled;
        }

        public override float GetHeight(SerializedProperty property, GUIContent label, float height)
        {
            if (Application.isPlaying == false || HideOnRuntime == false)
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