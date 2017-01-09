namespace KPlugin.Editor
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method)]
    public class SerializeMethodAttribute : Attribute
    {
        public static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public string name
        {
            get;
            private set;
        }

        public Mode mode
        {
            get;
            private set;
        }

        public SerializeMethodAttribute(Mode mode = Mode.All) : this(null, mode) { }

        public SerializeMethodAttribute(string name, Mode mode = Mode.All)
        {
            this.mode = mode;
            this.name = name;
        }
    }
}
