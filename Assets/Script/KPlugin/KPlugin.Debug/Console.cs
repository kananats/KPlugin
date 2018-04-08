namespace KPlugin.Debug
{
    using UnityEngine;
    using Extension;

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

        [Console("clear")]
        public static void Clear()
        {
            instance.consoleOutput.Clear();
        }

        [Console("save")]
        public static void Save()
        {
            instance.consoleOutput.Save();
        }

        [Console("opacity")]
        public static void SetOpacity(int opacity)
        {
            instance.consoleOutput.SetOpacity(opacity);
        }
    }
}
