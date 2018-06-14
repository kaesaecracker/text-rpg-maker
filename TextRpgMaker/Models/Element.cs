using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [DocumentedType]
    public class Element
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public virtual string Id { get; set; }

        [YamlMember(Alias = "based-on")]
        public string BasedOnId { get; set; }

        [YamlMember(Alias = "name")]
        public virtual string Name { get; set; }

        [YamlMember(Alias = "look-text")]
        public string LookText { get; set; }

        [YamlIgnore]
        public string OriginalFilePath { get; set; }

        public override string ToString()
        {
            return $"[Element Id={this.Id}]";
        }
    }
}