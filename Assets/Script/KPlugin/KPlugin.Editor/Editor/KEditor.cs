namespace KPlugin.Editor
{
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Extension;

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class KEditor : Editor
    {
        private MonoBehaviour monoBehaviour;
        private MethodInfo methodInfo;

        public override void OnInspectorGUI()
        {
            HideDefaultAttributeHandler();

            monoBehaviour.GetType().GetMethods(SerializeMethodAttribute.bindingFlags).ToList().ForEach(x =>
            {
                methodInfo = x;
                SerializedMethodAttributeHandler();
            });
        }

        private void HideDefaultAttributeHandler()
        {
            monoBehaviour = target as MonoBehaviour;
            if (monoBehaviour == null || monoBehaviour.GetType().GetCustomAttributes(typeof(HideDefaultAttribute), true).Cast<HideDefaultAttribute>().ToList().Count > 0)
                return;

            DrawDefaultInspector();
        }

        private void SerializedMethodAttributeHandler()
        {
            methodInfo.GetCustomAttributes(typeof(SerializeMethodAttribute), true).Cast<SerializeMethodAttribute>().ToList().ForEach(x =>
            {
                if (!EditorApplication.isPlaying && !x.mode.HasFlag(Mode.Edit) || EditorApplication.isPlaying && !x.mode.HasFlag(Mode.Play) || methodInfo.GetParameters().Length > 0)
                    return;

                string name = "(" + methodInfo.ReturnType.Name + ") " + (x.name == null ? methodInfo.Name.ToRegular() : x.name);
                if (!GUILayout.Button(name))
                    return;

                object value = methodInfo.Invoke(monoBehaviour, null);
                if (value == null)
                    return;

                Debug.Log(methodInfo.Name + "() returns " + value + ".");
            });
        }
    }
}
