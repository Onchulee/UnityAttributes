using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.dgn.UnityAttributes
{
    /// <summary>
    /// Display a field as read-only in the inspector.
    /// CustomPropertyDrawers will not work when this attribute is used.
    /// </summary>
    /// <seealso cref="BeginReadOnlyGroupAttribute"/>
    /// <seealso cref="EndReadOnlyGroupAttribute"/>
    public class ReadOnlyAttribute : PropertyModifierAttribute
    {
#if UNITY_EDITOR
        public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
        {
            EditorGUI.BeginDisabledGroup(true);
            return visible;
        }

        public override void AfterGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.EndDisabledGroup();
        }
#endif
    }
}