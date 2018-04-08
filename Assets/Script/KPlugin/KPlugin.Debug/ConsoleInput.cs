﻿namespace KPlugin.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;
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
        private bool focused
        {
            get
            {
                return _focused;
            }

            set
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
                return y => targetIdList == null || targetIdList.Any(z => z == y.GetInstanceID());
            }
        }

        void Awake()
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

        private void OnFocus()
        {
            consoleOutput.visible = true;
        }

        private void OnLosingFocus()
        {
            consoleOutput.visible = false;
        }

        private void InitializeDictionary()
        {
            typeList = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsAbstract && x.IsSealed || !x.IsAbstract && !x.IsInterface && !x.IsGenericType && typeof(MonoBehaviour).IsAssignableFrom(x)).ToList();

            fieldInfoDictionary = new Dictionary<string, FieldInfo>();
            propertyInfoDictionary = new Dictionary<string, PropertyInfo>();
            methodInfoDictionary = new Dictionary<string, List<MethodInfo>>();

            InitializeFieldInfoDictionary();
            InitializePropertyInfoDictionary();
            InitializeMethodInfoDictionary();
        }

        private void InitializeFieldInfoDictionary()
        {
            typeList.ForEach(x =>
            {
                x.GetFields(BindingFlagsConstantInternal.bindingFlags).ToList().ForEach(y =>
                {
                    ConsoleAttribute attribute = y.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string field = attribute.name;
                    if (field == null || !field.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        Debug.LogError(StringConstantInternal.unsupportedFieldNameError.ReplacedBy(field));
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(field) || propertyInfoDictionary.ContainsKey(field) || methodInfoDictionary.ContainsKey(field))
                    {
                        Debug.LogError(StringConstantInternal.duplicatedFieldError.ReplacedBy(field));
                        return;
                    }

                    Type type = y.FieldType;
                    if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                    {
                        Debug.LogError(StringConstantInternal.unsupportedFieldTypeError.ReplacedBy(field));
                        return;
                    }

                    fieldInfoDictionary[field] = y;
                });
            });
        }

        private void InitializePropertyInfoDictionary()
        {
            typeList.ForEach(x =>
            {
                x.GetProperties(BindingFlagsConstantInternal.bindingFlags).ToList().ForEach(y =>
                {
                    ConsoleAttribute attribute = y.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string property = attribute.name;
                    if (property == null || !property.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        Debug.LogError(StringConstantInternal.unsupportedPropertyNameError.ReplacedBy(property));
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(property) || propertyInfoDictionary.ContainsKey(property) || methodInfoDictionary.ContainsKey(property))
                    {
                        Debug.LogError(StringConstantInternal.duplicatedPropertyError.ReplacedBy(property));
                        return;
                    }

                    Type type = y.PropertyType;
                    if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                    {
                        Debug.LogError(StringConstantInternal.unsupportedPropertyTypeError.ReplacedBy(property));
                        return;
                    }

                    propertyInfoDictionary[property] = y;
                });
            });
        }

        private void InitializeMethodInfoDictionary()
        {
            typeList.ForEach(x =>
            {
                x.GetMethods(BindingFlagsConstantInternal.bindingFlags).Where(y => !y.IsAbstract && !y.IsGenericMethod && !y.IsDefined<ExtensionAttribute>()).ToList().ForEach(y =>
                {
                    ConsoleAttribute attribute = y.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string method = attribute.name;
                    if (method == null || !method.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        Debug.LogError(StringConstantInternal.unsupportedMethodNameError.ReplacedBy(method));
                        return;
                    }

                    ParameterInfo[] parameterInfos = y.GetParameters();
                    if (parameterInfos.Length >= 1 && parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>() || parameterInfos.ToList().Any(z =>
                    {
                        Type type = z.ParameterType;
                        return z.IsOut || type.IsByRef || !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
                    }))
                    {
                        Debug.LogError(StringConstantInternal.unsupportedArgumentError.ReplacedBy(method));
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(method) || propertyInfoDictionary.ContainsKey(method))
                    {
                        Debug.LogError(StringConstantInternal.duplicatedMethodError.ReplacedBy(method));
                        return;
                    }

                    if (!methodInfoDictionary.ContainsKey(method))
                        methodInfoDictionary[method] = new List<MethodInfo>();

                    List<MethodInfo> methodInfoList = methodInfoDictionary[method];

                    if (methodInfoList.Any(z =>
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
                        Debug.LogError(StringConstantInternal.duplicatedMethodError.ReplacedBy(method));
                        return;
                    }

                    methodInfoList.Add(y);
                });
            });
        }

        private void Handler(string input)
        {
            if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
                return;

            this.input = input;
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
                Debug.Log(StringConstantInternal.unexpectedInputError);
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
                        Debug.Log(StringConstantInternal.fieldOrPropertyNotFoundError.ReplacedBy(name));

                    break;

                case Command.Method:
                    if (methodInfoDictionary.ContainsKey(name))
                        MethodHandler();

                    else
                        Debug.Log(StringConstantInternal.commandNotFoundError.ReplacedBy(name));

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

            switch (arguments.Length)
            {
                case 0:
                    fieldInfo.AutoGetValue(predicate);
                    return;

                case 1:
                    int compatibility;
                    object boxedArgument = arguments[0].ChangeTypeWithCompatibility(type, out compatibility);

                    if (compatibility < 0)
                    {
                        Debug.Log(StringConstantInternal.fieldTypeMismatchError.ReplacedBy(name));
                        return;
                    }

                    fieldInfo.AutoSetValue(predicate, boxedArgument);
                    return;

                default:
                    Debug.Log(StringConstantInternal.fieldTypeMismatchError.ReplacedBy(name));
                    return;
            }
        }

        private void PropertyHandler()
        {
            PropertyInfo propertyInfo = propertyInfoDictionary[name];
            Type type = propertyInfo.PropertyType;

            switch (arguments.Length)
            {
                case 0:
                    if (!propertyInfo.CanRead)
                    {
                        Debug.Log(StringConstantInternal.accessorNotDefinedError.ReplacedBy("get", name));
                        return;
                    }

                    propertyInfo.AutoGetValue(predicate);
                    return;

                case 1:
                    if (!propertyInfo.CanWrite)
                    {
                        Debug.Log(StringConstantInternal.accessorNotDefinedError.ReplacedBy("set", name));
                        return;
                    }

                    int compatibility;
                    object boxedArgument = arguments[0].ChangeTypeWithCompatibility(type, out compatibility);

                    if (compatibility < 0)
                    {
                        Debug.Log(StringConstantInternal.propertyTypeMismatchError.ReplacedBy(name));
                        return;
                    }

                    propertyInfo.AutoSetValue(predicate, boxedArgument);
                    return;

                default:
                    Debug.Log(StringConstantInternal.propertyTypeMismatchError.ReplacedBy(name));
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
                Debug.Log(StringConstantInternal.argumentTypeMismatchError.ReplacedBy(name));
                ClearInputField();

                return;
            }

            try
            {
                mostCompatibleMethod.AutoInvoke(predicate, mostCompatibleBoxedArguments);
            }
            catch (Exception)
            {
                Debug.Log(StringConstantInternal.methodRuntimeError.ReplacedBy(name));
                return;
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

            input.SplitByWhiteSpaceExceptQuote().ToList().ForEach(x =>
            {
                if (name == null)
                {
                    if (!x.IsMatch(RegexConstant.alphabet))
                        throw new NotSupportedException();

                    switch (x)
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

                            name = x;
                            return;
                    }
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

            if (name == null)
                throw new NotSupportedException();

            arguments = argumentList.ToArray();

            targetIdList = optionDictionary.ContainsKey("id") ? optionDictionary["id"].Select(x => (int)x).ToList() : null;
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
