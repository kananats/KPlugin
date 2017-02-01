namespace KPlugin.Extension.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ParameterInfoExtensionInternal
    {
        public static T GetCustomAttribute<T>(this ParameterInfo parameterInfo, bool inherit = true) where T : Attribute
        {
            T[] attributes = parameterInfo.GetCustomAttributes<T>();
            return attributes.Length == 0 ? null : attributes[0];
        }

        public static T[] GetCustomAttributes<T>(this ParameterInfo parameterInfo, bool inherit = true) where T : Attribute
        {
            T[] attributes = parameterInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
            return attributes;
        }

        public static bool IsDefined<T>(this ParameterInfo parameterInfo, bool inherit = true) where T : Attribute
        {
            return parameterInfo.IsDefined(typeof(T), inherit);
        }
    }
}
