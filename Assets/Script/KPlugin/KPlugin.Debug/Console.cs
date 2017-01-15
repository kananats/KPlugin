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
    using Constant;

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

                string name;
                object[] arguments;
                List<int> targetIdList;
                List<string> targetNameList;

                try
                {
                    Parse(x, out name, out arguments, out targetIdList, out targetNameList);
                }
                catch (Exception)
                {
                    Debug.Log(ConsoleMethodAttribute.unexpectedTokenError);
                    ClearInputField();

                    return;
                }

                if (!dictionary.ContainsKey(name))
                {
                    Debug.Log(string.Format(ConsoleMethodAttribute.commandNotFoundError, name));
                    ClearInputField();

                    return;
                }

                List<MethodInfo> list = dictionary[name];

                MethodInfo mostCompatibleMethod = null;
                object[] mostCompatibleConvertedArguments = null;
                int minCompatibility = int.MaxValue;

                foreach (MethodInfo methodInfo in list)
                {
                    ParameterInfo[] targetArguments = methodInfo.GetParameters();
                    if (arguments.Length != targetArguments.Length)
                        continue;

                    int compatibility = 0;
                    object[] convertedArguments = new object[arguments.Length];

                    for (int i = 0; i < arguments.Length; i++)
                    {
                        int argumentCompatibility;
                        convertedArguments[i] = arguments[i].ChangeTypeWithCompatibility(targetArguments[i].ParameterType, out argumentCompatibility);

                        if (argumentCompatibility < 0)
                        {
                            compatibility = -1;
                            break;
                        }

                        compatibility += argumentCompatibility;
                    }

                    if (compatibility < 0)
                        continue;

                    if (compatibility < minCompatibility)
                    {
                        mostCompatibleMethod = methodInfo;
                        mostCompatibleConvertedArguments = convertedArguments;
                        minCompatibility = compatibility;
                    }

                    if (compatibility == 0)
                        break;
                }

                if (mostCompatibleMethod == null)
                {
                    Debug.Log(string.Format(ConsoleMethodAttribute.argumentMismatchError, name));
                    ClearInputField();

                    return;
                }

                try
                {
                    mostCompatibleMethod.AutoInvoke(y => targetIdList == null && targetNameList == null || targetIdList != null && targetIdList.Any(z => z == y.GetInstanceID()) || targetNameList != null && targetNameList.Any(z => z == y.name), mostCompatibleConvertedArguments);
                }
                catch (Exception)
                {
                    Debug.Log(string.Format(ConsoleMethodAttribute.runtimeError, name));
                    return;
                }
                finally
                {
                    ClearInputField();
                }
            });
        }

        private void ClearInputField()
        {
            inputField.ActivateInputField();
            inputField.text = "";
        }

        private static void Parse(string s, out string name, out object[] arguments, out List<int> targetIdList, out List<string> targetIdName)
        {
            Dictionary<string, List<object>> optionDictionary;

            Parse(s, out name, out arguments, out optionDictionary);

            targetIdList = optionDictionary.ContainsKey("id") ? optionDictionary["id"].Select(x => (int)x).ToList() : null;
            targetIdName = optionDictionary.ContainsKey("name") ? optionDictionary["name"].Select(x => (string)x).ToList() : null;
        }

        private static void Parse(string s, out string name, out object[] arguments, out Dictionary<string, List<object>> optionDictionary)
        {
            string nameTemp = null;
            List<object> argumentListTemp = argumentListTemp = new List<object>();
            Dictionary<string, List<object>> optionDictionaryTemp = new Dictionary<string, List<object>>();

            string option = null;

            s.SplitByWhiteSpace().ToList().ForEach(x =>
            {
                if (nameTemp == null)
                {
                    if (!x.IsMatch(RegexConstant.alphabet))
                        throw new NotSupportedException();

                    nameTemp = x;
                    return;
                }

                if (x[0] == '-' && x.Substring(1).IsMatch(RegexConstant.alphabet))
                {
                    option = x.Substring(1);
                    if (optionDictionaryTemp.ContainsKey(option))
                        throw new NotSupportedException();

                    optionDictionaryTemp[option] = new List<object>();
                    return;
                }

                if (option == null)
                {
                    argumentListTemp.Add(Box(x));
                    return;
                }

                optionDictionaryTemp[option].Add(Box(x));
            });

            name = nameTemp;
            arguments = argumentListTemp.ToArray();
            optionDictionary = optionDictionaryTemp;
        }

        private static object Box(string s)
        {
            bool boolOutput;
            int intOutput;
            float floatOutput;

            return bool.TryParse(s, out boolOutput) ? boolOutput : (int.TryParse(s, out intOutput) ? intOutput : (float.TryParse(s, out floatOutput) ? floatOutput : (object)s));
        }
    }
}
