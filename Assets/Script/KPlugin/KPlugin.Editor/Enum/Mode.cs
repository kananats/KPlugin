namespace KPlugin.Editor
{
    using System;

    [Flags]
    public enum Mode
    {
        Edit = 1 << 0,
        Play = 1 << 1,
        All = Edit | Play,
    }
}
