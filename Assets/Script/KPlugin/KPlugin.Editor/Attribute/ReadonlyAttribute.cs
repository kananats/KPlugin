namespace KPlugin.Editor
{
    using UnityEngine;
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class ReadonlyAttribute : PropertyAttribute { }
}
