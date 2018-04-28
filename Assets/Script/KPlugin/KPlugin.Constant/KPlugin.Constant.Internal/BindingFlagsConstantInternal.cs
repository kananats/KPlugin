using System.Reflection;

namespace KPlugin.Constant.Internal
{
    public static class BindingFlagsConstantInternal
    {
        public static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    }
}
