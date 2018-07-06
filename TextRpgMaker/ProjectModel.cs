using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.FileModels;

namespace TextRpgMaker
{
    /// <summary>
    ///     Everything that gets loaded when you open a project.
    ///     Does <b>NOT</b> contain anything related to the current save etc
    /// </summary>
    public class ProjectModel
    {
        public ProjectModel(string dir, List<ElementFileModel> tles)
        {
            this.ProjectDir = dir;
            this.TopLevelElements = tles;
        }

        public string ProjectDir { get; }

        // cannot be a dictionary because there could be duplicate ids
        public List<ElementFileModel> TopLevelElements { get; }

        public ProjectInfoFileModel InfoFileModel
            => this.TopLevelElements.OfType<ProjectInfoFileModel>().First();

        // shortcut, cant change structure of ProjectInfo because it has to be the same structure as the yaml file
        public ProjectInfoFileModel.StartInfoContainer StartInfo
            => this.InfoFileModel.StartInfo;

        public List<CharacterFileModel> Characters
            => this.TopLevelElements.OfType<CharacterFileModel>().ToList();

        public List<ArmorFileModel> ArmorTypes
            => this.TopLevelElements.OfType<ArmorFileModel>().ToList();

        public List<WeaponFileModel> WeaponTypes
            => this.TopLevelElements.OfType<WeaponFileModel>().ToList();

        public List<ConsumableFileModel> ConsumableTypes
            => this.TopLevelElements.OfType<ConsumableFileModel>().ToList();

        public List<ItemFileModel> AmmoTypes
            => this.TopLevelElements.OfType<ItemFileModel>().ToList();

        public List<SceneFileModel> Scenes
            => this.TopLevelElements.OfType<SceneFileModel>().ToList();

        public List<DialogFileModel> Dialogs
            => this.TopLevelElements.OfType<DialogFileModel>().ToList();

        public T ById<T>(string id) where T : ElementFileModel
        {
            return this.TopLevelElements.OfType<T>().First(e => e.Id == id);
        }
    }
}