namespace KPlugin.Editor
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEditor;
    using UnityEngine;
    using Extension;
    using Extension.Internal;
    using Constant.Internal;

    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class KEditor : Editor
    {
        private MonoBehaviour[] monoBehaviours;
        private MonoBehaviour monoBehaviour;

        override public void OnInspectorGUI()
        {
            monoBehaviours = targets.Cast<MonoBehaviour>().ToArray();
            monoBehaviour = monoBehaviours[0];

            HideDefaultAttributeHandler();
            monoBehaviour.GetType().GetMethods(BindingFlagsConstantInternal.bindingFlags).Where(x =>
            {
                ParameterInfo[] parameterInfos = x.GetParameters();

                return !x.IsAbstract && !x.IsGenericMethod && !x.IsDefined<ExtensionAttribute>() && (parameterInfos.Length == 0 || !parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>()) && !parameterInfos.ToList().Any(y =>
                {
                    Type type = y.ParameterType;
                    return y.IsOut || type.IsByRef || !y.IsOptional;
                });
            }).ToList().ForEach(x => SerializedMethodAttributeHandler(x));

            Repaint();
        }

        private void HideDefaultAttributeHandler()
        {
            if (monoBehaviour.GetType().GetCustomAttribute<HideDefaultAttribute>() == null)
                DrawDefaultInspector();
        }

        private void SerializedMethodAttributeHandler(MethodInfo methodInfo)
        {
            SerializeMethodAttribute attribute = methodInfo.GetCustomAttribute<SerializeMethodAttribute>();

            if (attribute == null)
                return;

            bool disableForEditMode = !EditorApplication.isPlaying && !attribute.mode.HasFlag(Mode.Edit);
            bool disableForPlayMode = EditorApplication.isPlaying && !attribute.mode.HasFlag(Mode.Play);

            if (disableForEditMode || disableForPlayMode)
                return;

            string name = "(" + methodInfo.ReturnType.Name + ") " + (attribute.name == null ? methodInfo.Name.Regular() : attribute.name);
            if (!GUILayout.Button(name))
                return;

            methodInfo.AutoInvoke(monoBehaviours, null);
        }
    }
}
