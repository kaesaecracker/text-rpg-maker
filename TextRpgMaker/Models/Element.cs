using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class Element
    {
        [YamlMember(Alias = "id")]
        public string Id { get; set; }
    }
}