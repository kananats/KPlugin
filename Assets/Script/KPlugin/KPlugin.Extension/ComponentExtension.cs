namespace KPlugin.Extension
{
    using System;
    using UnityEngine;

    public static class ComponentExtension
    {
        public static void SetParent(this Component component, Component target)
        {
            component.transform.SetParent(target.transform);
        }

        public static void SetParent(this Component component, GameObject target)
        {
            component.SetParent(target.transform);
        }

        public static void SetParent(this Component component, Component target, bool worldPositionStays)
        {
            component.transform.SetParent(target.transform, worldPositionStays);
        }

        public static void SetParent(this Component component, GameObject target, bool worldPositionStays)
        {
            component.SetParent(target.transform, worldPositionStays);
        }

        public static T GetComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.GetComponent<T>();
        }

        public static Component GetComponent(this Component component, Type componentType)
        {
            return component.gameObject.GetComponent(componentType);
        }

        public static T AddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.AddComponent<T>();
        }

        public static Component AddComponent(this Component component, Type componentType)
        {
            return component.gameObject.AddComponent(componentType);
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            T result = component.GetComponent<T>();
            return result == null ? component.AddComponent<T>() : result;
        }

        public static Component GetOrAddComponent(this Component component, Type componentType)
        {
            Component result = component.GetComponent(componentType);
            return result == null ? component.AddComponent(componentType) : result;
        }
    }
}
