using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KPlugin.Extension
{
    public static class IEnumerableExtension
    {
        private static object _lock = new object();

        public static void SafeForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            lock (_lock)
            {
                List<T> cloneList = list.ToList();

                for (int i = 0; i < cloneList.Count; i++)
                    action(cloneList[i]);
            }
        }

        public static void ClearAndDestroy<T>(this List<T> list) where T : MonoBehaviour
        {
            list.SafeForEach(element => UnityEngine.Object.Destroy(element.gameObject));

            list.Clear();
        }
    }
}
