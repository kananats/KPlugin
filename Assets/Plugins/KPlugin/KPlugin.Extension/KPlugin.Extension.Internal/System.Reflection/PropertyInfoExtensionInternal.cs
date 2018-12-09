using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Constant.Internal;

    /// <summary>
    /// An internal class for adding functionalities to <c>PropertyInfo</c>
    /// </summary>
    public static class PropertyInfoExtensionInternal
    {
        /// <summary>
        /// Automatically GET the property values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance property, this will GET values of all objects.
        /// </remarks>
        /// <param name="property">Property to be GET</param>
        /// <returns>The string describing value of the property or error message (discardable)</returns>
        public static string AutoGetValue(this PropertyInfo property)
        {
            return property.AutoGetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), _ => true);
        }

        /// <summary>
        /// Automatically GET the property values based on its type (instance/ static)
        /// </summary>
        /// <param name="property">Property to be GET</param>
        /// <param name="objects">Target objects</param>
        /// <returns>The string describing value of the property or error message (discardable)</returns>
        public static string AutoGetValue(this PropertyInfo property, UnityEngine.Object[] objects)
        {
            return property.AutoGetValue(objects, _ => true);
        }

        /// <summary>
        /// Automatically GET the property values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance property, this will GET values of all objects satisfying the predicate.
        /// </remarks>
        /// <param name="property">Property to be GET</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <returns>The string describing value of the property or error message (discardable)</returns>
        public static string AutoGetValue(this PropertyInfo property, Func<UnityEngine.Object, bool> predicate)
        {
            return property.AutoGetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), predicate);
        }

        /// <summary>
        /// Automatically GET the property values based on its type (instance/ static)
        /// </summary>
        /// <param name="property">Property to be GET</param>
        /// <param name="objects">Target objects</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <returns>The string describing value of the property or error message (discardable)</returns>
        public static string AutoGetValue(this PropertyInfo property, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate)
        {
            if (property.IsStatic())
                return property.AutoGetValueStatic();

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + property.AutoGetValueInstance(obj) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        /// <summary>
        /// GET the property value of the given instance
        /// </summary>
        /// <param name="property">Property to be GET</param>
        /// <param name="obj">Target object</param>
        /// <returns>The string describing value of the property or error message (discardable)</returns>
        private static string AutoGetValueInstance(this PropertyInfo property, UnityEngine.Object obj)
        {
            object value;
            try
            {
                value = property.GetValue(obj, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertyGetValueInstanceError.ReplacedBy(property.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            return StringConstantInternal.propertyGetValueInstanceSuccess.ReplacedBy(property.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        /// <summary>
        /// GET the static property value
        /// </summary>
        /// <param name="property">Property to be GET</param>
        /// <returns>The string describing value of the property or error message (discardable)</returns>
        private static string AutoGetValueStatic(this PropertyInfo property)
        {
            object value;
            try
            {
                value = property.GetValue(null, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertyGetValueStaticError.ReplacedBy(property.Name).Color(Color.red);
            }

            return StringConstantInternal.propertyGetValueStaticSuccess.ReplacedBy(property.Name, value.ToSimpleString());
        }

        /// <summary>
        /// Automatically SET the property values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance property, this will SET values of all objects.
        /// </remarks>
        /// <param name="property">Property to be SET</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET or error message (discardable)</returns>
        public static string AutoSetValue(this PropertyInfo property, object value)
        {
            return property.AutoSetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), _ => true, value);
        }

        /// <summary>
        /// Automatically SET the property values based on its type (instance/ static)
        /// </summary>
        /// <param name="property">Property to be SET</param>
        /// <param name="objects">Target objects</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET or error message (discardable)</returns>
        public static string AutoSetValue(this PropertyInfo property, UnityEngine.Object[] objects, object value)
        {
            return property.AutoSetValue(objects, _ => true, value);
        }

        /// <summary>
        /// Automatically SET the property values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance property, this will SET values of all objects satisfying the predicate.
        /// </remarks>
        /// <param name="property">Property to be SET</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET or error message (discardable)</returns>
        public static string AutoSetValue(this PropertyInfo property, Func<UnityEngine.Object, bool> predicate, object value)
        {
            return property.AutoSetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), predicate, value);
        }

        /// <summary>
        /// Automatically SET the property values based on its type (instance/ static)
        /// </summary>
        /// <param name="property">Property to be SET</param>
        /// <param name="objects">Target objects</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET or error message (discardable)</returns>
        public static string AutoSetValue(this PropertyInfo property, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object value)
        {
            if (property.IsStatic())
                return property.AutoSetValueStatic(value);

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + property.AutoSetValueInstance(obj, value) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        /// <summary>
        /// SET the property value of the given instance
        /// </summary>
        /// <param name="property">Property to be SET</param>
        /// <param name="obj">Target object</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET or error message (discardable)</returns>
        private static string AutoSetValueInstance(this PropertyInfo property, UnityEngine.Object obj, object value)
        {
            try
            {
                property.SetValue(obj, value, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertySetValueInstanceError.ReplacedBy(property.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            return StringConstantInternal.propertySetValueInstanceSuccess.ReplacedBy(property.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        /// <summary>
        /// SET the static property value
        /// </summary>
        /// <param name="property">Property to be SET</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET or error message (discardable)</returns>
        private static string AutoSetValueStatic(this PropertyInfo property, object value)
        {
            try
            {
                property.SetValue(null, value, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertySetValueStaticError.ReplacedBy(property.Name).Color(Color.red);
            }

            return StringConstantInternal.propertySetValueStaticSuccess.ReplacedBy(property.Name, value.ToSimpleString());
        }

        /// <summary>
        /// Check if the property is statically defined
        /// </summary>
        /// <param name="property">Property to be checked</param>
        /// <returns>`true` if `property` is statically defined; otherwise, `false`</returns>
        public static bool IsStatic(this PropertyInfo property)
        {
            MethodInfo get = property.GetGetMethod(true);
            MethodInfo set = property.GetSetMethod(true);

            return get != null && get.IsStatic || set != null && set.IsStatic;
        }

        /// <summary>
        /// Makes a help description for the property 
        /// </summary>
        /// <param name="property">The property</param>
        /// <returns>The help description of the property</returns>
        public static String GetDescription(this PropertyInfo property)
        {
            return "(" + property.PropertyType.Name + ") " + property.Name + " { " + (property.GetGetMethod(true) != null ? "get; " : "") + (property.GetSetMethod(true) != null ? "set; " : "") + "}";
        }
    }
}
