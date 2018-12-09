using System;

namespace KPlugin.Editor
{
    /// <summary>
    /// Enum for listing all possible usable scope of <c>SerializeMethodAttribute</c>
    /// </summary>
    [Flags]
    public enum Mode
    {
        /// <summary>
        /// Only usable in edit mode
        /// </summary>
        Edit = 1 << 0,

        /// <summary>
        /// Only usable in play mode
        /// </summary>
        Play = 1 << 1,

        /// <summary>
        /// Usable in both edit and play mode
        /// </summary>
        All = Edit | Play
    }
}
