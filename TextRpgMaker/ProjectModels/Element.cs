using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Base type for all elements
    /// </summary>
    [DocumentedType]
    public class BasicElement
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public virtual string Id { get; set; }

        [YamlMember(Alias = "based-on")]
        public string BasedOnId { get; set; }

        [YamlMember(Alias = "name")]
        public virtual string Name { get; set; }

        [YamlIgnore]
        public string OriginalFilePath { get; set; }

        public override string ToString() => $"[Element Id={this.Id}]";
    }

    /// <summary>
    /// Base type for all elements you can look at
    /// </summary>
    [DocumentedType]
    public class LookableElement : BasicElement
    {
        [YamlMember(Alias = "look-text")]
        public string LookText { get; set; }
    }
}