using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [LoadFromProjectFile("characters.yaml", true, true)]
    public class CharacterFileModel : ElementFileModel
    {
        [YamlMember(Alias = "health")]
        [YamlProperties(false, 1.0)]
        public double? Health { get; set; }

        [YamlMember(Alias = "evade")]
        [YamlProperties(false, 1.0)]
        public double? Evade { get; set; }

        [YamlMember(Alias = "attack")]
        [YamlProperties(false, 1.0)]
        public double? Attack { get; set; }

        [YamlMember(Alias = "speed")]
        [YamlProperties(false, 1.0)]
        public double? Speed { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemGroupingFileModel> Items { get; set; }

        [YamlMember(Alias = "drops")]
        public List<Drop> Drops { get; set; }

        [YamlMember(Alias = "talk-dialog")]
        public string TalkDialog { get; set; }
    }

    [DocumentedType]
    public class Drop
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(false)]
        public int Count { get; set; } = 1;

        [YamlMember(Alias = "chance")]
        [YamlProperties(false)]
        public double Chance { get; set; } = 1.0;
    }
}