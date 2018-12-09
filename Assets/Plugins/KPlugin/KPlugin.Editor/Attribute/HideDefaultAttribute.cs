using System;

namespace KPlugin.Editor
{
    /// <summary>
    /// A class attribute for hiding default attributes in inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HideDefaultAttribute : Attribute { }
}