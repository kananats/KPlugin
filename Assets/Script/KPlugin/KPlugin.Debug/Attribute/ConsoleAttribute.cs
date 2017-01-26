namespace KPlugin.Debug
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class ConsoleAttribute : Attribute
    {
        public static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        
        // General
        public static readonly string UnexpectedInputError = "String '{0}' is not valid input string";
        public static readonly string CommandNotFoundError = "String '{0}' is not recognized as command";

        // Field
        public static readonly string UnsupportedFieldNameError = "Field '{0}' contains unsupported character(s)";
        public static readonly string DuplicatedFieldError = "Field '{0}' is already defined";
        public static readonly string UnsupportedFieldTypeError = "Field '{0}' is unsupported type";
        public static readonly string FieldTypeMismatchError = "Type mismatch for set field '{0}'";
        public static readonly string FieldGetMessage = "Field '{0}' is {1}";

        // Property
        public static readonly string UnsupportedPropertyNameError = "Property '{0}' contains unsupported character(s)";
        public static readonly string DuplicatedPropertyError = "Property '{0}' is already defined";
        public static readonly string UnsupportedPropertyTypeError = "Property '{0}' is unsupported type";
        public static readonly string PropertyTypeMismatchError = "Type mismatch for set property '{0}'";
        public static readonly string AccessorNotDefinedError = "Accessor '{0}' is not defined for property '{1}'";
        public static readonly string PropertyGetMessage = "Property '{0}' is {1}";

        // Method
        public static readonly string UnsupportedMethodNameError = "Method '{0}' contains unsupported character(s)";
        public static readonly string DuplicatedMethodError = "Method '{0}' is already defined with same argument type(s)";
        public static readonly string UnsupportedArgumentError = "Method '{0}' contains unsupported argument type(s)";
        public static readonly string ArgumentTypeMismatchError = "Argument type mismatch for invoking method '{0}'";
        public static readonly string MethodRuntimeError = "Unexpected error while running method '{0}'";
        public static readonly string MethodReturnMessage = "Method '{0}' returns {1}";

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
