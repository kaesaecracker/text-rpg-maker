using System.Collections.Generic;
using System.Linq;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    ///     Everything that gets loaded when you open a project.
    ///     Does <b>NOT</b> contain anything related to the current save etc
    /// </summary>
    public class ProjectModel
    {
        public ProjectModel(string dir, List<Element> tles)
        {
            this.ProjectDir = dir;
            this.TopLevelElements = tles;
        }

        public string ProjectDir { get; }

        // cannot be a dictionary because there could be duplicate ids
        public List<Element> TopLevelElements { get; }

        public ProjectInfo Info
            => this.TopLevelElements.OfType<ProjectInfo>().First();

        // shortcut, cant change structure of ProjectInfo because it has to be the same structure as the yaml file
        public ProjectInfo.StartInfoContainer StartInfo
            => this.Info.StartInfo;

        public List<Character> Characters
            => this.TopLevelElements.OfType<Character>().ToList();

        public List<Armor> ArmorTypes
            => this.TopLevelElements.OfType<Armor>().ToList();

        public List<Weapon> WeaponTypes
            => this.TopLevelElements.OfType<Weapon>().ToList();

        public List<Consumable> ConsumableTypes
            => this.TopLevelElements.OfType<Consumable>().ToList();

        public List<Item> AmmoTypes
            => this.TopLevelElements.OfType<Item>().ToList();

        public List<Scene> Scenes
            => this.TopLevelElements.OfType<Scene>().ToList();

        public List<Dialog> Dialogs
            => this.TopLevelElements.OfType<Dialog>().ToList();

        public T ById<T>(string id) where T : Element
        {
            return this.TopLevelElements.OfType<T>().First(e => e.Id == id);
        }
    }
}