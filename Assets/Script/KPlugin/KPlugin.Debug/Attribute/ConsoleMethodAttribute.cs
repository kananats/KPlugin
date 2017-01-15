namespace KPlugin.Debug
{
    using System;
    using System.Reflection;
    using Extension;
    using Constant;

    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleMethodAttribute : Attribute
    {
        public static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static string commandNotFoundError = "'{0}' is not recognized as a command";
        public static string argumentMismatchError = "argument mismatch for invoking '{0}'";
        public static string runtimeError = "unexpected error while running '{0}'";

        public string name
        {
            get;
            private set;
        }

        public ConsoleMethodAttribute(string name)
        {
            this.name = name.IsMatch(RegexConstant.alphanumeric) ? name : null;
        }
    }
}
