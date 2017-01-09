namespace KPlugin.Common
{
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Editor;
    using Extension;

    public class SingletonManager : SingletonMonoBehaviour<SingletonManager>
    {
        [SerializeMethod]
        public void Refresh()
        {
            Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsGenericSubclass(typeof(SingletonMonoBehaviour<>))).ToList().ForEach(x =>
            {
                if (GetComponentInChildren(x) != null)
                    return;

                GameObject gameObject = new GameObject(x.Name.ToRegular());
                gameObject.transform.parent = transform;
                gameObject.AddComponent(x);
            });
        }
    }
}
