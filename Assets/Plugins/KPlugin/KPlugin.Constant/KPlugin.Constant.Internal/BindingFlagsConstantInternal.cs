using System.Reflection;

namespace KPlugin.Constant.Internal
{
    /// <summary>
    /// An internal class to declare <c>BindingFlags</c> constants
    /// </summary>
    public static class BindingFlagsConstantInternal
    {
        /// <summary>
        /// All binding flags
        /// </summary>
        public static readonly BindingFlags allBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Non-static only binding flags
        /// </summary>
        public static readonly BindingFlags instanceBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    }
}
