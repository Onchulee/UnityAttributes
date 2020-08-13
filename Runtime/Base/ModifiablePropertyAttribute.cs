using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ModifiablePropertyAttribute : PropertyAttribute
    {
        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}