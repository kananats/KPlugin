namespace KPlugin.Extension
{
    using System;
    using System.Collections;
    using System.Linq;
    using UnityEngine;

    public static class ObjectExtension
    {
        public static string ToSimplifiedString(this object obj, bool showType = false)
        {
            Type type = obj.GetType();

            // Primitive, String
            if (type.IsPrimitive || type == typeof(string))
                return (showType ? "({0}) ".ReplacedBy(type.Name) : "") + obj;

            // Array
            if (type.IsArray)
            {
                Array array = obj as Array;
                /*
                int cols = array.GetLength(array.Rank - 1);
                while (myEnumerator.MoveNext())
                {
                    if (i < cols)
                    {
                        i++;
                    }
                    else
                    {
                        Console.WriteLine();
                        i = 1;
                    }
                    Console.Write("\t{0}", myEnumerator.Current);
                }
                */
                return null;
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

            // Default
            Debug.Log("by default");
            return (showType ? "({0}) ".ReplacedBy(type.Name) : "") + obj;
        }
    }
}
