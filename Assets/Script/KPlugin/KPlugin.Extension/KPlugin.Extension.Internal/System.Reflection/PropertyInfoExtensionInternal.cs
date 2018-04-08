namespace KPlugin.Extension.Internal
{
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Debug;
    using Constant.Internal;

    public static class PropertyInfoExtensionInternal
    {
        public static void AutoGetValue(this PropertyInfo propertyInfo)
        {
            propertyInfo.AutoGetValue(Object.FindObjectsOfType(propertyInfo.ReflectedType), _ => true);
        }

        public static void AutoGetValue(this PropertyInfo propertyInfo, Object[] objects)
        {
            propertyInfo.AutoGetValue(objects, _ => true);
        }

        public static void AutoGetValue(this PropertyInfo propertyInfo, System.Func<Object, bool> predicate)
        {
            propertyInfo.AutoGetValue(Object.FindObjectsOfType(propertyInfo.ReflectedType), predicate);
        }

        public static void AutoGetValue(this PropertyInfo propertyInfo, Object[] objects, System.Func<Object, bool> predicate)
        {
            if (propertyInfo.IsStatic())
            {
                propertyInfo.AutoGetValueStatic();
                return;
            }

            if (objects.Count(predicate) == 0)
            {
                Console.Log(StringConstantInternal.objectNotFoundWarning.Color(Color.yellow));
                return;
            }

            objects.Where(predicate).ToList().ForEach(x => propertyInfo.AutoGetValueInstance(x));
        }

        private static void AutoGetValueInstance(this PropertyInfo propertyInfo, Object obj)
        {
            object value = propertyInfo.GetValue(obj, null);

            Console.Log(StringConstantInternal.propertyGetValueInstanceMessage.ReplacedBy(propertyInfo.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString()));
        }

        private static void AutoGetValueStatic(this PropertyInfo propertyInfo)
        {
            object value = propertyInfo.GetValue(null, null);

            Console.Log(StringConstantInternal.propertyGetValueStaticMessage.ReplacedBy(propertyInfo.Name, value.ToSimpleString()));
        }

        public static void AutoSetValue(this PropertyInfo propertyInfo, object value)
        {
            propertyInfo.AutoSetValue(Object.FindObjectsOfType(propertyInfo.ReflectedType), _ => true, value);
        }

        public static void AutoSetValue(this PropertyInfo propertyInfo, Object[] objects, object value)
        {
            propertyInfo.AutoSetValue(objects, _ => true, value);
        }

        public static void AutoSetValue(this PropertyInfo propertyInfo, System.Func<Object, bool> predicate, object value)
        {
            propertyInfo.AutoSetValue(Object.FindObjectsOfType(propertyInfo.ReflectedType), predicate, value);
        }

        public static void AutoSetValue(this PropertyInfo propertyInfo, Object[] objects, System.Func<Object, bool> predicate, object value)
        {
            if (propertyInfo.IsStatic())
            {
                propertyInfo.AutoSetValueStatic(value);
                return;
            }

            if (objects.Count(predicate) == 0)
            {
                Console.Log(StringConstantInternal.objectNotFoundWarning.Color(Color.yellow));
                return;
            }

            objects.Where(predicate).ToList().ForEach(x => propertyInfo.AutoSetValueInstance(x, value));
        }

        private static void AutoSetValueInstance(this PropertyInfo propertyInfo, Object obj, object value)
        {
            propertyInfo.SetValue(obj, value, null);

            Console.Log(StringConstantInternal.propertySetValueInstanceMessage.ReplacedBy(propertyInfo.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString()));
        }

        private static void AutoSetValueStatic(this PropertyInfo propertyInfo, object value)
        {
            propertyInfo.SetValue(null, value, null);

            Console.Log(StringConstantInternal.propertySetValueInstanceMessage.ReplacedBy(propertyInfo.Name, value.ToSimpleString()));
        }

        public static bool IsStatic(this PropertyInfo propertyInfo)
        {
            MethodInfo get = propertyInfo.GetGetMethod();
            MethodInfo set = propertyInfo.GetSetMethod();

            return get != null && get.IsStatic || set != null && set.IsStatic;
        }
    }
}
