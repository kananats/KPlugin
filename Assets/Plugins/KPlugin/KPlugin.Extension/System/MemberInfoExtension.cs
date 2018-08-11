using System;
using System.Linq;
using System.Reflection;

namespace KPlugin.Extension
{
    public static class MemberInfoExtension
    {
        public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            T[] attributes = member.GetCustomAttributes<T>();
            return attributes.Length == 0 ? null : attributes[0];
        }

        public static T[] GetCustomAttributes<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            T[] attributes = member.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
            return attributes;
        }

        public static bool IsDefined<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member.IsDefined(typeof(T), inherit);
        }
    }
}
