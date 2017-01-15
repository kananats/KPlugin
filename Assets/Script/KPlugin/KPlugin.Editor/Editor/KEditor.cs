namespace KPlugin.Editor
{
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Extension;

    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class KEditor : Editor
    {
        private MonoBehaviour[] monoBehaviours;
        private MonoBehaviour monoBehaviour;

        public override void OnInspectorGUI()
        {
            monoBehaviours = targets.Cast<MonoBehaviour>().ToArray();
            monoBehaviour = monoBehaviours[0];

            HideDefaultAttributeHandler();
            monoBehaviour.GetType().GetMethods(SerializeMethodAttribute.bindingFlags).ToList().ForEach(x => SerializedMethodAttributeHandler(x));

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

            bool disableEditMode = !EditorApplication.isPlaying && !attribute.mode.HasFlag(Mode.Edit);
            bool disablePlayMode = EditorApplication.isPlaying && !attribute.mode.HasFlag(Mode.Play);

            if (disableEditMode || disablePlayMode)
                return;

            string name = "(" + methodInfo.ReturnType.Name + ") " + (attribute.name == null ? methodInfo.Name.ToRegular() : attribute.name);
            if (!GUILayout.Button(name))
                return;

            methodInfo.AutoInvoke(monoBehaviours, null);
        }
    }
}
