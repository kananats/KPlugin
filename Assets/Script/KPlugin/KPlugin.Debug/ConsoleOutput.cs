namespace KPlugin.Debug
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Extension;

    public class ConsoleOutput : MonoBehaviour
    {
        [SerializeField]
        private Image blackPanel;

        [SerializeField]
        private RectTransform content;

        [SerializeField]
        private Text logPrefab;

        private List<Text> logTextList;

        public bool visible
        {
            get
            {
                return gameObject.activeInHierarchy;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }

        void Awake()
        {
            logTextList = new List<Text>();

            visible = false;
        }

        public void Log(string message)
        {
            Text logText = Instantiate(logPrefab);
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

        }

        public void SetOpacity(int opacity)
        {
            Color color = blackPanel.color;
            color.a = Mathf.Clamp01(opacity / 100.0f);
            blackPanel.color = color;
        }
    }
}
