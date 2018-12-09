using System;
using UnityEngine;

namespace KPlugin.Extension
{
    /// <summary>
    /// A class for adding functionalities to <c>GameObject</c>
    /// </summary>
    public static class GameObjectExtension
    {
        /// <summary>
        /// Set parent to target <c>Component</c>
        /// </summary>
        public static void SetParent(this GameObject gameObject, Component target)
        {
            gameObject.transform.SetParent(target.transform);
        }

        /// <summary>
        /// Set parent to target <c>GameObject</c>
        /// </summary>
        public static void SetParent(this GameObject gameObject, GameObject target)
        {
            gameObject.transform.SetParent(target.transform);
        }

        /// <summary>
        /// Set parent to target <c>Component</c>
        /// </summary>
        /// <param name="worldPositionStays">A <c>indicating</c> whether world position should remain unchanged</param>
        public static void SetParent(this GameObject gameObject, Component target, bool worldPositionStays = false)
        {
            gameObject.transform.SetParent(target.transform, worldPositionStays);
        }

        /// <summary>
        /// Set parent to target <c>GameObject</c>
        /// </summary>
        /// <param name="worldPositionStays">A <c>indicating</c> whether world position should remain unchanged</param>
        public static void SetParent(this GameObject gameObject, GameObject target, bool worldPositionStays = false)
        {
            gameObject.transform.SetParent(target.transform, worldPositionStays);
        }

        /// <summary>
        /// Get component or add if not exist
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T result = gameObject.GetComponent<T>();

            return result == null ? gameObject.AddComponent<T>() : result;
        }

        /// <summary>
        /// Get component or add if not exist
        /// </summary>
        public static Component GetOrAddComponent(this GameObject gameObject, Type componentType)
        {
            Component result = gameObject.GetComponent(componentType);

            return result == null ? gameObject.AddComponent(componentType) : result;
        }
    }
}
