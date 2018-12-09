using System;
using UnityEngine;

namespace KPlugin.Extension
{
    /// <summary>
    /// A class for adding functionalities to <c>Component</c>
    /// </summary>
    public static class ComponentExtension
    {
        /// <summary>
        /// Set parent to target <c>GameObject</c>
        /// </summary>
        public static void SetParent(this Component component, Component target)
        {
            component.transform.SetParent(target.transform);
        }

        /// <summary>
        /// Set parent to target <c>GameObject</c>
        /// </summary>
        public static void SetParent(this Component component, GameObject target)
        {
            component.SetParent(target.transform);
        }

        /// <summary>
        /// Set parent to target <c>Component</c>
        /// </summary>
        /// <param name="worldPositionStays">A <c>indicating</c> whether world position should remain unchanged</param>
        public static void SetParent(this Component component, Component target, bool worldPositionStays)
        {
            component.transform.SetParent(target.transform, worldPositionStays);
        }

        /// <summary>
        /// Set parent to target <c>GameObject</c>
        /// </summary>
        /// <param name="worldPositionStays">A <c>indicating</c> whether world position should remain unchanged</param>
        public static void SetParent(this Component component, GameObject target, bool worldPositionStays)
        {
            component.SetParent(target.transform, worldPositionStays);
        }

        /// <summary>
        /// Get component
        /// </summary>
        public static T GetComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.GetComponent<T>();
        }

        /// <summary>
        /// Get component
        /// </summary>
        public static Component GetComponent(this Component component, Type componentType)
        {
            return component.gameObject.GetComponent(componentType);
        }

        /// <summary>
        /// Add component
        /// </summary>
        public static T AddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// Add component
        /// </summary>
        public static Component AddComponent(this Component component, Type componentType)
        {
            return component.gameObject.AddComponent(componentType);
        }

        /// <summary>
        /// Get component or add if not exist
        /// </summary>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            T result = component.GetComponent<T>();
            return result == null ? component.AddComponent<T>() : result;
        }

        /// <summary>
        /// Get component or add if not exist
        /// </summary>
        public static Component GetOrAddComponent(this Component component, Type componentType)
        {
            Component result = component.GetComponent(componentType);
            return result == null ? component.AddComponent(componentType) : result;
        }
    }
}
