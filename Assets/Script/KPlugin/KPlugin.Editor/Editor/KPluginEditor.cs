namespace KPlugin.Editor
{
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Extension;

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class KPluginEditor : Editor
    {
        private MonoBehaviour monoBehaviour;
        private MethodInfo methodInfo;

        public override void OnInspectorGUI()
        {
            HideDefaultAttributeHandler();

            monoBehaviour.GetType().GetMethods(SerializedMethodAttribute.bindingFlags).ToList().ForEach(x =>
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
            methodInfo.GetCustomAttributes(typeof(SerializedMethodAttribute), true).Cast<SerializedMethodAttribute>().ToList().ForEach(x =>
            {
                if (!EditorApplication.isPlaying && !x.useInEditMode || methodInfo.GetParameters().Length > 0)
                    return;

                string name = "(" + methodInfo.ReturnType.Name + ") " + (x.overriddenName == null ? methodInfo.Name.ToRegular() : x.overriddenName);
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
