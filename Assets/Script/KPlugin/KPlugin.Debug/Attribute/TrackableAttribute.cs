namespace KPlugin.Debug
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TrackableAttribute : Attribute
    {
        public string name
        {
            get;
            private set;
        }

        public float frameCount
        {
            get;
            private set;
        }

        public TrackableAttribute(string name, int frameCount = 1)
        {
            this.name = name;
            this.frameCount = frameCount;
        }
    }
}
