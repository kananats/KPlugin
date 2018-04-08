namespace KPlugin.Debug
{
    using UnityEngine;
    using UnityEditor;

    public class ConsoleEditor : MonoBehaviour
    {
        [MenuItem("GameObject/Create Other/Console (with Canvas)")]
        static void CreateConsoleWithCanvas()
        {
            Instantiate(Resources.Load("Prefab/Console (with Canvas)")).name = "Canvas";
        }

        [MenuItem("GameObject/Create Other/Console")]
        static void CreateConsole()
        {
            Instantiate(Resources.Load<Console>("Prefab/Console"));
        }
    }
}
