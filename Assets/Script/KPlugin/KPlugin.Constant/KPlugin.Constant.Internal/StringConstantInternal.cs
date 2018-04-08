namespace KPlugin.Constant.Internal
{
    public static class StringConstantInternal
    {
        // General
        public static readonly string unexpectedInputError = "String '{0}' is not valid input string.";
        public static readonly string commandNotFoundError = "String '{0}' is not recognized as a command.";
        public static readonly string fieldOrPropertyNotFoundError = "String '{0}' is not recognized as a field or property.";
        public static readonly string objectNotFoundWarning = "There is no object that matches a criterion.";
        public static readonly string writeSuccessfully = "Data was successfully written to '{0}'";
        public static readonly string writeUnsuccessfully = "Unexpected error while writing to '{0}'";
        public static readonly string readSuccessfully = "Data was successfully retrieved from '{0}'";
        public static readonly string readUnsuccessfully = "Unexpected error while retrieving data from '{0}'";

        // Field
        public static readonly string unsupportedFieldNameError = "Field '{0}' contains unsupported character(s).";
        public static readonly string duplicatedFieldError = "Field '{0}' is already defined.";
        public static readonly string unsupportedFieldTypeError = "Field '{0}' is unsupported type.";
        public static readonly string fieldTypeMismatchError = "Type mismatch for set field '{0}'.";

        public static readonly string fieldGetValueInstanceMessage = "Field '{0}' of '{1}' (ID: {2}) is {3}.";
        public static readonly string fieldGetValueStaticMessage = "Field '{0}' is {1}.";
        public static readonly string fieldSetValueInstanceMessage = "Field '{0}' of '{1}' (ID: {2}) is set to {3}.";
        public static readonly string fieldSetValueStaticMessage = "Field '{0}' is set to {1}.";

        // Property
        public static readonly string unsupportedPropertyNameError = "Property '{0}' contains unsupported character(s).";
        public static readonly string duplicatedPropertyError = "Property '{0}' is already defined.";
        public static readonly string unsupportedPropertyTypeError = "Property '{0}' is unsupported type.";
        public static readonly string propertyTypeMismatchError = "Type mismatch for set property '{0}'.";
        public static readonly string accessorNotDefinedError = "Accessor '{0}' is not defined for property '{1}'.";
        public static readonly string propertyGetValueInstanceMessage = "Property '{0}' of '{1}' (ID: {2}) is {3}.";
        public static readonly string propertyGetValueStaticMessage = "Property '{0}' is {1}.";
        public static readonly string propertySetValueInstanceMessage = "Property '{0}' of '{1}' (ID: {2}) is set to {3}.";
        public static readonly string propertySetValueStaticMessage = "Property '{0}' is set to {1}.";

        // Method
        public static readonly string unsupportedMethodNameError = "Method '{0}' contains unsupported character(s).";
        public static readonly string duplicatedMethodError = "Method '{0}' is already defined with same argument type(s).";
        public static readonly string unsupportedArgumentError = "Method '{0}' contains unsupported argument type(s).";
        public static readonly string argumentTypeMismatchError = "Argument type mismatch for invoking method '{0}'.";
        public static readonly string methodRuntimeError = "Unexpected error while running method '{0}'.";
        public static readonly string methodVoidInstanceMessage = "Method '{0}' executes successfully on '{1}' (ID: {2}).";
        public static readonly string methodNonVoidInstanceMessage = "Method '{0}' executes successfully on '{1}' (ID: {2}) with return value {3}.";
        public static readonly string methodVoidStaticMessage = "Method '{0}' executes successfully.";
        public static readonly string methodNonVoidStaticMessage = "Method '{0}' executes successfully  with return value {1}.";
    }
}
