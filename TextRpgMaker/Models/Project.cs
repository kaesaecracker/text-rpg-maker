using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Models.Items;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker.Models
{
    public partial class Project
    {
        private readonly Deserializer _deserializer = new DeserializerBuilder().Build();

        public string ProjectDir { get; }

        // cannot be a dictionary because there could be duplicate ids
        public List<Element> TopLevelElements { get; } = new List<Element>();

        public ProjectInfo Info
            => this.TopLevelElements.OfType<ProjectInfo>().First();

        public List<Character> Characters
            => this.TopLevelElements.OfType<Character>().ToList();

        public List<Armor> ArmorTypes
            => this.TopLevelElements.OfType<Armor>().ToList();

        public List<Weapon> WeaponTypes
            => this.TopLevelElements.OfType<Weapon>().ToList();

        public List<Consumable> ConsumableTypes
            => this.TopLevelElements.OfType<Consumable>().ToList();

        public List<Ammo> AmmoTypes
            => this.TopLevelElements.OfType<Ammo>().ToList();
    }
}