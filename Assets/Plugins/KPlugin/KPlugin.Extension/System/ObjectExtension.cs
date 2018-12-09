using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KPlugin.Extension
{
    using Internal;
    using Debug;
    using Constant.Internal;

    /// <summary>
    /// A class for adding functionalities to <c>object</c>
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Makes a dictionary mapping from field name to its value
        /// </summary>
        /// <param name="obj">The object</param>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            obj.GetType().GetFields(BindingFlagsConstantInternal.instanceBindingFlags).ToList().ForEach(field => dictionary[field.Name] = field.GetValue(obj));

            return dictionary;
        }

        /// <summary>
        /// Logs itself to Unity console
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="useSimpleString">A <c>bool</c> indicating whether simple string should be used instead</param>
        public static void Log(this object obj, bool useSimpleString = true)
        {
            obj.Log("{0}", useSimpleString);
        }

        /// <summary>
        /// Logs itself to Unity console
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="format">The formatted string</param>
        /// <param name="useSimpleString">A <c>bool</c> indicating whether simple string should be used instead</param>
        public static void Log(this object obj, string format, bool useSimpleString = true)
        {
            UnityEngine.Debug.Log(format.ReplacedBy(useSimpleString ? obj.ToSimpleString() : obj.ToString()).DoubleQuote(false));
        }

        /// <summary>
        /// Logs itself to console
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="useSimpleString">A <c>bool</c> indicating whether simple string should be used instead</param>
        public static void LogConsole(this object obj, bool useSimpleString = true)
        {
            obj.LogConsole("{0}", useSimpleString);
        }

        /// <summary>
        /// Logs itself to console
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="format">The formatted string</param>
        /// <param name="useSimpleString">A <c>bool</c> indicating whether simple string should be used instead</param>
        public static void LogConsole(this object obj, string format, bool useSimpleString = true)
        {
            Console.Log(format.ReplacedBy(useSimpleString ? obj.ToSimpleString() : obj.ToString()).DoubleQuote(false));
        }

        /// <summary>
        /// Makes a readable string
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="showType">A <c>bool</c> indicating whether the type name should be shown</param>
        public static string ToSimpleString(this object obj, bool showType = false)
        {
            // Null
            if (obj == null)
                return "NULL";

            Type type = obj.GetType();

            // Primitive or Enum
            if (type.IsPrimitive || type.IsEnum)
                return (showType ? "({0}) ".ReplacedBy(type.Name) : "") + obj;

            // String
            if (type == typeof(string))
                return (showType ? "({0}) ".ReplacedBy(type.Name) : "") + ("\"" + obj + "\"");

            // Array
            if (type.IsArray)
            {
                Array array = obj as Array;
                return ArrayExtensionInternal.ToSimpleString(array, showType);
            }

            // IDictionary
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                IDictionary dictionary = obj as IDictionary;
                string s = "{";
                foreach (object o in dictionary.Keys)
                    s += "{0}: {1}, ".ReplacedBy(o.ToSimpleString(showType), dictionary[o].ToSimpleString(showType));

                return s.Length >= 2 ? s.Substring(0, s.Length - 2) + "}" : "{ }";
            }

            // IEnumerable
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                IEnumerable enumerable = obj as IEnumerable;
                string s = "[";
                foreach (object o in enumerable)
                    s += "{0}, ".ReplacedBy(o.ToSimpleString(showType));

                return s.Length >= 2 ? s.Substring(0, s.Length - 2) + "]" : "{ }";
            }

            // Class that overrides ToString()
            if (type.GetMethods().Any(method => method.Name == "ToString" && !method.IsStatic && method.DeclaringType == type))
                return (showType ? "({0}) ".ReplacedBy(type.Name) : "") + obj.ToString();

            // General Class
            return obj.ToDictionary().ToSimpleString(showType);
        }
    }
}
