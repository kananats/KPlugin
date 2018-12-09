namespace KPlugin.Debug
{
    /// <summary>
    /// Internal enum for listing all possible types of console command
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// Unknown command
        /// </summary>
        Unknown,

        /// <summary>
        /// GET value of field/ property
        /// </summary>
        Get,

        /// <summary>
        /// SET value of field/ property
        /// </summary>
        Set,

        /// <summary>
        /// Method
        /// </summary>
        Method,
    }
}
