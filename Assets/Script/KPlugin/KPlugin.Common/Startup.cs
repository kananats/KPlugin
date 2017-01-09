namespace KPlugin.Common
{
    using UnityEngine;
    using UnityEditor;

    [InitializeOnLoad]
    public class Startup
    {
        static Startup()
        {
            SingletonHandler();
        }

        private static void SingletonHandler()
        {
            GameObject gameObject = GameObject.Find("/Singleton");
            if (gameObject == null)
                gameObject = new GameObject("Singleton");

            if (gameObject.GetComponent<SingletonManager>() != null)
                return;

            gameObject.AddComponent<SingletonManager>().Refresh();
        }
    }
}
