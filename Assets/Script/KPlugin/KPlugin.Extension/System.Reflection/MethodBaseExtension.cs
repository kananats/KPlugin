namespace KPlugin.Extension
{
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public static class MethodBaseExtension
    {
        public static void AutoInvoke(this MethodBase methodBase, object[] parameters)
        {
            methodBase.AutoInvoke(Object.FindObjectsOfType(methodBase.ReflectedType), _ => true, parameters);
        }

        public static void AutoInvoke(this MethodBase methodBase, Object[] objects, object[] parameters)
        {
            methodBase.AutoInvoke(objects, _ => true, parameters);
        }

        public static void AutoInvoke(this MethodBase methodBase, System.Func<Object, bool> predicate, object[] parameters)
        {
            methodBase.AutoInvoke(Object.FindObjectsOfType(methodBase.ReflectedType), predicate, parameters);
        }

        public static void AutoInvoke(this MethodBase methodBase, Object[] objects, System.Func<Object, bool> predicate, object[] parameters)
        {
            if (methodBase.IsStatic)
            {
                methodBase.AutoInvokeStatic(parameters);
                return;
            }

            objects.Where(predicate).ToList().ForEach(x => methodBase.AutoInvokeInstance(x, parameters));
        }

        private static void AutoInvokeInstance(this MethodBase methodBase, object obj, object[] parameters)
        {
            object value = methodBase.Invoke(obj, parameters);
            if (value == null)
                return;

            Debug.Log(string.Format("{0}() returns {1}", methodBase.Name, value));
        }

        private static void AutoInvokeStatic(this MethodBase methodBase, object[] parameters)
        {
            object value = methodBase.Invoke(null, parameters);
            if (value == null)
                return;

            Debug.Log(string.Format("{0}() returns {1}", methodBase.Name, value));
            return;
        }
    }
}
