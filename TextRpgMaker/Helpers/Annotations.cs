using System;

namespace TextRpgMaker.Helpers
{
    /// <summary>
    /// Helper attribute to require a field to have value or set default one
    /// </summary>
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

    /// <summary>
    /// Helper attribute to mark a model to be loaded from a specific file
    /// </summary>
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

    /// <summary>
    /// Helper attribute to mark a type to be documented by the documentation generator
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class DocumentedTypeAttribute : Attribute
    {
    }

    /// <summary>
    /// Marks a method to be callable by text input.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class InputCommandAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        ///     Add this attribute to all command methods in InputLooper.
        ///     Such a method should have the following properties:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>private</description>
        ///         </item>
        ///         <item>
        ///             <description>instance</description>
        ///         </item>
        ///         <item>
        ///             <description>only one parameter of type string (the line entered, trimmed, excluding the command name)</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="command">The text you have to write in the text box</param>
        /// <param name="paramDescription">A short text describing the usage for the user</param>
        public InputCommandAttribute(string command, string paramDescription = "")
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
            this.ParamDescription = paramDescription;
        }

        public string ParamDescription { get; }

        public string Command { get; }
    }
}