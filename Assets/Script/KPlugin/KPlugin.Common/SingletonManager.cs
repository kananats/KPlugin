namespace KPlugin.Common
{
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Editor;
    using Extension;

    [ExecuteInEditMode]
    public class SingletonManager : SingletonMonoBehaviour<SingletonManager>
    {
        void Reset()
        {
            Refresh();
        }

        [SerializeMethod(Mode.Edit)]
        public void Refresh()
        {
            Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsGenericSubclass(typeof(SingletonMonoBehaviour<>))).ToList().ForEach(x =>
            {
                if (GetComponentInChildren(x) != null)
                    return;

                GameObject gameObject = new GameObject(x.Name.Regular());
                gameObject.SetParent(this);
                gameObject.AddComponent(x);
            });
        }
    }
}
