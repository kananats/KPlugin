using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Constant.Internal;

    public static class MethodBaseExtensionInternal
    {
        public static string AutoInvoke(this MethodBase method, object[] parameters)
        {
            return method.AutoInvoke(UnityEngine.Object.FindObjectsOfType(method.ReflectedType), _ => true, parameters);
        }

        public static string AutoInvoke(this MethodBase method, UnityEngine.Object[] objects, object[] parameters)
        {
            return method.AutoInvoke(objects, _ => true, parameters);
        }

        public static string AutoInvoke(this MethodBase method, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            return method.AutoInvoke(UnityEngine.Object.FindObjectsOfType(method.ReflectedType), predicate, parameters);
        }

        public static string AutoInvoke(this MethodBase method, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();

            int parametersLength = parameters == null ? 0 : parameters.Length;
            int fixedParametersLength = parameterInfos == null ? 0 : parameterInfos.Length;

            object[] fixedParameters = fixedParametersLength == 0 ? null : new object[fixedParametersLength];

            for (int i = 0; i < fixedParametersLength; i++)
                fixedParameters[i] = i < parametersLength ? parameters[i] : parameterInfos[i].DefaultValue;

            if (method.IsStatic)
                return method.AutoInvokeStatic(fixedParameters);

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + method.AutoInvokeInstance(obj, fixedParameters) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        private static string AutoInvokeInstance(this MethodBase method, UnityEngine.Object obj, object[] fixedParameters)
        {
            object value;
            try
            {
                value = method.Invoke(obj, fixedParameters);
            }
            catch (Exception)
            {
                return StringConstantInternal.methodInstanceError.ReplacedBy(method.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            if (value == null)
                return StringConstantInternal.methodVoidInstanceSuccess.ReplacedBy(method.Name, obj.name, obj.GetInstanceID());

            else
                return StringConstantInternal.methodNonVoidInstanceSuccess.ReplacedBy(method.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoInvokeStatic(this MethodBase method, object[] fixedParameters)
        {
            object value;
            try
            {
                value = method.Invoke(null, fixedParameters);
            }
            catch (Exception)
            {
                return StringConstantInternal.methodStaticError.ReplacedBy(method.Name).Color(Color.red);
            }

            if (value == null)
                return StringConstantInternal.methodVoidStaticSuccess.ReplacedBy(method.Name);

            else
                return StringConstantInternal.methodNonVoidStaticSuccess.ReplacedBy(method.Name, value.ToSimpleString());
        }
    }
}
