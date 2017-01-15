namespace KPlugin.Extension
{
    using UnityEngine;

    public static class TransformExtension
    {
        public static void SetParent(this Transform transform, Component target)
        {
            transform.SetParent(target.transform);
        }

        public static void SetParent(this Transform transform, GameObject target)
        {
            transform.SetParent(target.transform);
        }

        public static void SetParent(this Transform transform, Component target, bool worldPositionStays)
        {
            transform.SetParent(target.transform, worldPositionStays);
        }

        public static void SetParent(this Transform transform, GameObject target, bool worldPositionStays)
        {
            transform.SetParent(target.transform, worldPositionStays);
        }
    }
}