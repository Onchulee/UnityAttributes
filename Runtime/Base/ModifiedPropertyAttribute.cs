using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ModifiedPropertyAttribute : ModifiablePropertyAttribute
    {
#if UNITY_EDITOR
        public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        public virtual float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
#endif
    }
}