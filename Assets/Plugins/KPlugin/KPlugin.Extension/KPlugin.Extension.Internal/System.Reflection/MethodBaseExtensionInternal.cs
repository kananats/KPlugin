using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KPlugin.Extension.Internal
{
    using Constant.Internal;

    /// <summary>
    /// An internal class for adding functionalities to <c>MethodBase</c>
    /// </summary>
    public static class MethodBaseExtensionInternal
    {
        /// <summary>
        /// Automatically invokes the method based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance method, this will invokes on all objects.
        /// </remarks>
        /// <param name="method">Method to be invoked</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>The string describing result of the execution or error message (discardable)</returns>
        public static string AutoInvoke(this MethodBase method, object[] parameters)
        {
            return method.AutoInvoke(UnityEngine.Object.FindObjectsOfType(method.ReflectedType), _ => true, parameters);
        }

        /// <summary>
        /// Automatically invokes the method based on its type (instance/ static)
        /// </summary>
        /// <param name="method">Method to be invoked</param>
        /// <param name="objects">Target objects</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>The string describing result of the execution or error message (discardable)</returns>
        public static string AutoInvoke(this MethodBase method, UnityEngine.Object[] objects, object[] parameters)
        {
            return method.AutoInvoke(objects, _ => true, parameters);
        }

        /// <summary>
        /// Automatically invokes the method based on its type (instance/ static)
        /// </summary>
        /// <remarks>
        /// For instance method, this will invokes on all objects satisfying the predicate.
        /// </remarks>
        /// <param name="method">Method to be invoked</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>The string describing result of the execution or error message (discardable)</returns>
        public static string AutoInvoke(this MethodBase method, Func<UnityEngine.Object, bool> predicate, object[] parameters)
        {
            return method.AutoInvoke(UnityEngine.Object.FindObjectsOfType(method.ReflectedType), predicate, parameters);
        }

        /// <summary>
        /// Automatically invokes the method based on its type (instance/ static)
        /// </summary>
        /// <param name="method">Method to be invoked</param>
        /// <param name="objects">Target objects</param>
        /// <param name="predicate">Predicate for filtering objects</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>The string describing result of the execution or error message (discardable)</returns>
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

        /// <summary>
        /// Invokes the method on the given instance
        /// </summary>
        /// <param name="method">Method to be invoked</param>
        /// <param name="obj">Target object</param>
        /// <param name="fixedParameters">Boxed parameters</param>
        /// <returns>The string describing result of the execution or error message (discardable)</returns>
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

        /// <summary>
        /// Invokes the static method
        /// </summary>
        /// <param name="method">Method to be invoked</param>
        /// <param name="fixedParameters">Boxed parameters</param>
        /// <returns>The string describing result of the execution or error message (discardable)</returns>
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
