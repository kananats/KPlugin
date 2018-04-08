namespace KPlugin.Debug
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class ConsoleAttribute : Attribute
    {

       

        public string name
        {
            get;
            private set;
        }

        public ConsoleAttribute(string name)
        {
            this.name = name;
        }
    }
}
