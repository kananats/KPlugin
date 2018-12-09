using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace KPlugin.Debug.Internal
{
    /// <summary>
    /// The internal class for adding functionalities to Editor
    /// </summary>
    public class ConsoleEditor : MonoBehaviour
    {
        /// <summary>
        /// Creates console with canvas
        /// </summary>
        [MenuItem("GameObject/Create Other/Console (with Canvas)")]
        static void CreateConsoleWithCanvas()
        {
            Instantiate(Resources.Load("Prefab/Console (with Canvas)")).name = "Canvas";

            CreateEventSystemIfNotExist();
        }

        /// <summary>
        /// Creates console without canvas
        /// </summary>
        [MenuItem("GameObject/Create Other/Console")]
        static void CreateConsole()
        {
            Instantiate(Resources.Load<Console>("Prefab/Console"));

            CreateEventSystemIfNotExist();
        }

        /// <summary>
        /// Creates event system if it does not exist
        /// </summary>
        private static void CreateEventSystemIfNotExist()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                Instantiate(Resources.Load("Prefab/EventSystem")).name = "EventSystem";
            }
        }
    }
}
