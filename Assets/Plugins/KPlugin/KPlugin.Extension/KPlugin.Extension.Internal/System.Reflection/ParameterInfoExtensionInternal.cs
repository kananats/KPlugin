﻿using System;
using System.Linq;
using System.Reflection;

namespace KPlugin.Extension.Internal
{
    /// <summary>
    /// An internal class for adding functionalities to <c>Parameter</c>
    /// </summary>
    public static class ParameterInfoExtensionInternal
    {
        /// <summary>
        /// Get custom attribute
        /// </summary>
        /// <typeparam name="T">Type of the attribute</typeparam>
        /// <param name="parameter">The parameter info</param>
        /// <param name="inherit">A <c>bool</c> indicating whether inherited member should be considered</param>
        /// <returns>Custom attribute</returns>
        public static T GetCustomAttribute<T>(this ParameterInfo parameter, bool inherit = true) where T : Attribute
        {
            T[] attributes = parameter.GetCustomAttributes<T>();
            return attributes.Length == 0 ? null : attributes[0];
        }

        /// <summary>
        /// Get custom attributes
        /// </summary>
        /// <typeparam name="T">Type of the attribute</typeparam>
        /// <param name="parameter">The parameter info</param>
        /// <param name="inherit">A <c>bool</c> indicating whether inherited member should be considered</param>
        /// <returns>Custom attributes</returns>
        public static T[] GetCustomAttributes<T>(this ParameterInfo parameter, bool inherit = true) where T : Attribute
        {
            T[] attributes = parameter.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
            return attributes;
        }

        /// <summary>
        /// Checks if attribute is defined
        /// </summary>
        /// <typeparam name="T">Type of the attribute</typeparam>
        /// <param name="parameter">The parameter info</param>
        /// <param name="inherit">A <c>bool</c> indicating whether inherited member should be considered</param>
        /// <returns>`true` if the attribute is defined</returns>
        public static bool IsDefined<T>(this ParameterInfo parameter, bool inherit = true) where T : Attribute
        {
            return parameter.IsDefined(typeof(T), inherit);
        }
    }
}
