namespace KPlugin.Editor
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method)]
    public class SerializedMethodAttribute : Attribute
    {
        public static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public string overriddenName
        {
            get;
            private set;
        }

        public bool useInEditMode
        {
            get;
            private set;
        }

        public SerializedMethodAttribute(bool useInEditMode = true) : this(null, useInEditMode) { }

        public SerializedMethodAttribute(string overriddenName, bool useInEditMode = true)
        {
            this.useInEditMode = useInEditMode;
            this.overriddenName = overriddenName;
        }
    }
}
