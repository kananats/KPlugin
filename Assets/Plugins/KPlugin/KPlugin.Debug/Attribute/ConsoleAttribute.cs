using System;

namespace KPlugin.Debug
{
    /// <summary>
    /// A field/ property/ method attribute for enabling it in <c>Console</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class ConsoleAttribute : Attribute
    {
        /// <summary>
        /// Name to be referred in console
        /// </summary>
        public string name
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of <c>ConsoleAttribute</c>
        /// </summary>
        /// <param name="name">Name to be referred in console</param>
        public ConsoleAttribute(string name)
        {
            this.name = name;
        }
    }
}
