using System;

namespace KPlugin.Editor
{
    /// <summary>
    /// A method attribute for showing it in inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SerializeMethodAttribute : Attribute
    {
        /// <summary>
        /// Name to be referred in inspector
        /// </summary>
        public string name
        {
            get;
            private set;
        }

        /// <summary>
        /// Usable scope of this attribute
        /// </summary>
        public Mode mode
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a serialize method attribute 
        /// </summary>
        /// <param name="mode">Usable scope of this attribute</param>
        public SerializeMethodAttribute(Mode mode = Mode.All) : this(null, mode) { }

        /// <summary>
        /// Initializes a serialize method attribute 
        /// </summary>
        /// <param name="name">Name to be referred in inspector</param>
        /// <param name="mode">Usable scope of this attribute</param>
        public SerializeMethodAttribute(string name, Mode mode = Mode.All)
        {
            this.mode = mode;
            this.name = name;
        }
    }
}
