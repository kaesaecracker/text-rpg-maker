using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TextRpgMaker.Models.Items;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker.Models
{
    public partial class Project
    {
        private readonly Deserializer _deserializer = new DeserializerBuilder().Build();
        private readonly string _projectDir;

        // cannot be a dictionary because there could be duplicate ids
        private List<Element> TopLevelElements { get; } = new List<Element>();

        public ProjectInfo Info
            => this.TopLevelElements.OfType<ProjectInfo>().First();

        public List<StartCharacter> StartCharacters
            => this.TopLevelElements.OfType<StartCharacter>().ToList();

        public List<Armor> ArmorTypes
            => this.TopLevelElements.OfType<Armor>().ToList();

        public List<Weapon> WeaponTypes
            => this.TopLevelElements.OfType<Weapon>().ToList();

        public List<Consumable> ConsumableTypes
            => this.TopLevelElements.OfType<Consumable>().ToList();
    }
}