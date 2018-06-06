using System;

namespace TextRpgMaker
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(object defaultVal)
        {
            this.Value = defaultVal;
        }

        public object Value { get; }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredAttribute : Attribute
    {
    }
}