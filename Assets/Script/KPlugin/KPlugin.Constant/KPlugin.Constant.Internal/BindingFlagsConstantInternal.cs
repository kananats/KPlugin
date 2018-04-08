namespace KPlugin.Constant.Internal
{
    using System.Reflection;

    public static class BindingFlagsConstantInternal
    {
        public static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    }
}
