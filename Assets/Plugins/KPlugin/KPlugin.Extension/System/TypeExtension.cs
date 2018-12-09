using System;
using System.Collections.Generic;
using System.Linq;

namespace KPlugin.Extension.Internal
{
    /// <summary>
    /// An internal class for adding functionalities to <c>Type</c>
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Check if this type is generic subclass
        /// </summary>
        public static bool IsGenericSubclass(this Type type, Type genericType)
        {
            return type.GetTypeHierarchy(false).Where(target => target.IsGenericType).Select(target => target.GetGenericTypeDefinition()).Any(target => genericType.Equals(target));
        }

        /// <summary>
        /// Makes type hierarchy
        /// </summary>
        /// <param name="includingSelf">A <c>bool</c> indicating whether this type should be included</param>
        public static IEnumerable<Type> GetTypeHierarchy(this Type type, bool includingSelf = true)
        {
            Type currentType = includingSelf ? type : type.BaseType;

            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }
    }
}
