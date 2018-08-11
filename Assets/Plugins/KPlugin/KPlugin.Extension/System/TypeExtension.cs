using System;
using System.Collections.Generic;
using System.Linq;

namespace KPlugin.Extension.Internal
{
    public static class TypeExtension
    {
        public static bool IsGenericSubclass(this Type type, Type genericType)
        {
            return type.GetTypeHierarchy(false).Where(target => target.IsGenericType).Select(target => target.GetGenericTypeDefinition()).Any(target => genericType.Equals(target));
        }

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
