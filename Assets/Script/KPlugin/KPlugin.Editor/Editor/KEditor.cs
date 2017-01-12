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
        private MethodInfo methodInfo;

        public override void OnInspectorGUI()
        {
            monoBehaviours = targets.Cast<MonoBehaviour>().ToArray();
            monoBehaviour = monoBehaviours[0];

            if (monoBehaviour == null)
                return;

            HideDefaultAttributeHandler();

            monoBehaviour.GetType().GetMethods(SerializeMethodAttribute.bindingFlags).ToList().ForEach(x =>
            {
                methodInfo = x;
                SerializedMethodAttributeHandler();
            });
        }

        private void HideDefaultAttributeHandler()
        {
            if (monoBehaviour.GetType().GetCustomAttributes(typeof(HideDefaultAttribute), true).Cast<HideDefaultAttribute>().ToList().Count > 0)
                return;

            DrawDefaultInspector();
        }

        //TODO Undo Handler
        private void SerializedMethodAttributeHandler()
        {
            methodInfo.GetCustomAttributes(typeof(SerializeMethodAttribute), true).Cast<SerializeMethodAttribute>().ToList().ForEach(x =>
            {
                if (!EditorApplication.isPlaying && !x.mode.HasFlag(Mode.Edit) || EditorApplication.isPlaying && !x.mode.HasFlag(Mode.Play) || methodInfo.GetParameters().Length > 0)
                    return;

                string name = "(" + methodInfo.ReturnType.Name + ") " + (x.name == null ? methodInfo.Name.ToRegular() : x.name);
                if (!GUILayout.Button(name))
                    return;

                monoBehaviours.ToList().ForEach(y =>
                {
                    object value = methodInfo.Invoke(y, null);
                    Repaint();

                    if (value == null)
                        return;

                    Debug.Log("[" + y.name + "] " + methodInfo.Name + "() returns " + value + ".");
                });
            });
        }
    }
}
