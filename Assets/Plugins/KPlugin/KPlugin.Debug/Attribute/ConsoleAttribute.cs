﻿using System;

namespace KPlugin.Debug
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class ConsoleAttribute : Attribute
    {
        public string name
        {
            get;
            private set;
        }

        public ConsoleAttribute(string name)
        {
            this.name = name;
        }
    }
}
