using System;
using System.Linq;
using System.Reflection;

namespace KPlugin.Extension
{
    /// <summary>
    /// A class for adding functionalities to <c>MemberInfo</c>
    /// </summary>
    public static class MemberInfoExtension
    {
        /// <summary>
        /// Get custom attribute
        /// </summary>
        /// <typeparam name="T">Type of the attribute</typeparam>
        /// <param name="member">The member info</param>
        /// <param name="inherit">A <c>bool</c> indicating whether inherited member should be considered</param>
        /// <returns>Custom attribute</returns>
        public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            T[] attributes = member.GetCustomAttributes<T>();
            return attributes.Length == 0 ? null : attributes[0];
        }

        /// <summary>
        /// Get custom attributes
        /// </summary>
        /// <typeparam name="T">Type of the attribute</typeparam>
        /// <param name="member">The member info</param>
        /// <param name="inherit">A <c>bool</c> indicating whether inherited member should be considered</param>
        /// <returns>Custom attribute</param>
        public static T[] GetCustomAttributes<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            T[] attributes = member.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
            return attributes;
        }

        /// <summary>
        /// Checks if attribute is defined
        /// </summary>
        /// <typeparam name="T">Type of the attribute</typeparam>
        /// <param name="member">The member info</param>
        /// <param name="inherit">A <c>bool</c> indicating whether inherited member should be considered</param>
        /// <returns>`true` if the attribute is defined</returns>
        public static bool IsDefined<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            return member.IsDefined(typeof(T), inherit);
        }
    }
}
