using System;
using System.Reflection;
using UnityEditor;

namespace com.dgn.UnityAttributes.Editor
{
    public static class AttributeEditorUtilities
    {
        public static bool HasAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            // Inspired by http://answers.unity.com/answers/1347452/view.html
            // and https://forum.unity.com/threads/multiple-attributes.387515/#post-2566777
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            // fi is sometimes null for reasons I ignore but it still seems to work
            return fi?.GetCustomAttribute<T>() != null;
        }

        public static bool TryGetAttribute<T>(this SerializedProperty property, out T attribute) where T : Attribute
        {
            attribute = default;
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fi != null)
            {
                attribute = fi.GetCustomAttribute<T>();
            }
            return fi?.GetCustomAttribute<T>() != null;
        }
    }
}