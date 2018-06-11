using System;
using YamlDotNet.Serialization;

namespace TextRpgMaker
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class YamlPropertiesAttribute : Attribute
    {
        public YamlPropertiesAttribute(bool required, object defaultValue = null)
        {
            this.DefaultValue = defaultValue;
            this.Required = required;
        }

        public object DefaultValue { get; }
        
        public bool Required { get; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class LoadFromProjectFileAttribute : Attribute
    {
        public LoadFromProjectFileAttribute(string projectRelativePath, bool isList = false,
                                            bool required = true)
        {
            this.ProjectRelativePath = projectRelativePath;
            this.Required = required;
            this.IsList = isList;
        }

        public bool IsList { get; }

        public string ProjectRelativePath { get; }

        public bool Required { get; }
    }
}