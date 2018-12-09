using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KPlugin.Extension
{
    /// <summary>
    /// A class for adding functionalities to <c>IEnumerable</c>
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Synchronized lock
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// Safely performing foreach loop
        /// </summary>
        public static void SafeForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            lock (_lock)
            {
                List<T> cloneList = list.ToList();

                for (int i = 0; i < cloneList.Count; i++)
                    action(cloneList[i]);
            }
        }

        /// <summary>
        /// Clear a list of <c>MonoBehaviour</c> then destroy all the objects
        /// </summary>
        public static void ClearAndDestroy<T>(this List<T> list) where T : MonoBehaviour
        {
            list.SafeForEach(element => UnityEngine.Object.Destroy(element.gameObject));

            list.Clear();
        }
    }
}
