using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Constant.Internal;

    /// <summary>
    /// An internal class for adding functionalities to <c>FieldInfo</c>
    /// </summary>
    public static class FieldInfoExtensionInternal
    {

        /// <summary>
        /// Automatically GET the property values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance field, this will GET values of all objects.
        /// </remarks>
        /// <param name="field">Field to be GET</param>
        /// <returns>The string describing value of the field (discardable)</returns>
        public static string AutoGetValue(this FieldInfo field)
        {
            return field.AutoGetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), _ => true);
        }

        /// <summary>
        /// Automatically GET the field values based on its type (instance/ static)
        /// </summary>
        /// <param name="field">Field to be GET</param>
        /// <param name="objects">Target objects</param>
        /// <returns>The string describing value of the field (discardable)</returns>
        public static string AutoGetValue(this FieldInfo field, UnityEngine.Object[] objects)
        {
            return field.AutoGetValue(objects, _ => true);
        }

        /// <summary>
        /// Automatically GET the field values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance field, this will GET values of all objects satisfying the predicate.
        /// </remarks>
        /// <param name="field">Field to be GET</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <returns>The string describing value of the field (discardable)</returns>
        public static string AutoGetValue(this FieldInfo field, Func<UnityEngine.Object, bool> predicate)
        {
            return field.AutoGetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), predicate);
        }

        /// <summary>
        /// Automatically GET the field values based on its type (instance/ static)
        /// </summary>
        /// <param name="field">Field to be GET</param>
        /// <param name="objects">Target objects</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <returns>The string describing value of the field (discardable)</returns>
        public static string AutoGetValue(this FieldInfo field, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate)
        {
            if (field.IsStatic)
                return field.AutoGetValueStatic();

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + field.AutoGetValueInstance(obj) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        /// <summary>
        /// GET the field value of the given instance
        /// </summary>
        /// <param name="field">Field to be GET</param>
        /// <param name="obj">Target object</param>
        /// <returns>The string describing value of the field or error message (discardable)</returns>
        private static string AutoGetValueInstance(this FieldInfo field, UnityEngine.Object obj)
        {
            object value;
            try
            {
                value = field.GetValue(obj);
            }
            catch (Exception)
            {
                return StringConstantInternal.fieldGetValueInstanceError.ReplacedBy(field.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            return StringConstantInternal.fieldGetValueInstanceSuccess.ReplacedBy(field.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        /// <summary>
        /// GET the static field value
        /// </summary>
        /// <param name="field">Field to be GET</param>
        /// <returns>The string describing value of the field or error message (discardable)</returns>
        private static string AutoGetValueStatic(this FieldInfo field)
        {
            object value;
            try
            {
                value = field.GetValue(null);
            }
            catch (Exception)
            {
                return StringConstantInternal.fieldGetValueStaticError.ReplacedBy(field.Name).Color(Color.red);
            }

            return StringConstantInternal.fieldGetValueStaticSuccess.ReplacedBy(field.Name, value.ToSimpleString());
        }

        /// <summary>
        /// Automatically SET the field values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance field, this will SET values of all objects.
        /// </remarks>
        /// <param name="field">Field to be SET</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET (discardable)</returns>
        public static string AutoSetValue(this FieldInfo field, object value)
        {
            return field.AutoSetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), _ => true, value);
        }

        /// <summary>
        /// Automatically SET the field values based on its type (instance/ static)
        /// </summary>
        /// <param name="field">Field to be SET</param>
        /// <param name="objects">Target objects</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET (discardable)</returns>
        public static string AutoSetValue(this FieldInfo field, UnityEngine.Object[] objects, object value)
        {
            return field.AutoSetValue(objects, _ => true, value);
        }

        /// <summary>
        /// Automatically SET the field values based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance field, this will SET values of all objects satisfying the predicate.
        /// </remarks>
        /// <param name="field">Field to be SET</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET (discardable)</returns>
        public static string AutoSetValue(this FieldInfo field, Func<UnityEngine.Object, bool> predicate, object value)
        {
            return field.AutoSetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), predicate, value);
        }

        /// <summary>
        /// Automatically SET the field values based on its type (instance/ static)
        /// </summary>
        /// <param name="field">Field to be SET</param>
        /// <param name="objects">Target objects</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET (discardable)</returns>
        public static string AutoSetValue(this FieldInfo field, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object value)
        {
            if (field.IsStatic)
                return field.AutoSetValueStatic(value);

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + field.AutoSetValueInstance(obj, value) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        /// <summary>
        /// SET the field value of the given instance
        /// </summary>
        /// <param name="field">Field to be SET</param>
        /// <param name="obj">Target object</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET (discardable)</returns>
        private static string AutoSetValueInstance(this FieldInfo field, UnityEngine.Object obj, object value)
        {
            field.SetValue(obj, value);

            return StringConstantInternal.fieldSetValueInstanceSuccess.ReplacedBy(field.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        /// <summary>
        /// SET the static field value
        /// </summary>
        /// <param name="field">Field to be SET</param>
        /// <param name="value">Value</param>
        /// <returns>The string describing result of the SET (discardable)</returns>
        private static string AutoSetValueStatic(this FieldInfo field, object value)
        {
            field.SetValue(null, value);

            return StringConstantInternal.fieldSetValueStaticSuccess.ReplacedBy(field.Name, value.ToSimpleString());
        }

        /// <summary>
        /// Makes a help description for the field 
        /// </summary>
        /// <param name="field">The field</param>
        /// <returns>The help description of the field</returns>
        public static String GetDescription(this FieldInfo field)
        {
            return "(" + field.FieldType.Name + ") " + field.Name + " { get; set; }";
        }
    }
}
