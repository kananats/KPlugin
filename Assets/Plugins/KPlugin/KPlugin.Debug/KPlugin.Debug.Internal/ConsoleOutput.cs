using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KPlugin.Debug.Internal
{
    using Extension;

    /// <summary>
    /// Class representing outout of the console
    /// </summary>
    public class ConsoleOutput : MonoBehaviour
    {
        /// <summary>
        /// Reference to the console input
        /// </summary>
        [SerializeField]
        private ConsoleInput consoleInput;

        /// <summary>
        /// Reference to the scroll rect
        /// </summary>
        [SerializeField]
        private ScrollRect scrollRect;

        /// <summary>
        /// Reference to the image used for faded effect
        /// </summary>
        [SerializeField]
        private Image fadeImage;

        /// <summary>
        /// Reference to root of the scroll rect contents
        /// </summary>
        [SerializeField]
        private RectTransform content;

        /// <summary>
        /// List of all logs
        /// </summary>
        private List<Text> logTextList;

        /// <summary>
        /// Coroutine for automatic hiding
        /// </summary>
        private Coroutine _CR_Hide;

        /// <summary>
        /// Current font size
        /// </summary>
        private int fontSize;

        /// <summary>
        /// One-time initialization
        /// </summary>
        public void Initialize()
        {
            logTextList = new List<Text>();
            HideBlackPanel();

            _CR_Hide = null;

            fontSize = Resources.Load<Text>("Prefab/Log").fontSize;
        }

        void Update()
        {
            UpdateScrollPosition();
        }

        /// <summary>
        /// Log the given message to the output
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public void Log(string message)
        {
            Text logText = Instantiate(Resources.Load<Text>("Prefab/Log"));
            logText.fontSize = fontSize;
            logText.text = message;
            logText.transform.SetParent(content);

            logTextList.Add(logText);
        }

        /// <summary>
        /// Clear all the logs
        /// </summary>
        public void Clear()
        {
            logTextList.ClearAndDestroy();
        }

        /// <summary>
        /// Set font size
        /// </summary>
        public void SetFontSize(int fontSize)
        {
            this.fontSize = fontSize;

            logTextList.ForEach(text => text.fontSize = fontSize);
        }

        /// <summary>
        /// Writing all logs to the text file
        /// </summary>
        public void Save()
        {
            string data = "";
            logTextList.ForEach(text => data = data + text.text + "\n");
            FileManager.Write(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".log", data);
        }

        /// <summary>
        /// Set opacity
        /// </summary>
        public void SetOpacity(int opacity)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(opacity / 100.0f);
            fadeImage.color = color;
        }

        /// <summary>
        /// Coroutine for automatic hiding
        /// </summary>
        /// <returns></returns>
        private IEnumerator CR_Hide()
        {
            ShowLog();
            yield return new WaitForSeconds(3.0f);

            HideLog();
        }

        /// <summary>
        /// Enables faded effect
        /// </summary>
        public void ShowBlackPanel()
        {
            fadeImage.enabled = true;
        }

        /// <summary>
        /// Disables faded effect
        /// </summary>
        public void HideBlackPanel()
        {
            fadeImage.enabled = false;
        }

        /// <summary>
        /// Shows logs
        /// </summary>
        public void ShowLog()
        {
            if (_CR_Hide != null)
                StopCoroutine(_CR_Hide);

            content.gameObject.SetActive(true);
        }

        /// <summary>
        /// Shows logs then automatically hides after few seconds
        /// </summary>
        public void ShowLogThenHide()
        {
            if (_CR_Hide != null)
                StopCoroutine(_CR_Hide);

            _CR_Hide = StartCoroutine(CR_Hide());
        }

        /// <summary>
        /// Hides logs
        /// </summary>
        public void HideLog()
        {
            content.gameObject.SetActive(false);
        }

        /// <summary>
        /// Updates scroll position of the scroll rect
        /// </summary>
        private void UpdateScrollPosition()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y / 100.0f;
                    return;

                case RuntimePlatform.WindowsEditor:
                    scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y / 20.0f;
                    return;

                default:
                    scrollRect.verticalNormalizedPosition += Input.mouseScrollDelta.y / 100.0f;
                    return;
            }
        }
    }
}
