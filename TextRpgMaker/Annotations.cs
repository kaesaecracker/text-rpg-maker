using System;

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
        public LoadFromProjectFileAttribute(string projectRelativePath, bool required,
                                            bool isList = false)
        {
            this.ProjectRelativePath = projectRelativePath;
            this.Required = required;
            this.IsList = isList;
        }

        public bool IsList { get; }

        public string ProjectRelativePath { get; }

        public bool Required { get; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ValidatorMethodAttribute : Attribute
    {
    }
}