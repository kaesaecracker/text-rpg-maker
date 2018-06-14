using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Models.Items;

namespace TextRpgMaker.Models
{
    public class Project
    {
        public Project(string dir, List<Element> tles)
        {
            this.ProjectDir = dir;
            this.TopLevelElements = tles;
        }

        public string ProjectDir { get; }

        // cannot be a dictionary because there could be duplicate ids
        public List<Element> TopLevelElements { get; }

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