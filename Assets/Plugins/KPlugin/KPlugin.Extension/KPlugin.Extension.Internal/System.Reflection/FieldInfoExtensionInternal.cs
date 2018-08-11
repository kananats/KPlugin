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
        public static string AutoGetValue(this FieldInfo fieldInfo)
        {
            return fieldInfo.AutoGetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), _ => true);
        }

        public static string AutoGetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects)
        {
            return fieldInfo.AutoGetValue(objects, _ => true);
        }

        public static string AutoGetValue(this FieldInfo fieldInfo, Func<UnityEngine.Object, bool> predicate)
        {
            return fieldInfo.AutoGetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), predicate);
        }

        public static string AutoGetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate)
        {
            if (fieldInfo.IsStatic)
                return fieldInfo.AutoGetValueStatic();

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(x => s = s + fieldInfo.AutoGetValueInstance(x) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        private static string AutoGetValueInstance(this FieldInfo fieldInfo, UnityEngine.Object obj)
        {
            object value;
            try
            {
                value = fieldInfo.GetValue(obj);
            }
            catch (Exception)
            {
                return StringConstantInternal.fieldGetValueInstanceError.ReplacedBy(fieldInfo.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            return StringConstantInternal.fieldGetValueInstanceSuccess.ReplacedBy(fieldInfo.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoGetValueStatic(this FieldInfo fieldInfo)
        {
            object value;
            try
            {
                value = fieldInfo.GetValue(null);
            }
            catch (Exception)
            {
                return StringConstantInternal.fieldGetValueStaticError.ReplacedBy(fieldInfo.Name).Color(Color.red);
            }

            return StringConstantInternal.fieldGetValueStaticSuccess.ReplacedBy(fieldInfo.Name, value.ToSimpleString());
        }

        public static string AutoSetValue(this FieldInfo fieldInfo, object value)
        {
            return fieldInfo.AutoSetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), _ => true, value);
        }

        public static string AutoSetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects, object value)
        {
            return fieldInfo.AutoSetValue(objects, _ => true, value);
        }

        public static string AutoSetValue(this FieldInfo fieldInfo, Func<UnityEngine.Object, bool> predicate, object value)
        {
            return fieldInfo.AutoSetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), predicate, value);
        }

        public static string AutoSetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object value)
        {
            if (fieldInfo.IsStatic)
                return fieldInfo.AutoSetValueStatic(value);

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(x => s = s + fieldInfo.AutoSetValueInstance(x, value) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        private static string AutoSetValueInstance(this FieldInfo fieldInfo, UnityEngine.Object obj, object value)
        {
            fieldInfo.SetValue(obj, value);

            return StringConstantInternal.fieldSetValueInstanceSuccess.ReplacedBy(fieldInfo.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoSetValueStatic(this FieldInfo fieldInfo, object value)
        {
            fieldInfo.SetValue(null, value);

            return StringConstantInternal.fieldSetValueStaticSuccess.ReplacedBy(fieldInfo.Name, value.ToSimpleString());
        }
    }
}
