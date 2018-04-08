namespace KPlugin.Constant.Internal
{
    public static class StringConstantInternal
    {
        // General
        public static readonly string unexpectedInputError = "String '{0}' is not valid input string";
        public static readonly string commandNotFoundError = "String '{0}' is not recognized as a command";
        public static readonly string fieldOrPropertyNotFoundError = "'{0}' is not recognized as a field or property";

        // Field
        public static readonly string unsupportedFieldNameError = "Field '{0}' contains unsupported character(s)";
        public static readonly string duplicatedFieldError = "Field '{0}' is already defined";
        public static readonly string unsupportedFieldTypeError = "Field '{0}' is unsupported type";
        public static readonly string fieldTypeMismatchError = "Type mismatch for set field '{0}'";
        public static readonly string fieldGetMessage = "Field '{0}' is {1}";

        // Property
        public static readonly string unsupportedPropertyNameError = "Property '{0}' contains unsupported character(s)";
        public static readonly string duplicatedPropertyError = "Property '{0}' is already defined";
        public static readonly string unsupportedPropertyTypeError = "Property '{0}' is unsupported type";
        public static readonly string propertyTypeMismatchError = "Type mismatch for set property '{0}'";
        public static readonly string accessorNotDefinedError = "Accessor '{0}' is not defined for property '{1}'";
        public static readonly string propertyGetMessage = "Property '{0}' is {1}";

        // Method
        public static readonly string unsupportedMethodNameError = "Method '{0}' contains unsupported character(s)";
        public static readonly string duplicatedMethodError = "Method '{0}' is already defined with same argument type(s)";
        public static readonly string unsupportedArgumentError = "Method '{0}' contains unsupported argument type(s)";
        public static readonly string argumentTypeMismatchError = "Argument type mismatch for invoking method '{0}'";
        public static readonly string methodRuntimeError = "Unexpected error while running method '{0}'";
        public static readonly string methodReturnMessage = "Method '{0}' returns {1}";
    }
}
