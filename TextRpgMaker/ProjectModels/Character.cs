﻿using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents a character entry in YAML files
    /// </summary>
    [LoadFromProjectFile("characters.yaml", true, true)]
    public class Character : LookableElement
    {
        [YamlMember(Alias = "current-hp")]
        [YamlProperties(false)]
        public double CurrentHp { get; set; } = 100;

        [YamlMember(Alias = "name")]
        [YamlProperties(true)]
        public override string Name { get; set; }

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
        public Inventory Items { get; set; } = new Inventory();

        [YamlMember(Alias = "drops")]
        public List<Drop> Drops { get; set; } = new List<Drop>();

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