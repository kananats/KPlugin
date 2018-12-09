namespace KPlugin.Debug
{
    /// <summary>
    /// Appearance behavior of console output
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Shows output once received new message, then hide automatically after few seconds 
        /// </summary>
        Auto,

        /// <summary>
        /// Always shows the output
        /// </summary>
        Show,

        /// <summary>
        /// Always hides the output
        /// </summary>
        Hide
    }
}
