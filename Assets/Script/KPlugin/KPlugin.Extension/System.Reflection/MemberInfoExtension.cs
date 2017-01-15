namespace KPlugin.Extension
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class MemberInfoExtension
    {
        public static T GetCustomAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            T[] attributes = memberInfo.GetCustomAttributes<T>();
            return attributes.Length == 0 ? null : attributes[0];
        }

        public static T[] GetCustomAttributes<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            T[] attributes = memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
            return attributes;
        }

        public static bool IsDefined<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.IsDefined(typeof(T), inherit);
        }
    }
}
