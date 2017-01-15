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

        private Dictionary<string, List<MethodInfo>> dictionary;

        void Awake()
        {
            dictionary = new Dictionary<string, List<MethodInfo>>();

            Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAbstract && x.IsSealed || !x.IsAbstract && !x.IsInterface && !x.IsGenericType && typeof(MonoBehaviour).IsAssignableFrom(x)).ToList().ForEach(x =>
             {
                 x.GetMethods(ConsoleMethodAttribute.bindingFlags).Where(y => !y.IsAbstract && !y.IsGenericMethod && !y.IsDefined<ExtensionAttribute>()).ToList().ForEach(y =>
                 {
                     ConsoleMethodAttribute attribute = y.GetCustomAttribute<ConsoleMethodAttribute>();
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
                         dictionary[attribute.name] = new List<MethodInfo>();

                     List<MethodInfo> list = dictionary[attribute.name];
                     list.Add(y);
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
                    Debug.Log(string.Format(ConsoleMethodAttribute.commandNotFoundError, name));
                    return;
                }

                List<MethodInfo> list = dictionary[name];

                object[] parameters = tokens.Skip(1).Select(y =>
                {
                    bool boolOutput;
                    int intOutput;
                    float floatOutput;

                    return bool.TryParse(y, out boolOutput) ? boolOutput : (int.TryParse(y, out intOutput) ? intOutput : (float.TryParse(y, out floatOutput) ? floatOutput : (object)y));
                }).ToArray();

                MethodInfo mostCompatibleMethod = null;
                object[] mostCompatibleConvertedParameters = null;
                int minCompatibility = int.MaxValue;

                foreach (MethodInfo methodInfo in list)
                {
                    ParameterInfo[] targetParameters = methodInfo.GetParameters();
                    if (parameters.Length != targetParameters.Length)
                        continue;

                    int compatibility = 0;
                    object[] convertedParameters = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        int parameterCompatibility;
                        convertedParameters[i] = parameters[i].ChangeTypeWithCompatibility(targetParameters[i].ParameterType, out parameterCompatibility);

                        if (parameterCompatibility < 0)
                        {
                            compatibility = -1;
                            break;
                        }

                        compatibility += parameterCompatibility;
                    }

                    if (compatibility < 0)
                        continue;

                    if (compatibility < minCompatibility)
                    {
                        mostCompatibleMethod = methodInfo;
                        mostCompatibleConvertedParameters = convertedParameters;
                        minCompatibility = compatibility;
                    }

                    if (compatibility == 0)
                        break;
                }

                if (mostCompatibleMethod == null)
                {
                    Debug.Log(string.Format(ConsoleMethodAttribute.argumentMismatchError, name));
                    return;
                }

                try
                {
                    mostCompatibleMethod.AutoInvoke(mostCompatibleConvertedParameters);
                }
                catch (Exception)
                {
                    Debug.Log(string.Format(ConsoleMethodAttribute.runtimeError, name));
                    return;
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
