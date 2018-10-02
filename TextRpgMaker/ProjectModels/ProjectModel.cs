using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    ///     Everything that gets loaded when you open a project.
    ///     Does <b>NOT</b> contain anything related to the current save etc
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        /// empty constructor for deserialization
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ProjectModel()
        {
        }
        
        public ProjectModel(string dir, List<BasicElement> tles)
        {
            this.ProjectDir = dir;
            this.TopLevelElements = tles;
        }

        public string ProjectDir { get; }

        // cannot be a dictionary because there could be duplicate ids
        [YamlMember(Alias = "top-level-elements")]
        public List<BasicElement> TopLevelElements { get; set; }

        [YamlIgnore]
        public ProjectInfo Info => this.TopLevelElements.OfType<ProjectInfo>().First();

        // shortcut, cant change structure of ProjectInfo because it has to be the same structure as the yaml file
        [YamlIgnore]
        public ProjectInfo.StartInfoContainer StartInfo => this.Info.StartInfo;

        [YamlIgnore]
        public List<Character> Characters => this.TopLevelElements.OfType<Character>().ToList();

        [YamlIgnore]
        public List<Armor> ArmorTypes => this.TopLevelElements.OfType<Armor>().ToList();

        [YamlIgnore]
        public List<Weapon> WeaponTypes => this.TopLevelElements.OfType<Weapon>().ToList();

        [YamlIgnore]
        public List<Consumable> ConsumableTypes => this.TopLevelElements.OfType<Consumable>().ToList();

        [YamlIgnore]
        public List<Item> AmmoTypes => this.TopLevelElements.OfType<Item>().ToList();

        [YamlIgnore]
        public List<Scene> Scenes => this.TopLevelElements.OfType<Scene>().ToList();

        [YamlIgnore]
        public List<Dialog> Dialogs => this.TopLevelElements.OfType<Dialog>().ToList();

        /// <summary>
        /// Get element by id
        /// </summary>
        /// <param name="id">The ID to look for</param>
        /// <typeparam name="T">The type of the element. Use BasicElement to ignore.</typeparam>
        public T ById<T>(string id) where T : BasicElement
        {
            return this.TopLevelElements.OfType<T>().First(e => e.Id == id);
        }
    }
}