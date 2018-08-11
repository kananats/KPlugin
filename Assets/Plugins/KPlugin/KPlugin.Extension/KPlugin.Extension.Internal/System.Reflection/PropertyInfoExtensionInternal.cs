using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Constant.Internal;

    public static class PropertyInfoExtensionInternal
    {
        public static string AutoGetValue(this PropertyInfo property)
        {
            return property.AutoGetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), _ => true);
        }

        public static string AutoGetValue(this PropertyInfo property, UnityEngine.Object[] objects)
        {
            return property.AutoGetValue(objects, _ => true);
        }

        public static string AutoGetValue(this PropertyInfo property, Func<UnityEngine.Object, bool> predicate)
        {
            return property.AutoGetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), predicate);
        }

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

        public static string AutoSetValue(this PropertyInfo property, object value)
        {
            return property.AutoSetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), _ => true, value);
        }

        public static string AutoSetValue(this PropertyInfo property, UnityEngine.Object[] objects, object value)
        {
            return property.AutoSetValue(objects, _ => true, value);
        }

        public static string AutoSetValue(this PropertyInfo property, Func<UnityEngine.Object, bool> predicate, object value)
        {
            return property.AutoSetValue(UnityEngine.Object.FindObjectsOfType(property.ReflectedType), predicate, value);
        }

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

        public static bool IsStatic(this PropertyInfo property)
        {
            MethodInfo get = property.GetGetMethod(true);
            MethodInfo set = property.GetSetMethod(true);

            return get != null && get.IsStatic || set != null && set.IsStatic;
        }

        public static String GetDescription(this PropertyInfo property)
        {
            return "(" + property.PropertyType.Name + ") " + property.Name + " { " + (property.GetGetMethod(true) != null ? "get; " : "") + (property.GetSetMethod(true) != null ? "set; " : "aaa") + "}";
        }
    }
}
