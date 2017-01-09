namespace KPlugin.Editor
{
    using System;

    [Flags]
    public enum Mode
    {
        Edit = 1,
        Play = 2,
        All = Edit | Play,
    }
}