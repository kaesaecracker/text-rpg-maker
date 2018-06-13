using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("characters.yaml", required: true, isList: true)]
    public class Character
    {
        [YamlMember(Alias = "current-hp")]
        [YamlProperties(required: false, defaultValue: 100)]
        public double CurrentHp { get; set; }

        [YamlMember(Alias = "name")]
        [YamlProperties(required: true)]
        public string Name { get; set; }

        [YamlMember(Alias = "health")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Health { get; set; }

        [YamlMember(Alias = "evade")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Evade { get; set; }

        [YamlMember(Alias = "attack")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Attack { get; set; }

        [YamlMember(Alias = "speed")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Speed { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemIdWithCount> StartItems { get; set; } = new List<ItemIdWithCount>();

        [YamlMember(Alias = "drops")]
        public List<Drop> Drops { get; set; }
    }

    public class Drop
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(required: false, defaultValue: 1)]
        public int Count { get; set; }

        [YamlMember(Alias = "chance")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double Chance { get; set; }
    }
}