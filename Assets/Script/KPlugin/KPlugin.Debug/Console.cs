namespace KPlugin.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;
    using Extension;

    public class Console : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;

        private Dictionary<string, Dictionary<int, MethodInfo>> dictionary;

        void Awake()
        {
            dictionary = new Dictionary<string, Dictionary<int, MethodInfo>>();

            Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAbstract && x.IsSealed || !x.IsAbstract && !x.IsInterface && !x.IsGenericType && typeof(MonoBehaviour).IsAssignableFrom(x)).ToList().ForEach(x =>
             {
                 x.GetMethods(ConsoleCommandAttribute.bindingFlags).Where(y => !y.IsAbstract && !y.IsGenericMethod && !y.IsDefined<ExtensionAttribute>()).ToList().ForEach(y =>
                 {
                     ConsoleCommandAttribute attribute = y.GetCustomAttribute<ConsoleCommandAttribute>();
                     if (attribute == null || attribute.name == null)
                         return;

                     ParameterInfo[] parameterInfos = y.GetParameters();
                     if (parameterInfos.Length >= 1 && parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>() || parameterInfos.ToList().Any(z =>
                     {
                         Type type = z.ParameterType;
                         return z.IsOut || type.IsByRef || !type.IsPrimitive && type != typeof(string);
                     }))
                         return;

                     if (!dictionary.ContainsKey(attribute.name))
                         dictionary[attribute.name] = new Dictionary<int, MethodInfo>();

                     Dictionary<int, MethodInfo> subDictionary = dictionary[attribute.name];

                     if (subDictionary.ContainsKey(parameterInfos.Length))
                         return;

                     subDictionary[parameterInfos.Length] = y;
                 });
             });

            inputField.onEndEdit.AddListener(x =>
            {
                if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
                    return;

                IEnumerable<string> tokens = x.SplitByWhiteSpace();
                string name = tokens.First();

                if (!dictionary.ContainsKey(name))
                {
                    Debug.Log(string.Format(ConsoleCommandAttribute.commandNotFoundError, name));
                    return;
                }

                Dictionary<int, MethodInfo> subDictionary = dictionary[name];

                string[] parameters = tokens.Skip(1).ToArray();
                int numberOfParameters = parameters.Length;

                if (!subDictionary.ContainsKey(numberOfParameters))
                {
                    Debug.Log(string.Format(ConsoleCommandAttribute.argumentMismatchError, name, numberOfParameters));
                    return;
                }

                object[] boxedParameters = parameters.ToList().Select(y =>
                {
                    bool boolOutput;
                    int intOutput;
                    float floatOutput;

                    if (bool.TryParse(y, out boolOutput))
                        return Convert.ChangeType(boolOutput, typeof(object));

                    else if (int.TryParse(y, out intOutput))
                        return Convert.ChangeType(intOutput, typeof(object));

                    else if (float.TryParse(y, out floatOutput))
                        return Convert.ChangeType(floatOutput, typeof(object));

                    return Convert.ChangeType(y, typeof(object));
                }).ToArray();

                try
                {
                    subDictionary[numberOfParameters].AutoInvoke(boxedParameters);
                }
                catch (Exception)
                {
                    Debug.Log(string.Format(ConsoleCommandAttribute.runtimeError, name));
                }
                finally
                {
                    inputField.ActivateInputField();
                    inputField.text = "";
                }
            });
        }
    }
}
