using UnityEngine;
using System;
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

//[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
//    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyModifierAttribute
{
    public enum Condition { True, False}

    //The name of the bool field that will be in control
    public string ConditionalSourceField = "";
    //TRUE = Hide in inspector / FALSE = Disable in inspector 
    public bool HideInInspector = false;
    // Condition that apply this attribute
    public Condition ApplyCondition = Condition.True;
    
    public ConditionalHideAttribute(string conditionalSourceField)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.ApplyCondition = Condition.True;
    }
    
    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.ApplyCondition = Condition.True;
    }
    
    public ConditionalHideAttribute(string conditionalSourceField, Condition condition)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.ApplyCondition = condition;
    }
    
    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, Condition condition)
    {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.ApplyCondition = condition;
    }

#if UNITY_EDITOR

    bool wasEnabled;

    public override bool BeforeGUI(ref Rect position, SerializedProperty property, GUIContent label, bool visible)
    {
        bool enabled = GetConditionalHideAttributeResult(property);
        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        return (!HideInInspector || enabled) && visible;
    }

    public override void AfterGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = wasEnabled;
    }

    public override float GetHeight(SerializedProperty property, GUIContent label, float height)
    {
        bool enabled = GetConditionalHideAttributeResult(property);

        if (!HideInInspector || enabled)
        {
            return height;
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private bool GetConditionalHideAttributeResult(SerializedProperty property)
    {
        bool enabled = true;
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, ConditionalSourceField); //changes the path to the conditional source property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            enabled = sourcePropertyValue.boolValue;
            if (ApplyCondition == ConditionalHideAttribute.Condition.False) enabled = !enabled;
        }
        else
        {
            Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + ConditionalSourceField);
        }
        return enabled;
    }
#endif

}