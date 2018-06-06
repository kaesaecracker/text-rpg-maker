using System;

namespace TextRpgMaker
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(object defaultVal, bool exceptionIfNull = false)
        {
            this.Value = defaultVal;
        }

        public object Value { get; }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    sealed class RequiredAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public MyAttribute()
        {
            // TODO: Implement code here
            throw new NotImplementedException();
        }
    }
}