namespace KPlugin.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Class)]
    public class HideDefaultAttribute : Attribute { }
}