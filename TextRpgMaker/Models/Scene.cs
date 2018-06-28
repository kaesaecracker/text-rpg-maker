using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("scenes.yaml", true, true)]
    public class Scene : BasicElement
    {
        [YamlMember(Alias = "characters")]
        public List<string> Characters { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemGrouping> Items { get; set; }
    }
}