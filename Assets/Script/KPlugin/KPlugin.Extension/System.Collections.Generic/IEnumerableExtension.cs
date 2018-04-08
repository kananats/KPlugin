namespace KPlugin.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class IEnumerableExtension
    {
        public static void SafeForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            List<T> cloneList = list.ToList();

            for (int i = 0; i < cloneList.Count; i++)
                action(cloneList[i]);
        }

        public static void ClearAndDestroy<T>(this List<T> list) where T : MonoBehaviour
        {
            list.SafeForEach(x => UnityEngine.Object.Destroy(x.gameObject));

            list.Clear();
        }
    }
}
