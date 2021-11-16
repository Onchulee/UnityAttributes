using UnityEngine;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Hide or Disable Fields based on user input
// How to use; use like Header attribute, defualt condition is false
// 1. [ConditionalHide("EnableAutoAim", true)]]
/**
        [Header("Auto Aim")]
        public bool EnableAutoAim = false;
 
        [ConditionalHide("EnableAutoAim", true)]
        public float Range = 0.0f;
**/
// 2. [ConditionalHide("ConsumeResources")]
/**
        [Header("Resources")]
        public bool ConsumeResources = true;
 
        [ConditionalHide("ConsumeResources")]
        public bool DestroyOnResourcesDepleted = true;
**/
// credit: brechtos
// source: http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/


namespace com.dgn.UnityAttributes
{
    /// <summary>
    ///  Hide or Disable Fields based on user input
    /// <para>
    /// conditionalSourceField : Name of field that will be condition for hide/display this field.
    /// </para>
    /// <para>
    /// hideInInspector : If (true), hide this field when condition is matched. Otherwise disable it. Default is false.
    /// </para>
    /// <para>
    /// condition : Condition that apply this attribute. Default is true.
    /// </para>
    /// </summary>

    public class ConditionalHideAttribute : PropertyModifierAttribute
    {
        //The name of the bool field that will be in control
        public string ConditionalSourceField = "";
        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public bool HideInInspector = false;
        // Condition that apply this attribute
        public ConditionalHide ApplyCondition = ConditionalHide.True;

        public ConditionalHideAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
            this.ApplyCondition = ConditionalHide.True;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.ApplyCondition = ConditionalHide.True;
        }

        public ConditionalHideAttribute(string conditionalSourceField, ConditionalHide condition)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = false;
            this.ApplyCondition = condition;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, ConditionalHide condition)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspector = hideInInspector;
            this.ApplyCondition = condition;
        }

#if UNITY_EDITOR

        struct Warning
        {
            public bool sourceProp;
            public bool typeProp;
            public bool arrayProp;
        }

        Warning warning;
        bool wasEnabled;

        public override float GetHeight(SerializedProperty property, GUIContent label, float height)
        {
            bool enabled = GetConditionalHideAttributeResult(property);

            if (!warning.arrayProp)
            {
                string[] variableName = property.propertyPath.Split('.');
                SerializedProperty rootProp = property.serializedObject.FindProperty(variableName[0]);
                if (rootProp.isArray)
                {
                    Debug.LogWarning("Property [" + variableName[0] + "] couldn't be handled properly since it is an array.");
                }
                warning.arrayProp = true;
            }

            if (enabled || HideInInspector == false)
                return height;

            return -EditorGUIUtility.standardVerticalSpacing;
        }

        public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
        {
            bool enabled = GetConditionalHideAttributeResult(property);
            wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            return visible && (enabled || HideInInspector == false);
        }

        public override void AfterGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = wasEnabled;
        }

        private bool GetConditionalHideAttributeResult(SerializedProperty property)
        {
            bool enabled = true;

            SerializedProperty sourcePropertyValue = FindSerializableProperty(property);

            if (sourcePropertyValue != null)
            {
                enabled = CheckPropertyType(sourcePropertyValue);
            }
            else
            {
                var propertyInfo = property.serializedObject.targetObject.GetType().GetProperty(ConditionalSourceField,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(property.serializedObject.targetObject);
                    if (!CheckValueType(value, out enabled))
                    {
                        if (warning.sourceProp)
                        {
                            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + ConditionalSourceField);
                            warning.sourceProp = true;
                        }
                    }
                }
            }
            if (ApplyCondition == ConditionalHide.False) enabled = !enabled;
            return enabled;
        }

        private SerializedProperty FindSerializableProperty(SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            int idx = propertyPath.LastIndexOf('.');
            if (idx == -1)
            {
                return property.serializedObject.FindProperty(ConditionalSourceField);
            }
            else
            {
                string[] variableName = property.propertyPath.Split('.');
                SerializedProperty rootProp = property.serializedObject.FindProperty(variableName[0]);
                if (rootProp.isArray)
                {
                    return rootProp.serializedObject.FindProperty(ConditionalSourceField);
                }
                propertyPath = propertyPath.Substring(0, idx);
                return property.serializedObject.FindProperty(propertyPath)
                .FindPropertyRelative(ConditionalSourceField);
            }
        }

        private bool CheckPropertyType(SerializedProperty _propertyValue)
        {
            // Add other data types to handel them here.
            SerializedPropertyType _propType = _propertyValue.propertyType;
            if (_propType == SerializedPropertyType.Boolean)
            {
                return _propertyValue.boolValue;
            }
            else
            {
                if (!warning.typeProp)
                {
                    Debug.LogError("Data type of the property used for conditional hiding [" + _propertyValue.propertyType + "] is currently not supported");
                    warning.typeProp = true;
                }
                return true;
            }
        }

        private bool CheckValueType(object val, out bool retVal)
        {
            retVal = false;
            if (val is bool)
            {
                retVal = (bool)val;
                return true;
            }
            return false;
        }
#endif
    }
}