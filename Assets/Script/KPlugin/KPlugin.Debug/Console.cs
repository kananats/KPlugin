namespace KPlugin.Debug
{
    using UnityEngine;
    using Extension;

    public class Console : SingletonUIMonoBehaviour<Console>
    {
        [SerializeField]
        private ConsoleOutput consoleOutput;

        [SerializeField]
        private bool useDefaultLog = false;

        public static void Log(string message)
        {
            if (instance.useDefaultLog)
                message.Log();

            if (instance.consoleOutput == null)
                return;

            instance.consoleOutput.Log(message);
        }

        [Console("clear")]
        public static void Clear()
        {
            if (instance.consoleOutput == null)
                return;

            instance.consoleOutput.Clear();
        }

        [Console("save")]
        public static void Save()
        {
            if (instance.consoleOutput == null)
                return;

            instance.consoleOutput.Save();
        }

        [Console("opacity")]
        public static void SetOpacity(int opacity)
        {
            if (instance.consoleOutput == null)
                return;

            instance.consoleOutput.SetOpacity(opacity);
        }
    }
}
