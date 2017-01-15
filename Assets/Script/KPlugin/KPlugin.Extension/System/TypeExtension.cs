namespace KPlugin.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TypeExtension
    {
        public static bool IsGenericSubclass(this Type type, Type genericType)
        {
            return type.GetTypeHierarchy(false).Where(x => x.IsGenericType).Select(x => x.GetGenericTypeDefinition()).Any(x => genericType.Equals(x));
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
