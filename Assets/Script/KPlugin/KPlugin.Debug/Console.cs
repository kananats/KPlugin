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

        private static readonly string unexpectedTokenError = "'{0}' is not a valid input string";

        private Dictionary<string, List<FieldInfo>> fieldInfoDictionary;
        private Dictionary<string, List<PropertyInfo>> propertyInfoDictionary;
        private Dictionary<string, List<MethodInfo>> methodInfoDictionary;

        private List<Type> typeList;

        private string input;

        private string command;
        private object[] arguments;
        private List<int> targetIdList;
        private List<string> targetNameList;

        void Awake()
        {
            InitializeDictionary();

            inputField.onEndEdit.AddListener(Handler);
        }

        private void InitializeDictionary()
        {
            typeList = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAbstract && x.IsSealed || !x.IsAbstract && !x.IsInterface && !x.IsGenericType && typeof(MonoBehaviour).IsAssignableFrom(x)).ToList();

            InitializeFieldInfoDictionary();
            InitializeMethodInfoDictionary();
        }

        private void InitializeFieldInfoDictionary()
        {

        }

        private void InitializeMethodInfoDictionary()
        {
            methodInfoDictionary = new Dictionary<string, List<MethodInfo>>();

            typeList.ForEach(x =>
            {
                x.GetMethods(ConsoleMethodAttribute.bindingFlags).Where(y => !y.IsAbstract && !y.IsGenericMethod && !y.IsDefined<ExtensionAttribute>()).ToList().ForEach(y =>
                {
                    ConsoleMethodAttribute attribute = y.GetCustomAttribute<ConsoleMethodAttribute>();
                    if (attribute == null)
                        return;

                    string command = attribute.command;
                    if (command == null || !command.IsMatch(RegexConstant.alphabetOrUnderscore))
                    {
                        Debug.LogError(ConsoleMethodAttribute.unsupportedMethodNameError.ReplacedBy(command));
                        return;
                    }

                    if (command == "get" || command == "set")
                    {
                        Debug.LogError(ConsoleMethodAttribute.reservedKeywordError.ReplacedBy(command));
                        return;
                    }

                    ParameterInfo[] parameterInfos = y.GetParameters();
                    if (parameterInfos.Length >= 1 && parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>() || parameterInfos.ToList().Any(z =>
                    {
                        Type type = z.ParameterType;
                        return z.IsOut || type.IsByRef || !type.IsPrimitive && type != typeof(string);
                    }))
                    {
                        Debug.LogError(ConsoleMethodAttribute.unsupportedArgumentError.ReplacedBy(command));
                        return;
                    }

                    if (!methodInfoDictionary.ContainsKey(attribute.command))
                        methodInfoDictionary[attribute.command] = new List<MethodInfo>();

                    List<MethodInfo> list = methodInfoDictionary[attribute.command];

                    if (list.Any(z =>
                    {
                        ParameterInfo[] otherParameterInfos = z.GetParameters();
                        if (parameterInfos.Length != otherParameterInfos.Length)
                            return false;

                        for (int i = 0; i < parameterInfos.Length; i++)
                            if (parameterInfos[i].ParameterType != otherParameterInfos[i].ParameterType)
                                return false;

                        return true;
                    }))
                    {
                        Debug.LogError(ConsoleMethodAttribute.duplicatedMethodError.ReplacedBy(command));
                        return;
                    }

                    list.Add(y);
                });
            });
        }

        private void Handler(string input)
        {
            if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
                return;

            this.input = input;

            command = null;
            arguments = null;
            targetIdList = null;
            targetNameList = null;
            
            try
            {
                Parse();
            }
            catch (Exception)
            {
                Debug.Log(unexpectedTokenError);
                ClearInputField();

                return;
            }
            
            if (command == "get")
            {
                return;
            }

            if (command == "set")
            {
                return;
            }

            if (!methodInfoDictionary.ContainsKey(command))
            {
                Debug.Log(ConsoleMethodAttribute.methodNotFoundError.ReplacedBy(command));
                ClearInputField();

                return;
            }
            
            List<MethodInfo> methodInfoList = methodInfoDictionary[command];

            MethodInfo mostCompatibleMethod = null;
            object[] mostCompatibleBoxedArguments = null;
            int minCompatibility = int.MaxValue;

            foreach (MethodInfo methodInfo in methodInfoList)
            {
                ParameterInfo[] targetArguments = methodInfo.GetParameters();
                if (arguments.Length != targetArguments.Length)
                    continue;

                int compatibility = 0;
                object[] boxedArguments = new object[arguments.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    int argumentCompatibility;
                    boxedArguments[i] = arguments[i].ChangeTypeWithCompatibility(targetArguments[i].ParameterType, out argumentCompatibility);

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
                    mostCompatibleBoxedArguments = boxedArguments;
                    minCompatibility = compatibility;
                }

                if (compatibility == 0)
                    break;
            }

            if (mostCompatibleMethod == null)
            {
                Debug.Log(ConsoleMethodAttribute.argumentTypeMismatchError.ReplacedBy(command));
                ClearInputField();

                return;
            }

            try
            {
                mostCompatibleMethod.AutoInvoke(y => targetIdList == null && targetNameList == null || targetIdList != null && targetIdList.Any(z => z == y.GetInstanceID()) || targetNameList != null && targetNameList.Any(z => z == y.name), mostCompatibleBoxedArguments);
            }
            catch (Exception)
            {
                Debug.Log(ConsoleMethodAttribute.runtimeError.ReplacedBy(command));
                return;
            }
            finally
            {
                ClearInputField();
            }
        }

        private void ClearInputField()
        {
            inputField.ActivateInputField();
            inputField.text = "";
        }

        private void Parse()
        {
            List<object> argumentList = new List<object>();
            Dictionary<string, List<object>> optionDictionary = new Dictionary<string, List<object>>();

            string option = null;

            input.SplitByWhiteSpace().ToList().ForEach(x =>
            {
                if (command == null)
                {
                    if (!x.IsMatch(RegexConstant.alphabet))
                        throw new NotSupportedException();

                    command = x;
                    return;
                }

                if (x[0] == '-' && x.Substring(1).IsMatch(RegexConstant.alphabet))
                {
                    option = x.Substring(1);
                    if (optionDictionary.ContainsKey(option))
                        throw new NotSupportedException();

                    optionDictionary[option] = new List<object>();
                    return;
                }

                if (option == null)
                {
                    argumentList.Add(Box(x));
                    return;
                }

                optionDictionary[option].Add(Box(x));
            });

            arguments = argumentList.ToArray();

            targetIdList = optionDictionary.ContainsKey("id") ? optionDictionary["id"].Select(x => (int)x).ToList() : null;
            targetNameList = optionDictionary.ContainsKey("name") ? optionDictionary["name"].Select(x => (string)x).ToList() : null;
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
