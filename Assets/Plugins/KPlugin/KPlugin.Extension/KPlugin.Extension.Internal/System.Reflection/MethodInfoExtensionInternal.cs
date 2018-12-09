using System;
using System.Reflection;

namespace KPlugin.Extension.Internal
{
    /// <summary>
    /// An internal class for adding functionalities to <c>MethodInfo</c>
    /// </summary>
    public static class MethodInfoExtensionInternal
    {
        /// <summary>
        /// Makes a help description for the method 
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns>The help description of the method</returns>
        public static String GetDescription(this MethodInfo method)
        {
            string description = "(";
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
                description = description + parameters[i].ParameterType.Name + (i == parameters.Length - 1 ? "" : ", ");
            description = description + ")";

            return "(" + method.ReturnType.Name + ") " + method.Name + " " + description;
        }
    }
}
