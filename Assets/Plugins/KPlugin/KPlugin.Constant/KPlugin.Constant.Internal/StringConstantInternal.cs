namespace KPlugin.Constant.Internal
{
    /// <summary>
    /// An internal class to declare <c>string</c> constants
    /// </summary>
    public static class StringConstantInternal
    {
        #region File Operation

        /// <summary>
        /// Formatted string for reporting reading success
        /// </summary>
        public static readonly string readSuccess = "Data was successfully retrieved from '{0}'";

        /// <summary>
        /// Formatted string for reporting reading error
        /// </summary>
        public static readonly string readError = "Unexpected error while retrieving data from '{0}'";

        /// <summary>
        /// Formatted string for reporting writing success
        /// </summary>
        public static readonly string writeSuccess = "Data was successfully written to '{0}'";

        /// <summary>
        /// Formatted string for reporting writing error
        /// </summary>
        public static readonly string writeError = "Unexpected error while writing to '{0}'";

        #endregion

        #region Console

        /// <summary>
        /// Formatted string for reporting input error
        /// </summary>
        public static readonly string unexpectedInputError = "String '{0}' is not valid input string.";

        /// <summary>
        /// Formatted string for reporting command not found error
        /// </summary>
        public static readonly string commandNotFoundError = "String '{0}' is not recognized as a command.";

        /// <summary>
        /// Formatted string for reporting field/ property not found error
        /// </summary>
        public static readonly string fieldOrPropertyNotFoundError = "String '{0}' is not recognized as a field or property.";

        /// <summary>
        /// Formatted string for reporting filtered object not found warning
        /// </summary>
        public static readonly string objectNotFoundWarning = "There is no object that matches a criterion.";

        /// <summary>
        /// Formatted string for reporting unsupported field name error
        /// </summary>
        public static readonly string unsupportedFieldNameError = "Field '{0}' contains unsupported character(s).";

        /// <summary>
        /// Formatted string for reporting duplicated field name error
        /// </summary>
        public static readonly string duplicatedFieldError = "Field '{0}' is already defined.";

        /// <summary>
        /// Formatted string for reporting unexpected number of arguments error
        /// </summary>
        public static readonly string fieldUnexpectedArgumentsError = "Unexpected number of arguments for accessor '{0}' of field '{1}'.";

        /// <summary>
        /// Formatted string for reporting unsupported operation error
        /// </summary>
        public static readonly string fieldUnsupportedOperationError = "The operation '{0}' is unsupported for field '{1}'.";

        /// <summary>
        /// Formatted string for reporting SET type mismatch error
        /// </summary>
        public static readonly string fieldTypeMismatchError = "Type mismatch for set field '{0}'.";

        /// <summary>
        /// Formatted string for reporting GET instance value error
        /// </summary>
        public static readonly string fieldGetValueInstanceError = "Unexpected error occured while getting field '{0}' of '{1}' (ID: {2}).";

        /// <summary>
        /// Formatted string for reporting GET instance value success
        /// </summary>
        public static readonly string fieldGetValueInstanceSuccess = "Field '{0}' of '{1}' (ID: {2}) is {3}.";

        /// <summary>
        /// Formatted string for reporting GET static value error
        /// </summary>
        public static readonly string fieldGetValueStaticError = "Unexpected error occured while getting field '{0}'.";

        /// <summary>
        /// Formatted string for reporting GET static value success
        /// </summary>
        public static readonly string fieldGetValueStaticSuccess = "Field '{0}' is {1}.";

        /// <summary>
        /// Formatted string for reporting SET instance value error
        /// </summary>
        public static readonly string fieldSetValueInstanceError = "Unexpected error occured while setting field '{0}' of '{1}' (ID: {2}).";

        /// <summary>
        /// Formatted string for reporting SET instance value success
        /// </summary>
        public static readonly string fieldSetValueInstanceSuccess = "Field '{0}' of '{1}' (ID: {2}) is set to {3}.";

        /// <summary>
        /// Formatted string for reporting SET static value error
        /// </summary>
        public static readonly string fieldSetValueStaticError = "Unexpected error occured while setting field '{0}'.";

        /// <summary>
        /// Formatted string for reporting SET static value success
        /// </summary>
        public static readonly string fieldSetValueStaticSuccess = "Field '{0}' is set to {1}.";

        /// <summary>
        /// Formatted string for reporting unsupported property name error
        /// </summary>
        public static readonly string propertyUnsupportedNameError = "Property '{0}' contains unsupported character(s).";

        /// <summary>
        /// Formatted string for reporting duplicated property name error
        /// </summary>
        public static readonly string propertyDuplicatedError = "Property '{0}' is already defined.";

        /// <summary>
        /// Formatted string for reporting undefined accessor error
        /// </summary>
        public static readonly string propertyUndefinedAccessorError = "Accessor '{0}' is not defined for property '{1}'.";

        /// <summary>
        /// Formatted string for reporting unexpected number of arguments error
        /// </summary>
        public static readonly string propertyUnexpectedArgumentsError = "Unexpected number of arguments for accessor '{0}' of property '{1}'.";

        /// <summary>
        /// Formatted string for reporting unsupported operation error
        /// </summary>
        public static readonly string propertyUnsupportedOperationError = "The operation '{0}' is unsupported for property '{1}'.";

        /// <summary>
        /// Formatted string for reporting SET type mismatch error
        /// </summary>
        public static readonly string propertyTypeMismatchError = "Type mismatch for set property '{0}'.";

        /// <summary>
        /// Formatted string for reporting GET instance value error
        /// </summary>
        public static readonly string propertyGetValueInstanceError = "Unexpected error occured while getting property '{0}' of '{1}' (ID: {2}).";

        /// <summary>
        /// Formatted string for reporting GET instance value success
        /// </summary>
        public static readonly string propertyGetValueInstanceSuccess = "Property '{0}' of '{1}' (ID: {2}) is {3}.";

        /// <summary>
        /// Formatted string for reporting GET static value error
        /// </summary>
        public static readonly string propertyGetValueStaticError = "Unexpected error occured while getting property '{0}'.";

        /// <summary>
        /// Formatted string for reporting GET static value success
        /// </summary>
        public static readonly string propertyGetValueStaticSuccess = "Property '{0}' is {1}.";

        /// <summary>
        /// Formatted string for reporting SET instance value error
        /// </summary>
        public static readonly string propertySetValueInstanceError = "Unexpected error occured while setting property '{0}' of '{1}' (ID: {2}).";

        /// <summary>
        /// Formatted string for reporting SET instance value success
        /// </summary>
        public static readonly string propertySetValueInstanceSuccess = "Property '{0}' of '{1}' (ID: {2}) is set to {3}.";

        /// <summary>
        /// Formatted string for reporting SET static value error
        /// </summary>
        public static readonly string propertySetValueStaticError = "Unexpected error occured while setting property '{0}'.";

        /// <summary>
        /// Formatted string for reporting SET static value success
        /// </summary>
        public static readonly string propertySetValueStaticSuccess = "Property '{0}' is set to {1}.";

        /// <summary>
        /// Formatted string for reporting unsupported method name error
        /// </summary>
        public static readonly string methodUnsupportedNameError = "Method '{0}' contains unsupported character(s).";

        /// <summary>
        /// Formatted string for reporting duplicated method name error
        /// </summary>
        public static readonly string methodDuplicatedError = "Method '{0}' is already defined with same argument type(s).";

        /// <summary>
        /// Formatted string for reporting unsupported argument error
        /// </summary>
        public static readonly string methodUnsupportedArgumentError = "Method '{0}' contains unsupported argument type(s).";

        /// <summary>
        /// Formatted string for reporting argument type mismatch error
        /// </summary>
        public static readonly string methodMismatchArgumentTypeError = "Argument type mismatch for invoking method '{0}'.";

        /// <summary>
        /// Formatted string for reporting instance method invoking error
        /// </summary>
        public static readonly string methodInstanceError = "Unexpected error occurred while executing method '{0}' on '{1}' (ID: {2}).";

        /// <summary>
        /// Formatted string for reporting instance void method invoking success
        /// </summary>
        public static readonly string methodVoidInstanceSuccess = "Method '{0}' executed successfully on '{1}' (ID: {2}).";

        /// <summary>
        /// Formatted string for reporting instance non-void method invoking success
        /// </summary>
        public static readonly string methodNonVoidInstanceSuccess = "Method '{0}' executed successfully on '{1}' (ID: {2}) with return value {3}.";

        /// <summary>
        /// Formatted string for reporting static method invoking error
        /// </summary>
        public static readonly string methodStaticError = "Unexpected error occurred while executing method '{0}'";

        /// <summary>
        /// Formatted string for reporting void static method invoking success
        /// </summary>
        public static readonly string methodVoidStaticSuccess = "Method '{0}' executed successfully.";

        /// <summary>
        /// Formatted string for reporting non-void static method invoking error
        /// </summary>
        public static readonly string methodNonVoidStaticSuccess = "Method '{0}' executed successfully with return value {1}.";

        #endregion
    }
}
