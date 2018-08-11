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
        public static string AutoInvoke(this MethodBase methodBase, object[] parameters)
        {
            return methodBase.AutoInvoke(UnityEngine.Object.FindObjectsOfType(methodBase.ReflectedType), _ => true, parameters);
        }

        public static string AutoInvoke(this MethodBase methodBase, UnityEngine.Object[] objects, object[] parameters)
        {
            return methodBase.AutoInvoke(objects, _ => true, parameters);
        }

        public static string AutoInvoke(this MethodBase methodBase, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            return methodBase.AutoInvoke(UnityEngine.Object.FindObjectsOfType(methodBase.ReflectedType), predicate, parameters);
        }

        public static string AutoInvoke(this MethodBase methodBase, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            ParameterInfo[] parameterInfos = methodBase.GetParameters();

            int parametersLength = parameters == null ? 0 : parameters.Length;
            int fixedParametersLength = parameterInfos == null ? 0 : parameterInfos.Length;

            object[] fixedParameters = fixedParametersLength == 0 ? null : new object[fixedParametersLength];

            for (int i = 0; i < fixedParametersLength; i++)
                fixedParameters[i] = i < parametersLength ? parameters[i] : parameterInfos[i].DefaultValue;

            if (methodBase.IsStatic)
                return methodBase.AutoInvokeStatic(fixedParameters);

            List<UnityEngine.Object> objectList = objects.Where(predicate).ToList();

            if (objectList.Count == 0)
                return StringConstantInternal.objectNotFoundWarning.Color(Color.yellow);

            string s = "";
            objectList.ForEach(obj => s = s + methodBase.AutoInvokeInstance(obj, fixedParameters) + "\n");

            return s.Substring(0, s.Length - 1);
        }

        private static string AutoInvokeInstance(this MethodBase methodBase, UnityEngine.Object obj, object[] fixedParameters)
        {
            object value;
            try
            {
                value = methodBase.Invoke(obj, fixedParameters);
            }
            catch (Exception)
            {
                return StringConstantInternal.methodInstanceError.ReplacedBy(methodBase.Name, obj.name, obj.GetInstanceID()).Color(Color.red);
            }

            if (value == null)
                return StringConstantInternal.methodVoidInstanceSuccess.ReplacedBy(methodBase.Name, obj.name, obj.GetInstanceID());

            else
                return StringConstantInternal.methodNonVoidInstanceSuccess.ReplacedBy(methodBase.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString());
        }

        private static string AutoInvokeStatic(this MethodBase methodBase, object[] fixedParameters)
        {
            object value;
            try
            {
                value = methodBase.Invoke(null, fixedParameters);
            }
            catch (Exception)
            {
                return StringConstantInternal.methodStaticError.ReplacedBy(methodBase.Name).Color(Color.red);
            }

            if (value == null)
                return StringConstantInternal.methodVoidStaticSuccess.ReplacedBy(methodBase.Name);

            else
                return StringConstantInternal.methodNonVoidStaticSuccess.ReplacedBy(methodBase.Name, value.ToSimpleString());
        }
    }
}
