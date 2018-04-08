namespace KPlugin.Extension.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Debug;
    using Constant.Internal;

    public static class FieldInfoExtensionInternal
    {
        public static void AutoGetValue(this FieldInfo fieldInfo)
        {
            fieldInfo.AutoGetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), _ => true);
        }

        public static void AutoGetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects)
        {
            fieldInfo.AutoGetValue(objects, _ => true);
        }

        public static void AutoGetValue(this FieldInfo fieldInfo, Func<UnityEngine.Object, bool> predicate)
        {
            fieldInfo.AutoGetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), predicate);
        }

        public static void AutoGetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate)
        {
            if (fieldInfo.IsStatic)
            {
                fieldInfo.AutoGetValueStatic();
                return;
            }

            objects.Where(predicate).ToList().ForEach(x => fieldInfo.AutoGetValueInstance(x));
        }

        private static void AutoGetValueInstance(this FieldInfo fieldInfo, object obj)
        {
            object value = fieldInfo.GetValue(obj);

            KPlugin.Debug.Console.Log(StringConstantInternal.fieldGetMessage.ReplacedBy(fieldInfo.Name, value));
        }

        private static void AutoGetValueStatic(this FieldInfo fieldInfo)
        {
            object value = fieldInfo.GetValue(null);

            KPlugin.Debug.Console.Log(StringConstantInternal.fieldGetMessage.ReplacedBy(fieldInfo.Name, value));
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, object value)
        {
            fieldInfo.AutoSetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), _ => true, value);
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects, object value)
        {
            fieldInfo.AutoSetValue(objects, _ => true, value);
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, Func<UnityEngine.Object, bool> predicate, object value)
        {
            fieldInfo.AutoSetValue(UnityEngine.Object.FindObjectsOfType(fieldInfo.ReflectedType), predicate, value);
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object value)
        {
            if (fieldInfo.IsStatic)
            {
                fieldInfo.AutoSetValueStatic(value);
                return;
            }

            objects.Where(predicate).ToList().ForEach(x => fieldInfo.AutoSetValueInstance(x, value));
        }

        private static void AutoSetValueInstance(this FieldInfo fieldInfo, object obj, object value)
        {
            fieldInfo.SetValue(obj, value);
        }

        private static void AutoSetValueStatic(this FieldInfo fieldInfo, object value)
        {
            fieldInfo.SetValue(null, value);
        }
    }
}
