namespace KPlugin.UI
{
    using UnityEngine;
    using Editor;

    [RequireComponent(typeof(RectTransform))]
    public class RectTransformHelper : UIMonoBehaviour
    {
        public float x, y, w, z;

        //[SerializeField, Readonly]
        //private Vector2 anchoredPosition;

        void Update()
        {
            //anchoredPosition = rectTransform.anchoredPosition;
        }
    }
}
