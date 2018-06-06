using System.Collections.Generic;
using System.IO;

namespace TextRpgMaker.Models
{
    public partial class Project
    {
        /// <summary>
        /// empty constructor used for deserialization
        /// </summary>
        public Project()
        {
        }

        private T LoadFileElement<T>(string fileInProject) where T : Element
        {
            T elem;
            var file = this.ProjectToNormalPath(fileInProject);

            if (!File.Exists(file))
            {
                throw LoadFailedException.FileMissing(fileInProject, file);    
            }
            
            using (var reader = new StreamReader(file))
            {
                elem = this._deserializer.Deserialize<T>(reader);
            }

            this.TopLevelElements.Add(elem);
            return elem;
        }

        private List<T> LoadFileList<T>(string fileInProject) where T : Element
        {
            List<T> elems;
            var file = this.ProjectToNormalPath(fileInProject);

            if (!File.Exists(file))
            {
                throw LoadFailedException.FileMissing(fileInProject, file);
            }

            using (var reader = new StreamReader(file))
            {
                elems = this._deserializer.Deserialize<List<T>>(reader);
            }

            if (elems != null) // empty file
            {
                foreach (var e in elems)
                {
                    this.TopLevelElements.Add(e);
                }
            }

            return elems;
        }

        private string ProjectToNormalPath(string pathInProj) =>
            this._projectDir + "/" + pathInProj;


        private class Const
        {
            public const string ProjectInfoFile = "project-info.yaml";
            public const string StartCharactersFile = "start-characters.yaml";

            private const string ItemsFolder = "items/";
            public const string ArmorFile = ItemsFolder + "armor.yaml";
            public const string WeaponFile = ItemsFolder + "weapons.yaml";
            public const string ConsumableFile = ItemsFolder + "consumables.yaml";

            private const string NpcFolder = "npcs";
            private const string ResourcesFolder = "resources";
            private const string SavesFolder = "saves";
            private const string ScenesFolder = "scenes";
        }
    }
}