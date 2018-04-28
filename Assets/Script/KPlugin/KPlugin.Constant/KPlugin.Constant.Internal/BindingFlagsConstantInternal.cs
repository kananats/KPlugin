using System.Reflection;

namespace KPlugin.Constant.Internal
{
    public static class BindingFlagsConstantInternal
    {
        public static readonly BindingFlags allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static readonly BindingFlags instanceBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    }
}
