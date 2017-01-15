namespace KPlugin.UI
{
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class UIMonoBehaviour : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = transform as RectTransform;

                return _rectTransform;
            }
        }
    }
}
