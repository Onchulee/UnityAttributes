using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 Example:
     
    case 1:
    [LabelArray("val")]
    public int[] values; // each element will be displayed as "val"

    case 2:
    [LabelArray("target", true)]
    public GameObject[] target; // each element will be displayed as "target" or name of each GameObject

    case 3:
    [System.Serializable]
    public struct TestStruct {
        public GameObject obj;
        public string str;
    }
    [LabelArray("obj", true)]
    public TestStruct[] objLabel; // each element will be displayed as "obj" or name of GameObject [obj]
    [LabelArray("str", true)]
    public TestStruct[] strLabel; // each element will be displayed as "str" or string value from [str]

**/


/// <summary>
///  <para>Rename array element label with [fieldName]</para>
///  <para>
///  If [replaceWithValue] is true, label name will be replaced by value of field with name [fieldName]; Only when the field is not empty.
///  </para>
///  <para>
///  [replaceWithValue] can be applied on field with type of String or GameObject only.
///  </para>
/// </summary>
public class LabelArrayAttribute : ModifiedPropertyAttribute
{
    public string TargetName { get; private set; }
    public bool ReplaceWithValue { get; private set; }

    public LabelArrayAttribute(string fieldName)
    {
        TargetName = fieldName;
        ReplaceWithValue = false;
    }

    public LabelArrayAttribute(string fieldName, bool replaceWithValue)
    {
        TargetName = fieldName;
        ReplaceWithValue = replaceWithValue;
    }

#if UNITY_EDITOR
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            if (ReplaceWithValue)
            {
                DrawRenameWithValue(position, property, TargetName);
            }
            else
            {
                DrawLabelWithName(position, property, TargetName);
            }
        }
        catch
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (GetCustomDrawer(property.type, out PropertyDrawer drawer))
        {
            return drawer.GetPropertyHeight(property, label);
        }
        return EditorGUI.GetPropertyHeight(property, true);
    }


    public void DrawLabelWithName(Rect rect, SerializedProperty property, string labelName)
    {
        var path = property.propertyPath;
        string[] pathSplit = property.propertyPath.Split('[', ']');
        string pos = pathSplit[pathSplit.Length - 2];
        if (GetCustomDrawer(property.type, out PropertyDrawer drawer))
        {
            drawer.OnGUI(rect, property, new GUIContent(labelName + " [" + pos + "]"));
        }
        else
        {
            EditorGUI.PropertyField(rect, property, new GUIContent(labelName + " [" + pos + "]"), true);
        }
    }

    public void DrawRenameWithValue(Rect rect, SerializedProperty property, string fieldName)
    {
        var path = property.propertyPath;
        string[] pathSplit = property.propertyPath.Split('[', ']');
        string pos = pathSplit[pathSplit.Length - 2];

        SerializedProperty fieldProp = property.FindPropertyRelative(fieldName);
        if (fieldProp == null)
        {
            if (path.Contains(fieldName)) {
                fieldProp = property;
            }
            else {
                EditorGUI.PropertyField(rect, property, new GUIContent("[" + pos + "] " + fieldName), true);
                return;
            }
        }

        fieldName = ObjectNames.NicifyVariableName(fieldName);

        if (fieldProp.propertyType.Equals(SerializedPropertyType.ObjectReference))
        {

            if (fieldProp.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(rect, property, new GUIContent("[" + pos + "] " + "Unassigned: " + fieldName), true);
                return;
            }

            if (GetCustomDrawer(property.type, out PropertyDrawer drawer))
            {
                drawer.OnGUI(rect, property, new GUIContent("[" + pos + "] " + fieldProp.objectReferenceValue.name));
            }
            else
            {
                EditorGUI.PropertyField(rect, property, new GUIContent("[" + pos + "] " + fieldProp.objectReferenceValue.name), true);
            }
        }
        else if (fieldProp.propertyType.Equals(SerializedPropertyType.String)) {
            if (string.IsNullOrEmpty(fieldProp.stringValue)) {
                EditorGUI.PropertyField(rect, property, new GUIContent("[" + pos + "] " + "Unassigned: " + fieldName), true);
            }
            else {
                EditorGUI.PropertyField(rect, property, new GUIContent("[" + pos + "] " + fieldProp.stringValue), true);
            }
        }
        else {
            EditorGUI.PropertyField(rect, property, new GUIContent("[" + pos + "] " + fieldName), true);
        }
    }

    public bool GetCustomDrawer(string type, out PropertyDrawer propertyDrawer)
    {
        propertyDrawer = null;
        return false;
    }
#endif
}