namespace KPlugin.Debug
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleMethodAttribute : Attribute
    {
        public static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static readonly string unsupportedMethodNameError = "'{0}' contains unsupported character(s)";
        public static readonly string reservedKeywordError = "'{0}' is used as reserved keyword";
        public static readonly string unsupportedArgumentError = "'{0}' contains unsupported argument type(s)";
        public static readonly string duplicatedMethodError = "'{0}' is already defined with same argument type(s)";
        public static readonly string methodNotFoundError = "'{0}' is not recognized as a command";
        public static readonly string argumentTypeMismatchError = "Argument type mismatch for invoking '{0}'";
        public static readonly string runtimeError = "Unexpected error while running '{0}'";

        public string command
        {
            get;
            private set;
        }

        public ConsoleMethodAttribute(string command)
        {
            this.command = command;
        }
    }
}
