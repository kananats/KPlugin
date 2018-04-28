using System;

namespace KPlugin.Editor
{
    [Flags]
    public enum Mode
    {
        Edit = 1 << 0,
        Play = 1 << 1,
        All = Edit | Play,
    }
}
