using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Debug;
    using Constant.Internal;

    public static class MethodBaseExtensionInternal
    {
        public static void AutoInvoke(this MethodBase methodBase, object[] parameters)
        {
            methodBase.AutoInvoke(UnityEngine.Object.FindObjectsOfType(methodBase.ReflectedType), _ => true, parameters);
        }

        public static void AutoInvoke(this MethodBase methodBase, UnityEngine.Object[] objects, object[] parameters)
        {
            methodBase.AutoInvoke(objects, _ => true, parameters);
        }

        public static void AutoInvoke(this MethodBase methodBase, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            methodBase.AutoInvoke(UnityEngine.Object.FindObjectsOfType(methodBase.ReflectedType), predicate, parameters);
        }

        public static void AutoInvoke(this MethodBase methodBase, UnityEngine.Object[] objects, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            ParameterInfo[] parameterInfos = methodBase.GetParameters();

            int parametersLength = parameters == null ? 0 : parameters.Length;
            int fixedParametersLength = parameterInfos == null ? 0 : parameterInfos.Length;

            object[] fixedParameters = fixedParametersLength == 0 ? null : new object[fixedParametersLength];

            for (int i = 0; i < fixedParametersLength; i++)
                fixedParameters[i] = i < parametersLength ? parameters[i] : parameterInfos[i].DefaultValue;

            if (methodBase.IsStatic)
            {
                methodBase.AutoInvokeStatic(fixedParameters);
                return;
            }

            if (objects.Count(predicate) == 0)
            {
                Console.Log(StringConstantInternal.objectNotFoundWarning.Color(Color.yellow));
                return;
            }

            objects.Where(predicate).ToList().ForEach(x => methodBase.AutoInvokeInstance(x, fixedParameters));
        }

        private static void AutoInvokeInstance(this MethodBase methodBase, UnityEngine.Object obj, object[] fixedParameters)
        {
            object value = methodBase.Invoke(obj, fixedParameters);

            if (value == null)
                Console.Log(StringConstantInternal.methodVoidInstanceMessage.ReplacedBy(methodBase.Name, obj.name, obj.GetInstanceID()));

            else
                Console.Log(StringConstantInternal.methodNonVoidInstanceMessage.ReplacedBy(methodBase.Name, obj.name, obj.GetInstanceID(), value.ToSimpleString()));
        }

        private static void AutoInvokeStatic(this MethodBase methodBase, object[] fixedParameters)
        {
            object value = methodBase.Invoke(null, fixedParameters);

            if (value == null)
                Console.Log(StringConstantInternal.methodVoidStaticMessage.ReplacedBy(methodBase.Name));

            else
                Console.Log(StringConstantInternal.methodNonVoidStaticMessage.ReplacedBy(methodBase.Name, value.ToSimpleString()));
        }
    }
}
