namespace KPlugin.UI
{
    using UnityEditor;
    using Editor;

    [CustomEditor(typeof(RectTransformHelper), true), CanEditMultipleObjects]
    public class RectTransformHelperEditor : KEditor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //target.
        }
    }
}
