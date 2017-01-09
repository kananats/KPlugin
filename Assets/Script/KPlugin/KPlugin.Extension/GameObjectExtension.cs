namespace KPlugin.Extension
{
    using System;
    using UnityEngine;

    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T result = gameObject.GetComponent<T>();

            return result == null ? gameObject.AddComponent<T>() : result;
        }

        public static Component GetOrAddComponent(this GameObject gameObject, Type componentType)
        {
            Component result = gameObject.GetComponent(componentType);

            return result == null ? gameObject.AddComponent(componentType) : result;
        }
    }
}
 