namespace KPlugin.Debug
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Extension;

    public class ConsoleOutput : MonoBehaviour
    {
        [SerializeField]
        private ConsoleInput consoleInput;

        [SerializeField]
        private Image fadeImage;

        [SerializeField]
        private RectTransform content;

        private List<Text> logTextList;

        private Coroutine _CR_Hide;

        void Start()
        {
            logTextList = new List<Text>();
            HideBlackPanel();

            _CR_Hide = null;
        }

        public void Log(string message)
        {
            Text logText = Instantiate(Resources.Load<Text>("Prefab/Log"));
            logText.text = message;
            logText.transform.SetParent(content);

            logTextList.Add(logText);
        }

        public void Clear()
        {
            logTextList.ClearAndDestroy();
        }

        public void Save()
        {
            string data = "";
            logTextList.ForEach(x => data = data + x.text + "\n");
            FileManager.Write(System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".log", data);
        }

        public void SetOpacity(int opacity)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(opacity / 100.0f);
            fadeImage.color = color;
        }

        private IEnumerator CR_Hide()
        {
            ShowLog();
            yield return new WaitForSeconds(3.0f);

            HideLog();
        }

        public void ShowBlackPanel()
        {
            fadeImage.enabled = true;
        }

        public void HideBlackPanel()
        {
            fadeImage.enabled = false;
        }

        public void ShowLog()
        {
            content.gameObject.SetActive(true);
        }

        public void ShowLogThenHide()
        {
            if (_CR_Hide != null)
                StopCoroutine(_CR_Hide);

            _CR_Hide = StartCoroutine(CR_Hide());
        }

        public void HideLog()
        {
            content.gameObject.SetActive(false);
        }
    }
}
