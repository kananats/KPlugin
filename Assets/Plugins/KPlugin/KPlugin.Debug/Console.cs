using System;
using UnityEngine;

namespace KPlugin.Debug
{
    using Internal;
    using Extension;

    public class Console : SingletonUIMonoBehaviour<Console>
    {
        [SerializeField]
        private ConsoleInput consoleInput;

        [SerializeField]
        private ConsoleOutput consoleOutput;

        [SerializeField]
        private bool useDefaultLog = false;

        [SerializeField]
        private Mode _mode = Mode.Auto;

        public Mode mode
        {
            get
            {
                return _mode;
            }
        }

        void Awake()
        {
            consoleOutput.gameObject.SetActive(true);
            consoleInput.gameObject.SetActive(true);

            consoleOutput.Initialize();
            consoleInput.Initialize();
        }

        public static void Log(object obj)
        {
            Log(obj.ToSimpleString());
        }

        [Console("log")]
        public static void Log(string message)
        {
            if (instance == null)
                return;

            if (instance.useDefaultLog)
                message.Log();

            if (instance.consoleOutput == null)
                return;

            instance.consoleOutput.Log(message);

            if (!instance.consoleInput.focused)
                instance.consoleOutput.ShowLogThenHide();
        }

        [Console("clear")]
        public static void Clear()
        {
            if (instance == null || instance.consoleOutput == null)
                return;

            instance.consoleOutput.Clear();
        }

        [Console("save")]
        public static void Save()
        {
            if (instance == null || instance.consoleOutput == null)
                return;

            instance.consoleOutput.Save();
        }

        [Console("opacity")]
        public static void SetOpacity(int opacity)
        {
            if (instance == null || instance.consoleOutput == null)
                return;

            instance.consoleOutput.SetOpacity(opacity);
        }

        [Console("mode")]
        public static void SetMode(string mode)
        {
            if (instance == null || instance.consoleOutput == null)
                return;

            if (mode.EqualsIgnoreCase("a") || mode.EqualsIgnoreCase("auto") || mode.EqualsIgnoreCase("d") || mode.EqualsIgnoreCase("default"))
                instance._mode = Mode.Auto;

            else if (mode.EqualsIgnoreCase("s") || mode.EqualsIgnoreCase("show") || mode.EqualsIgnoreCase("p") || mode.EqualsIgnoreCase("permanent"))
                instance._mode = Mode.Show;

            else if (mode.EqualsIgnoreCase("h") || mode.EqualsIgnoreCase("hide"))
                instance._mode = Mode.Hide;

            else
                throw new NotSupportedException();
        }

        [Console("size")]
        public static void SetFontSize(int size)
        {
            if (instance == null || instance.consoleOutput == null)
                return;

            instance.consoleOutput.SetFontSize(size);
        }
    }
}
