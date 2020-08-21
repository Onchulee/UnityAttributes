using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class ModifiablePropertyAttribute : PropertyAttribute
{
    public override string ToString()
    {
        return this.GetType().Name;
    }
}