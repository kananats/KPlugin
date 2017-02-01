namespace KPlugin.Extension
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;
    using Internal;

    public static class ObjectExtension
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList().ForEach(x => dictionary[x.Name] = x.GetValue(obj));

            return dictionary;
        }

        public static string ToSimplifiedString(this object obj, bool showType = false)
        {
            // Null
            if (obj == null)
                return "NULL";

            Type type = obj.GetType();

            // Primitive, String
            if (type.IsPrimitive || type == typeof(string))
                return (showType ? "({0}) ".ReplacedBy(type.Name) : "") + obj;

            // Array
            if (type.IsArray)
            {
                Array array = obj as Array;
                return ArrayExtensionInternal.ToSimplifiedString(array, showType);
            }

            // IDictionary
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                IDictionary dictionary = obj as IDictionary;
                string s = "{";
                foreach (object o in dictionary.Keys)
                    s += "\"{0}\": {1}, ".ReplacedBy(ToSimplifiedString(o, showType), ToSimplifiedString(dictionary[o], showType));

                return s.Length >= 2 ? s.Substring(0, s.Length - 2) + "}" : "{ }";
            }

            // IEnumerable
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                IEnumerable enumerable = obj as IEnumerable;
                string s = "[";
                foreach (object o in enumerable)
                    s += "{0}, ".ReplacedBy(ToSimplifiedString(o, showType));

                return s.Length >= 2 ? s.Substring(0, s.Length - 2) + "]" : "{ }";
            }

            // Class that override ToString()
            if (type.GetMethods().Any(x => x.Name == "ToString" && !x.IsStatic && x.DeclaringType == type))
                return (showType ? "({0})".ReplacedBy(type.Name) : "") + " " + obj.ToString();

            // General Class
            return obj.ToDictionary().ToSimplifiedString(showType);
        }
    }
}
