using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KPlugin.Debug.Internal
{
    using Extension;
    using Extension.Internal;
    using Constant;
    using Constant.Internal;

    [RequireComponent(typeof(InputField))]
    public class ConsoleInput : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Console console;

        [SerializeField]
        private ConsoleOutput consoleOutput;

        private Dictionary<string, FieldInfo> fieldInfoDictionary;
        private Dictionary<string, PropertyInfo> propertyInfoDictionary;
        private Dictionary<string, List<MethodInfo>> methodInfoDictionary;

        private List<Type> typeList;

        private string input;

        private List<string> inputHistory;
        private int inputHistoryIndex;

        private Command command;
        private new string name;
        private object[] arguments;
        private List<int> targetIdList;

        private bool _focused;
        public bool focused
        {
            get
            {
                return _focused;
            }

            private set
            {
                if (!_focused && value)
                {
                    _focused = true;
                    OnFocus();
                    return;
                }
                if (focused && !value)
                {
                    _focused = false;
                    OnLosingFocus();
                }
            }
        }

        private Func<UnityEngine.Object, bool> predicate
        {
            get
            {
                return target => targetIdList == null || targetIdList.Any(element => element == target.GetInstanceID());
            }
        }

        public void Initialize()
        {
            _focused = false;
            inputHistory = new List<string>();
            inputHistoryIndex = 0;

            InitializeDictionary();

            inputField.onEndEdit.AddListener(Handler);
        }

        void Update()
        {
            focused = inputField.isFocused;

            if (!focused)
                return;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (inputHistoryIndex <= 0)
                    return;

                if (inputHistoryIndex == inputHistory.Count)
                    input = inputField.text;

                inputField.text = inputHistory[--inputHistoryIndex];
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (inputHistoryIndex >= inputHistory.Count)
                    return;

                if (inputHistoryIndex == inputHistory.Count - 1)
                {
                    inputField.text = input;
                    inputHistoryIndex++;
                    return;
                }

                inputField.text = inputHistory[++inputHistoryIndex];
            }
        }

        public string GetDescription()
        {
            string description = "===FIELD===\n";
            foreach (KeyValuePair<string, FieldInfo> entry in fieldInfoDictionary)
                description = description + entry.Key + ":    " + entry.Value.GetDescription() + "\n";

            description = description + "\n===PROPERTY===\n";
            foreach (KeyValuePair<string, PropertyInfo> entry in propertyInfoDictionary)
                description = description + entry.Key + ":    " + entry.Value.GetDescription() + "\n";

            description = description + "\n===METHOD===\n";

            foreach (KeyValuePair<string, List<MethodInfo>> entry in methodInfoDictionary)
                entry.Value.ForEach(method => description = description + entry.Key + ":    " + method.GetDescription() + "\n");

            return description;
        }

        private void OnFocus()
        {
            consoleOutput.ShowBlackPanel();

            if (console.mode == Mode.Hide || console.mode == Mode.Auto)
                consoleOutput.ShowLog();
        }

        private void OnLosingFocus()
        {
            consoleOutput.HideBlackPanel();

            if (console.mode == Mode.Hide || console.mode == Mode.Auto)
                consoleOutput.HideLog();
        }

        private void InitializeDictionary()
        {
            typeList = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                typeList.AddRange(assembly.GetTypes().Where(type => type.IsAbstract && type.IsSealed || !type.IsAbstract && !type.IsInterface && !type.IsGenericType && typeof(MonoBehaviour).IsAssignableFrom(type)));
            });

            InitializeFieldInfoDictionary();
            InitializePropertyInfoDictionary();           
            InitializeMethodInfoDictionary();
        }

        private void InitializeFieldInfoDictionary()
        {
            fieldInfoDictionary = new Dictionary<string, FieldInfo>();

            typeList.ForEach(type =>
            {
                type.GetFields(BindingFlagsConstantInternal.allBindingFlags).ToList().ForEach(field =>
                {
                    ConsoleAttribute attribute = field.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string name = attribute.name;
                    if (name == null || !name.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        StringConstantInternal.unsupportedFieldNameError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(name) 
                        || propertyInfoDictionary != null && propertyInfoDictionary.ContainsKey(name) 
                        || methodInfoDictionary != null && methodInfoDictionary.ContainsKey(name))
                    {
                        StringConstantInternal.duplicatedFieldError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    fieldInfoDictionary[name] = field;
                });
            });
        }

        private void InitializePropertyInfoDictionary()
        {
            propertyInfoDictionary = new Dictionary<string, PropertyInfo>();

            typeList.ForEach(type =>
            {
                type.GetProperties(BindingFlagsConstantInternal.allBindingFlags).ToList().ForEach(property =>
                {
                    ConsoleAttribute attribute = property.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string name = attribute.name;
                    if (name == null || !name.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        StringConstantInternal.propertyUnsupportedNameError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    if (fieldInfoDictionary != null && fieldInfoDictionary.ContainsKey(name) 
                        || propertyInfoDictionary.ContainsKey(name) 
                        || methodInfoDictionary != null && methodInfoDictionary.ContainsKey(name))
                    {
                        StringConstantInternal.propertyDuplicatedError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    propertyInfoDictionary[name] = property;
                });
            });
        }

        private void InitializeMethodInfoDictionary()
        {
            methodInfoDictionary = new Dictionary<string, List<MethodInfo>>();

            typeList.ForEach(type =>
            {
                type.GetMethods(BindingFlagsConstantInternal.allBindingFlags).Where(method => !method.IsAbstract && !method.IsGenericMethod && !method.IsDefined<ExtensionAttribute>()).ToList().ForEach(method =>
                {
                    ConsoleAttribute attribute = method.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string name = attribute.name;
                    if (name == null || !name.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        StringConstantInternal.methodUnsupportedNameError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    ParameterInfo[] parameterInfos = method.GetParameters();
                    if (parameterInfos.Length >= 1 && parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>() || parameterInfos.ToList().Any(parameter =>
                    {
                        return parameter.IsOut || parameter.ParameterType.IsByRef || !parameter.ParameterType.IsPrimitive && parameter.ParameterType != typeof(string) && !parameter.ParameterType.IsEnum;
                    }))
                    {
                        StringConstantInternal.methodUnsupportedArgumentError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    if (fieldInfoDictionary != null && fieldInfoDictionary.ContainsKey(name) 
                        || propertyInfoDictionary != null && propertyInfoDictionary.ContainsKey(name))
                    {
                        StringConstantInternal.methodDuplicatedError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    if (!methodInfoDictionary.ContainsKey(name))
                        methodInfoDictionary[name] = new List<MethodInfo>();

                    List<MethodInfo> methodInfoList = methodInfoDictionary[name];

                    if (methodInfoList.Any(otherMethod =>
                    {
                        ParameterInfo[] otherParameterInfos = otherMethod.GetParameters();
                        if (parameterInfos.Length != otherParameterInfos.Length)
                            return false;

                        for (int i = 0; i < parameterInfos.Length; i++)
                            if (parameterInfos[i].ParameterType != otherParameterInfos[i].ParameterType)
                                return false;

                        return true;
                    }))
                    {
                        StringConstantInternal.methodDuplicatedError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    methodInfoList.Add(method);
                });
            });
        }

        private void Handler(string input)
        {
            if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
                return;

            this.input = input;
            consoleOutput.Log(("User > " + input).Color(Color.green));

            if (input.Trim() == "")
            {
                ClearInputField();
                return;
            }

            inputHistory.Add(input);
            inputHistoryIndex = inputHistory.Count;

            command = Command.Unknown;
            name = null;
            arguments = null;
            targetIdList = null;

            try
            {
                Parse();
            }
            catch (Exception)
            {
                StringConstantInternal.unexpectedInputError.ReplacedBy(input).LogConsole();
                ClearInputField();

                return;
            }

            switch (command)
            {
                case Command.Get:
                case Command.Set:
                    if (fieldInfoDictionary.ContainsKey(name))
                        FieldHandler();

                    else if (propertyInfoDictionary.ContainsKey(name))
                        PropertyHandler();

                    else
                        StringConstantInternal.fieldOrPropertyNotFoundError.ReplacedBy(name).LogConsole();

                    break;

                case Command.Method:
                    if (methodInfoDictionary.ContainsKey(name))
                        MethodHandler();

                    else
                        StringConstantInternal.commandNotFoundError.ReplacedBy(name).Color(Color.red).LogConsole();

                    break;

                default:
                    break;
            }
            ClearInputField();
        }

        private void FieldHandler()
        {
            FieldInfo fieldInfo = fieldInfoDictionary[name];
            Type type = fieldInfo.FieldType;

            switch (command)
            {
                case Command.Get:
                    if (arguments.Length != 0)
                    {
                        StringConstantInternal.fieldUnexpectedArgumentsError.ReplacedBy("get", fieldInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }
                    fieldInfo.AutoGetValue(predicate).LogConsole();
                    return;

                case Command.Set:
                    if (arguments.Length != 1)
                    {
                        StringConstantInternal.fieldUnexpectedArgumentsError.ReplacedBy("set", fieldInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }

                    if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                    {
                        StringConstantInternal.fieldUnsupportedOperationError.ReplacedBy("set", fieldInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }

                    int compatibility;
                    object boxedArgument = arguments[0].ChangeTypeWithCompatibility(type, out compatibility);

                    if (compatibility < 0)
                    {
                        StringConstantInternal.fieldTypeMismatchError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    fieldInfo.AutoSetValue(predicate, boxedArgument).LogConsole();
                    return;

                default:
                    return;
            }
        }

        private void PropertyHandler()
        {
            PropertyInfo propertyInfo = propertyInfoDictionary[name];
            Type type = propertyInfo.PropertyType;

            switch (command)
            {
                case Command.Get:
                    if (!propertyInfo.CanRead)
                    {
                        StringConstantInternal.propertyUndefinedAccessorError.ReplacedBy("get", propertyInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }
                    if (arguments.Length != 0)
                    {
                        StringConstantInternal.propertyUnexpectedArgumentsError.ReplacedBy("get", propertyInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }

                    propertyInfo.AutoGetValue(predicate).LogConsole();
                    return;

                case Command.Set:
                    if (!propertyInfo.CanWrite)
                    {
                        StringConstantInternal.propertyUndefinedAccessorError.ReplacedBy("set", propertyInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }
                    if (arguments.Length != 1)
                    {
                        StringConstantInternal.propertyUnexpectedArgumentsError.ReplacedBy("set", propertyInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }
                    if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                    {
                        StringConstantInternal.propertyUnsupportedOperationError.ReplacedBy("set", propertyInfo.Name).Color(Color.red).LogConsole();
                        return;
                    }

                    int compatibility;
                    object boxedArgument = arguments[0].ChangeTypeWithCompatibility(type, out compatibility);

                    if (compatibility < 0)
                    {
                        StringConstantInternal.propertyTypeMismatchError.ReplacedBy(name).Color(Color.red).LogConsole();
                        return;
                    }

                    propertyInfo.AutoSetValue(predicate, boxedArgument).LogConsole();
                    return;

                default:
                    return;
            }
        }

        private void MethodHandler()
        {
            List<MethodInfo> methodInfoList = methodInfoDictionary[name];

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
                StringConstantInternal.methodMismatchArgumentTypeError.ReplacedBy(name).Color(Color.red).LogConsole();
                return;
            }

            mostCompatibleMethod.AutoInvoke(predicate, mostCompatibleBoxedArguments).LogConsole();
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

            input.SplitByWhiteSpaceExceptQuote().ToList().ForEach(token =>
            {
                if (name == null)
                {
                    if (!token.IsMatch(RegexConstant.alphabet))
                        throw new NotSupportedException();

                    switch (token)
                    {
                        case "get":
                            command = Command.Get;
                            return;

                        case "set":
                            command = Command.Set;
                            return;

                        default:
                            if (command == Command.Unknown)
                                command = Command.Method;

                            name = token;
                            return;
                    }
                }

                if (token[0] == '-' && token.Substring(1).IsMatch(RegexConstant.alphabet))
                {
                    option = token.Substring(1);
                    if (optionDictionary.ContainsKey(option))
                        throw new NotSupportedException();

                    optionDictionary[option] = new List<object>();
                    return;
                }

                if (option == null)
                {
                    argumentList.Add(Box(token));
                    return;
                }

                optionDictionary[option].Add(Box(token));
            });

            if (name == null)
                throw new NotSupportedException();

            arguments = argumentList.ToArray();

            targetIdList = optionDictionary.ContainsKey("id") ? optionDictionary["id"].Select(id => (int)id).ToList() : null;
        }

        private static object Box(string s)
        {
            string sNoDoubleQuote = s.DoubleQuote(false);
            if (s != sNoDoubleQuote)
                return sNoDoubleQuote;

            bool boolOutput;
            int intOutput;
            float floatOutput;

            return bool.TryParse(s, out boolOutput) ? boolOutput : (int.TryParse(s, out intOutput) ? intOutput : (float.TryParse(s, out floatOutput) ? floatOutput : (object)s));
        }
    }
}
