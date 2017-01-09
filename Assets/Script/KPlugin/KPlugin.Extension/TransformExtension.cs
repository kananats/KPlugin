namespace KPlugin.Extension
{
    using UnityEngine;

    public static class TransformExtension
    {
        public static void SetParent(this Transform transform, Component target, bool worldPositionStays = false)
        {
            transform.SetParent(target.transform, worldPositionStays);
        }

        public static void SetParent(this Transform transform, GameObject target, bool worldPositionStays = false)
        {
            transform.SetParent(target.transform, worldPositionStays);
        }
    }
}