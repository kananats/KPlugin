using System;
using UnityEngine;

namespace KPlugin.Editor
{
    /// <summary>
    /// A field attribute for making it readonly
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadonlyAttribute : PropertyAttribute { }
}
