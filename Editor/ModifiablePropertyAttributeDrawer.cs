using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace com.dgn.UnityAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ModifiablePropertyAttribute), true)]
    public class ModifiablePropertyAttributeDrawer : PropertyDrawer
    {
        List<PropertyModifierAttribute> modifiers;
        ModifiedPropertyAttribute modifiedProp = null;
        bool initialized = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;
            if (!initialized)
            {
                modifiers = fieldInfo.GetCustomAttributes(typeof(PropertyModifierAttribute), false).Cast<PropertyModifierAttribute>()
                        .OrderBy(s => s.order).ToList();

                var modifiable = fieldInfo.GetCustomAttributes(typeof(ModifiedPropertyAttribute), false);
                if (modifiable.Length > 0)
                {
                    modifiedProp = (ModifiedPropertyAttribute)modifiable[0];
                }
                initialized = true;
            }

            if (modifiedProp == null)
            {
                height = EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                height = modifiedProp.GetPropertyHeight(property, label);
            }

            foreach (var attr in modifiers)
                height = attr.GetHeight(property, label, height);

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool visible = true;
            foreach (var attr in modifiers.AsEnumerable().Reverse())
                visible = attr.BeforeGUI(ref position, property, label, visible);

            if (visible)
            {
                if (modifiedProp == null)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
                else
                {
                    modifiedProp.OnGUI(position, property, label);
                }
            }

            foreach (var attr in modifiers)
                attr.AfterGUI(position, property, label);

        }

    }
}