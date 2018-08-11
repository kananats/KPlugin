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
        public static string AutoGetValue(this PropertyInfo propertyInfo)
        {
            return propertyInfo.AutoGetValue(UnityEngine.Object.FindObjectsOfType(propertyInfo.ReflectedType), _ => true);
        }

        public static string AutoGetValue(this PropertyInfo propertyInfo, UnityEngine.Object[] objects)
        {
            return propertyInfo.AutoGetValue(objects, _ => true);
        }

        public static string AutoGetValue(this PropertyInfo propertyInfo, Func<UnityEngine.Object, bool> predicate)
        {
            return propertyInfo.AutoGetValue(UnityEngine.Object.FindObjectsOfType(propertyInfo.ReflectedType), predicate);
        }

        public static string AutoGetValue(this PropertyInfo propertyInfo, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate)
        {
            if (propertyInfo.IsStatic())
                return propertyInfo.AutoGetValueStatic();

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + propertyInfo.AutoGetValueInstance(obj) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        private static string AutoGetValueInstance(this PropertyInfo propertyInfo, UnityEngine.Object obj)
        {
            object value;
            try
            {
                value = propertyInfo.GetValue(obj, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertyGetValueInstanceError.ReplacedBy(propertyInfo.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            return StringConstantInternal.propertyGetValueInstanceSuccess.ReplacedBy(propertyInfo.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoGetValueStatic(this PropertyInfo propertyInfo)
        {
            object value;
            try
            {
                value = propertyInfo.GetValue(null, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertyGetValueStaticError.ReplacedBy(propertyInfo.Name).Color(Color.red);
            }

            return StringConstantInternal.propertyGetValueStaticSuccess.ReplacedBy(propertyInfo.Name, value.ToSimpleString());
        }

        public static string AutoSetValue(this PropertyInfo propertyInfo, object value)
        {
            return propertyInfo.AutoSetValue(UnityEngine.Object.FindObjectsOfType(propertyInfo.ReflectedType), _ => true, value);
        }

        public static string AutoSetValue(this PropertyInfo propertyInfo, UnityEngine.Object[] objects, object value)
        {
            return propertyInfo.AutoSetValue(objects, _ => true, value);
        }

        public static string AutoSetValue(this PropertyInfo propertyInfo, Func<UnityEngine.Object, bool> predicate, object value)
        {
            return propertyInfo.AutoSetValue(UnityEngine.Object.FindObjectsOfType(propertyInfo.ReflectedType), predicate, value);
        }

        public static string AutoSetValue(this PropertyInfo propertyInfo, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object value)
        {
            if (propertyInfo.IsStatic())
                return propertyInfo.AutoSetValueStatic(value);

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + propertyInfo.AutoSetValueInstance(obj, value) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        private static string AutoSetValueInstance(this PropertyInfo propertyInfo, UnityEngine.Object obj, object value)
        {
            try
            {
                propertyInfo.SetValue(obj, value, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertySetValueInstanceError.ReplacedBy(propertyInfo.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            return StringConstantInternal.propertySetValueInstanceSuccess.ReplacedBy(propertyInfo.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoSetValueStatic(this PropertyInfo propertyInfo, object value)
        {
            try
            {
                propertyInfo.SetValue(null, value, null);
            }
            catch (Exception)
            {
                return StringConstantInternal.propertySetValueStaticError.ReplacedBy(propertyInfo.Name).Color(Color.red);
            }

            return StringConstantInternal.propertySetValueStaticSuccess.ReplacedBy(propertyInfo.Name, value.ToSimpleString());
        }

        public static bool IsStatic(this PropertyInfo propertyInfo)
        {
            MethodInfo get = propertyInfo.GetGetMethod();
            MethodInfo set = propertyInfo.GetSetMethod();

            return get != null && get.IsStatic || set != null && set.IsStatic;
        }
    }
}
