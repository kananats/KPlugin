using System;
using System.Linq;
using System.Reflection;

namespace KPlugin.Extension.Internal
{
    public static class ParameterInfoExtensionInternal
    {
        public static T GetCustomAttribute<T>(this ParameterInfo parameter, bool inherit = true) where T : Attribute
        {
            T[] attributes = parameter.GetCustomAttributes<T>();
            return attributes.Length == 0 ? null : attributes[0];
        }

        public static T[] GetCustomAttributes<T>(this ParameterInfo parameter, bool inherit = true) where T : Attribute
        {
            T[] attributes = parameter.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
            return attributes;
        }

        public static bool IsDefined<T>(this ParameterInfo parameter, bool inherit = true) where T : Attribute
        {
            return parameter.IsDefined(typeof(T), inherit);
        }
    }
}
