namespace KPlugin.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TypeExtension
    {
        public static bool IsGenericSubclass(this Type type, Type genericType)
        {
            return type.GetTypeHierarchy(false).Where(t => t.IsGenericType).Select(t => t.GetGenericTypeDefinition()).Any(t => genericType.Equals(t));
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
