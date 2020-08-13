using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public abstract class PropertyModifierAttribute : ModifiablePropertyAttribute
    {
#if UNITY_EDITOR
        public virtual float GetHeight(SerializedProperty property, GUIContent label, float height)
        {
            return height;
        }

        public virtual bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible) { return true; }
        public virtual void AfterGUI(Rect position, SerializedProperty property, GUIContent label) { }
#endif
    }
}