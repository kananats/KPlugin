namespace KPlugin.Debug
{
    using UnityEngine;
    using KPlugin;
    using KPlugin.Extension;

    public class Console : SingletonMonoBehaviour<Console>
    {
        [SerializeField]
        private ConsoleOutput consoleOutput;

        [SerializeField]
        private bool useDefaultLog = false;

        public static void Log(string message)
        {
            instance.consoleOutput.Log(message);
            if (instance.useDefaultLog)
                message.Log();
        }
    }
}
