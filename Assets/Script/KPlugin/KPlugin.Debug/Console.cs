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

        private Dictionary<string, FieldInfo> fieldInfoDictionary;
        private Dictionary<string, PropertyInfo> propertyInfoDictionary;
        private Dictionary<string, List<MethodInfo>> methodInfoDictionary;

        private List<Type> typeList;

        private string input;

        private new string name;
        private object[] arguments;
        private List<int> targetIdList;
        private List<string> targetNameList;

        private Func<UnityEngine.Object, bool> predicate
        {
            get
            {
                return y => targetIdList == null && targetNameList == null || targetIdList != null && targetIdList.Any(z => z == y.GetInstanceID()) || targetNameList != null && targetNameList.Any(z => z == y.name);
            }
        }

        void Awake()
        {
            InitializeDictionary();

            inputField.onEndEdit.AddListener(Handler);
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
                x.GetFields(ConsoleAttribute.bindingFlags).ToList().ForEach(y =>
                {
                    ConsoleAttribute attribute = y.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string field = attribute.name;
                    if (field == null || !field.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        Debug.LogError(ConsoleAttribute.UnsupportedFieldNameError.ReplacedBy(field));
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(field) || propertyInfoDictionary.ContainsKey(field) || methodInfoDictionary.ContainsKey(field))
                    {
                        Debug.LogError(ConsoleAttribute.DuplicatedFieldError.ReplacedBy(field));
                        return;
                    }

                    Type type = y.FieldType;
                    if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                    {
                        Debug.LogError(ConsoleAttribute.UnsupportedFieldTypeError.ReplacedBy(field));
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
                x.GetProperties(ConsoleAttribute.bindingFlags).ToList().ForEach(y =>
                {
                    ConsoleAttribute attribute = y.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string property = attribute.name;
                    if (property == null || !property.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        Debug.LogError(ConsoleAttribute.UnsupportedPropertyNameError.ReplacedBy(property));
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(property) || propertyInfoDictionary.ContainsKey(property) || methodInfoDictionary.ContainsKey(property))
                    {
                        Debug.LogError(ConsoleAttribute.DuplicatedPropertyError.ReplacedBy(property));
                        return;
                    }

                    Type type = y.PropertyType;
                    if (!type.IsPrimitive && type != typeof(string) && !type.IsEnum)
                    {
                        Debug.LogError(ConsoleAttribute.UnsupportedPropertyTypeError.ReplacedBy(property));
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
                x.GetMethods(ConsoleAttribute.bindingFlags).Where(y => !y.IsAbstract && !y.IsGenericMethod && !y.IsDefined<ExtensionAttribute>()).ToList().ForEach(y =>
                {
                    ConsoleAttribute attribute = y.GetCustomAttribute<ConsoleAttribute>();
                    if (attribute == null)
                        return;

                    string method = attribute.name;
                    if (method == null || !method.IsMatch(RegexConstant.alphanumericOrUnderscore))
                    {
                        Debug.LogError(ConsoleAttribute.UnsupportedMethodNameError.ReplacedBy(method));
                        return;
                    }

                    ParameterInfo[] parameterInfos = y.GetParameters();
                    if (parameterInfos.Length >= 1 && parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>() || parameterInfos.ToList().Any(z =>
                    {
                        Type type = z.ParameterType;
                        return z.IsOut || type.IsByRef || !type.IsPrimitive && type != typeof(string) && !type.IsEnum;
                    }))
                    {
                        Debug.LogError(ConsoleAttribute.UnsupportedArgumentError.ReplacedBy(method));
                        return;
                    }

                    if (fieldInfoDictionary.ContainsKey(method) || propertyInfoDictionary.ContainsKey(method))
                    {
                        Debug.LogError(ConsoleAttribute.DuplicatedMethodError.ReplacedBy(method));
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
                        Debug.LogError(ConsoleAttribute.DuplicatedMethodError.ReplacedBy(method));
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

            name = null;
            arguments = null;
            targetIdList = null;
            targetNameList = null;

            try
            {
                Parse();
            }
            catch (Exception)
            {
                Debug.Log(ConsoleAttribute.UnexpectedInputError);
                ClearInputField();

                return;
            }

            if (name != null && fieldInfoDictionary.ContainsKey(name))
                FieldHandler();

            else if (name != null && propertyInfoDictionary.ContainsKey(name))
                PropertyHandler();

            else if (name != null && methodInfoDictionary.ContainsKey(name))
                MethodHandler();

            else if (name != null)
                Debug.Log(ConsoleAttribute.CommandNotFoundError.ReplacedBy(name));

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
                        Debug.Log(ConsoleAttribute.FieldTypeMismatchError.ReplacedBy(name));
                        return;
                    }

                    fieldInfo.AutoSetValue(predicate, arguments[0]);
                    return;

                default:
                    Debug.Log(ConsoleAttribute.FieldTypeMismatchError.ReplacedBy(name));
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
                        Debug.Log(ConsoleAttribute.AccessorNotDefinedError.ReplacedBy("get", name));
                        return;
                    }

                    propertyInfo.AutoGetValue(predicate);
                    return;

                case 1:
                    if (!propertyInfo.CanWrite)
                    {
                        Debug.Log(ConsoleAttribute.AccessorNotDefinedError.ReplacedBy("set", name));
                        return;
                    }

                    int compatibility;
                    object boxedArgument = arguments[0].ChangeTypeWithCompatibility(type, out compatibility);

                    if (compatibility < 0)
                    {
                        Debug.Log(ConsoleAttribute.PropertyTypeMismatchError.ReplacedBy(name));
                        return;
                    }

                    propertyInfo.AutoSetValue(predicate, arguments[0]);
                    return;

                default:
                    Debug.Log(ConsoleAttribute.PropertyTypeMismatchError.ReplacedBy(name));
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
                Debug.Log(ConsoleAttribute.ArgumentTypeMismatchError.ReplacedBy(name));
                ClearInputField();

                return;
            }

            try
            {
                mostCompatibleMethod.AutoInvoke(predicate, mostCompatibleBoxedArguments);
            }
            catch (Exception)
            {
                Debug.Log(ConsoleAttribute.MethodRuntimeError.ReplacedBy(name));
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

            input.SplitByWhiteSpace().ToList().ForEach(x =>
            {
                if (name == null)
                {
                    if (!x.IsMatch(RegexConstant.alphabet))
                        throw new NotSupportedException();

                    name = x;
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
