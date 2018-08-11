using System;
using System.Reflection;

namespace KPlugin.Extension.Internal
{
    public static class MethodInfoExtensionInternal
    {
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
