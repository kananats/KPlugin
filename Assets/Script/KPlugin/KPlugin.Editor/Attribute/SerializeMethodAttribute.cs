namespace KPlugin.Editor
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class SerializeMethodAttribute : Attribute
    {
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
