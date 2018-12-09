using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;

namespace KPlugin.Editor
{
    using Extension;
    using Extension.Internal;
    using Constant.Internal;

    /// <summary>
    /// Class representing custom editor
    /// <remarks>
    /// This class is required to use <c>HideDefaultAttribute</c>, and <c>SerializeMethodAttribute</c>.
    /// </remarks>
    /// </summary>
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class KEditor : UnityEditor.Editor
    {
        /// <summary>
        /// All <c>MonoBehaviour</c> attached
        /// </summary>
        private MonoBehaviour[] monoBehaviours;

        /// <summary>
        /// First <c>MonoBehaviour</c> attached
        /// </summary>
        private MonoBehaviour monoBehaviour;

        override public void OnInspectorGUI()
        {
            monoBehaviours = targets.Cast<MonoBehaviour>().ToArray();
            monoBehaviour = monoBehaviours[0];

            HideDefaultAttributeHandler();
            monoBehaviour.GetType().GetMethods(BindingFlagsConstantInternal.allBindingFlags).Where(x =>
            {
                ParameterInfo[] parameterInfos = x.GetParameters();

                return !x.IsAbstract && !x.IsGenericMethod && !x.IsDefined<ExtensionAttribute>() && (parameterInfos.Length == 0 || !parameterInfos[parameterInfos.Length - 1].IsDefined<ParamArrayAttribute>()) && !parameterInfos.ToList().Any(y =>
                {
                    Type type = y.ParameterType;
                    return y.IsOut || type.IsByRef || !y.IsOptional;
                });
            }).ToList().ForEach(x => SerializeMethodAttributeHandler(x));

            Repaint();
        }

        /// <summary>
        /// Handler function for <c>HideDefaultAttribute</c>
        /// </summary>
        private void HideDefaultAttributeHandler()
        {
            if (monoBehaviour.GetType().GetCustomAttribute<HideDefaultAttribute>() == null)
                DrawDefaultInspector();
        }

        /// <summary>
        /// Handler function for <c>SerializMethodAttribute</c>
        /// </summary>
        /// <param name="methodInfo">The method</param>
        private void SerializeMethodAttributeHandler(MethodInfo methodInfo)
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

            methodInfo.AutoInvoke(monoBehaviours, null).Log(false);
        }
    }
}
