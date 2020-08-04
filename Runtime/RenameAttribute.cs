using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Rename parameter field
// How to use; use like Header attribute
// [Rename("My super properties!")]
/**
    [Rename("My super properties!")]
    public TPC_Properties properties;
**/
// credit: Hellium
// source: https://answers.unity.com/questions/1487864/change-a-variable-name-only-on-the-inspector.html

public class RenameAttribute : ModifiablePropertyAttribute
{
    public string NewName { get; private set; }
    public RenameAttribute(string name)
    {
        NewName = name;
    }

#if UNITY_EDITOR
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUIContent displayLabel = label;
        if (HasDisplayName()) {
            displayLabel = new GUIContent(NewName);
        }
        EditorGUI.PropertyField(position, property, displayLabel);
    }

    private bool HasDisplayName()
    {
        return !string.IsNullOrEmpty(NewName);
    }
#endif
}