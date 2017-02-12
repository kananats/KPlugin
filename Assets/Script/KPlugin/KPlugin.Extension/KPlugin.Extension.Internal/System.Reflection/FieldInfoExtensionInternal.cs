namespace KPlugin.Extension.Internal
{
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Debug;

    public static class FieldInfoExtensionInternal
    {
        public static void AutoGetValue(this FieldInfo fieldInfo)
        {
            fieldInfo.AutoGetValue(Object.FindObjectsOfType(fieldInfo.ReflectedType), _ => true);
        }

        public static void AutoGetValue(this FieldInfo fieldInfo, Object[] objects)
        {
            fieldInfo.AutoGetValue(objects, _ => true);
        }

        public static void AutoGetValue(this FieldInfo fieldInfo, System.Func<Object, bool> predicate)
        {
            fieldInfo.AutoGetValue(Object.FindObjectsOfType(fieldInfo.ReflectedType), predicate);
        }

        public static void AutoGetValue(this FieldInfo fieldInfo, Object[] objects, System.Func<Object, bool> predicate)
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

            Debug.Log(ConsoleAttribute.fieldGetMessage.ReplacedBy(fieldInfo.Name, value));
        }

        private static void AutoGetValueStatic(this FieldInfo fieldInfo)
        {
            object value = fieldInfo.GetValue(null);

            Debug.Log(ConsoleAttribute.fieldGetMessage.ReplacedBy(fieldInfo.Name, value));
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, object value)
        {
            fieldInfo.AutoSetValue(Object.FindObjectsOfType(fieldInfo.ReflectedType), _ => true, value);
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, Object[] objects, object value)
        {
            fieldInfo.AutoSetValue(objects, _ => true, value);
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, System.Func<Object, bool> predicate, object value)
        {
            fieldInfo.AutoSetValue(Object.FindObjectsOfType(fieldInfo.ReflectedType), predicate, value);
        }

        public static void AutoSetValue(this FieldInfo fieldInfo, Object[] objects, System.Func<Object, bool> predicate, object value)
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
