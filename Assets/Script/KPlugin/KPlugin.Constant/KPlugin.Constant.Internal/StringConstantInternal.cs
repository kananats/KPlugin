namespace KPlugin.Constant.Internal
{
    public static class StringConstantInternal
    {
        // File Operation
        public static readonly string writeSuccess = "Data was successfully written to '{0}'";
        public static readonly string writeError = "Unexpected error while writing to '{0}'";
        public static readonly string readSuccess = "Data was successfully retrieved from '{0}'";
        public static readonly string readError = "Unexpected error while retrieving data from '{0}'";

        // Console
        public static readonly string unexpectedInputError = "String '{0}' is not valid input string.";
        public static readonly string commandNotFoundError = "String '{0}' is not recognized as a command.";
        public static readonly string fieldOrPropertyNotFoundError = "String '{0}' is not recognized as a field or property.";
        public static readonly string objectNotFoundWarning = "There is no object that matches a criterion.";

        public static readonly string unsupportedFieldNameError = "Field '{0}' contains unsupported character(s).";
        public static readonly string duplicatedFieldError = "Field '{0}' is already defined.";
        public static readonly string unsupportedFieldTypeError = "Field '{0}' is unsupported type.";
        public static readonly string fieldUnexpectedArgumentsError = "Unexpected number of arguments for accessor '{0}' of field '{1}'.";
        public static readonly string fieldTypeMismatchError = "Type mismatch for set field '{0}'.";

        public static readonly string fieldGetValueInstanceError = "Unexpected error occured while getting field '{0}' of '{1}' (ID: {2}).";
        public static readonly string fieldGetValueInstanceSuccess = "Field '{0}' of '{1}' (ID: {2}) is {3}.";

        public static readonly string fieldGetValueStaticError = "Unexpected error occured while getting field '{0}'.";
        public static readonly string fieldGetValueStaticSuccess = "Field '{0}' is {1}.";

        public static readonly string fieldSetValueInstanceError = "Unexpected error occured while setting field '{0}' of '{1}' (ID: {2}).";
        public static readonly string fieldSetValueInstanceSuccess = "Field '{0}' of '{1}' (ID: {2}) is set to {3}.";

        public static readonly string fieldSetValueStaticError = "Unexpected error occured while setting field '{0}'.";
        public static readonly string fieldSetValueStaticSuccess = "Field '{0}' is set to {1}.";

        public static readonly string propertyUnsupportedNameError = "Property '{0}' contains unsupported character(s).";
        public static readonly string propertyDuplicatedError = "Property '{0}' is already defined.";
        public static readonly string propertyUnsupportedTypeError = "Property '{0}' is unsupported type.";
        public static readonly string propertyUndefinedAccessorError = "Accessor '{0}' is not defined for property '{1}'.";
        public static readonly string propertyUnexpectedArgumentsError = "Unexpected number of arguments for accessor '{0}' of property '{1}'.";
        public static readonly string propertyTypeMismatchError = "Type mismatch for set property '{0}'.";

        public static readonly string propertyGetValueInstanceError = "Unexpected error occured while getting property '{0}' of '{1}' (ID: {2}).";
        public static readonly string propertyGetValueInstanceSuccess = "Property '{0}' of '{1}' (ID: {2}) is {3}.";

        public static readonly string propertyGetValueStaticError = "Unexpected error occured while getting property '{0}'.";
        public static readonly string propertyGetValueStaticSuccess = "Property '{0}' is {1}.";

        public static readonly string propertySetValueInstanceError = "Unexpected error occured while setting property '{0}' of '{1}' (ID: {2}).";
        public static readonly string propertySetValueInstanceSuccess = "Property '{0}' of '{1}' (ID: {2}) is set to {3}.";

        public static readonly string propertySetValueStaticError = "Unexpected error occured while setting property '{0}'.";
        public static readonly string propertySetValueStaticSuccess = "Property '{0}' is set to {1}.";

        public static readonly string methodUnsupportedNameError = "Method '{0}' contains unsupported character(s).";
        public static readonly string methodDuplicatedError = "Method '{0}' is already defined with same argument type(s).";
        public static readonly string methodUnsupportedArgumentError = "Method '{0}' contains unsupported argument type(s).";
        public static readonly string methodMismatchArgumentTypeError = "Argument type mismatch for invoking method '{0}'.";

        public static readonly string methodInstanceError = "Unexpected error occurred while executing method '{0}' on '{1}' (ID: {2}).";
        public static readonly string methodVoidInstanceSuccess = "Method '{0}' executed successfully on '{1}' (ID: {2}).";
        public static readonly string methodNonVoidInstanceSuccess = "Method '{0}' executed successfully on '{1}' (ID: {2}) with return value {3}.";

        public static readonly string methodStaticError = "Unexpected error occurred while executing method '{0}'";
        public static readonly string methodVoidStaticSuccess = "Method '{0}' executed successfully.";
        public static readonly string methodNonVoidStaticSuccess = "Method '{0}' executed successfully with return value {1}.";
    }
}
