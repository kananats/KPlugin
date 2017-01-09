namespace KPlugin.Common
{
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using Editor;
    using Extension;

    public class SingletonManager : MonoBehaviour
    {
        [SerializedMethod]
        public void Refresh()
        {
            Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsGenericSubclass(typeof(SingletonMonoBehaviour<>))).ToList().ForEach(x => this.GetOrAddComponent(x));
        }
    }
}
