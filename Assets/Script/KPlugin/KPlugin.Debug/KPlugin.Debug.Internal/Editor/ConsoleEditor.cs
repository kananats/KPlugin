namespace KPlugin.Debug.Internal
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.EventSystems;

    public class ConsoleEditor : MonoBehaviour
    {
        [MenuItem("GameObject/Create Other/Console (with Canvas)")]
        static void CreateConsoleWithCanvas()
        {
            Instantiate(Resources.Load("Prefab/Console (with Canvas)")).name = "Canvas";

            CreateEventSystemIfNotExist();
        }

        [MenuItem("GameObject/Create Other/Console")]
        static void CreateConsole()
        {
            Instantiate(Resources.Load<Console>("Prefab/Console"));

            CreateEventSystemIfNotExist();
        }

        private static void CreateEventSystemIfNotExist()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                Instantiate(Resources.Load("Prefab/EventSystem")).name = "EventSystem";
            }
        }
    }
}
