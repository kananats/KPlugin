using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Constant.Internal;

    public static class FieldInfoExtensionInternal
    {
        public static string AutoGetValue(this FieldInfo field)
        {
            return field.AutoGetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), _ => true);
        }

        public static string AutoGetValue(this FieldInfo field, UnityEngine.Object[] objects)
        {
            return field.AutoGetValue(objects, _ => true);
        }

        public static string AutoGetValue(this FieldInfo field, Func<UnityEngine.Object, bool> predicate)
        {
            return field.AutoGetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), predicate);
        }

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

        public static string AutoSetValue(this FieldInfo field, object value)
        {
            return field.AutoSetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), _ => true, value);
        }

        public static string AutoSetValue(this FieldInfo field, UnityEngine.Object[] objects, object value)
        {
            return field.AutoSetValue(objects, _ => true, value);
        }

        public static string AutoSetValue(this FieldInfo field, Func<UnityEngine.Object, bool> predicate, object value)
        {
            return field.AutoSetValue(UnityEngine.Object.FindObjectsOfType(field.ReflectedType), predicate, value);
        }

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

        private static string AutoSetValueInstance(this FieldInfo field, UnityEngine.Object obj, object value)
        {
            field.SetValue(obj, value);

            return StringConstantInternal.fieldSetValueInstanceSuccess.ReplacedBy(field.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoSetValueStatic(this FieldInfo field, object value)
        {
            field.SetValue(null, value);

            return StringConstantInternal.fieldSetValueStaticSuccess.ReplacedBy(field.Name, value.ToSimpleString());
        }

        public static String GetDescription(this FieldInfo field)
        {
            return "(" + field.FieldType.Name + ") " + field.Name + " { get; set; }";
        }
    }
}
