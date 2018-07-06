using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    [DocumentedType]
    public class Element
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

        [YamlMember(Alias = "look-text")]
        public string LookText { get; set; }

        public override string ToString() => $"[Element Id={this.Id}]";
    }
}