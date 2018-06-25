﻿using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("characters.yaml", required: true, isList: true)]
    public class Character : Element
    {
        [YamlMember(Alias = "current-hp")]
        [YamlProperties(required: false)]
        public double CurrentHp { get; set; } = 100;

        [YamlMember(Alias = "name")]
        [YamlProperties(required: true)]
        public override string Name { get; set; }

        [YamlMember(Alias = "health")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double? Health { get; set; }

        [YamlMember(Alias = "evade")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double? Evade { get; set; }

        [YamlMember(Alias = "attack")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double? Attack { get; set; }

        [YamlMember(Alias = "speed")]
        [YamlProperties(required: false, defaultValue: 1.0)]
        public double? Speed { get; set; }

        [YamlMember(Alias = "items")]
        public Inventory Items { get; set; }

        [YamlMember(Alias = "drops")]
        public List<Drop> Drops { get; set; }

        [YamlMember(Alias = "talk-dialog")]
        public string TalkDialog { get; set; }
    }

    [DocumentedType]
    public class Drop
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(required: false)]
        public int Count { get; set; } = 1;

        [YamlMember(Alias = "chance")]
        [YamlProperties(required: false)]
        public double Chance { get; set; } = 1.0;
    }
}