using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Element = TextRpgMaker.Models.Element;

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

    public class LoadFailedException : Exception
    {
        public static LoadFailedException FileMissing(string fileInProject, string triedPath) =>
            new LoadFailedException(
                $"The required project file {fileInProject} is missing!\n" +
                $" Expected it at {triedPath}"
            );

        public static LoadFailedException DuplicateIds(IEnumerable<IGrouping<string, Element>> duplicates) =>
            new LoadFailedException(
                "The project contains duplicate element ids, which is not allowed. The duplicate ids are: " +
                $"{string.Join(", ", duplicates.Select(d => d.Key))}"
            );

        public static LoadFailedException BaseElementNotFound(Element element, InvalidOperationException ex) =>
            new LoadFailedException(
                $"The item id {element.Id} is based on {element.BasedOnId} which could not be found",
                ex
            );

        public static LoadFailedException BaseElementHasDifferentType(Element baseElem, Element targetElem) =>
            new LoadFailedException(
                $"The item id '{targetElem.Id}' is based on '{baseElem.Id}', but the types are different. (" +
                $"'{targetElem.Id}' has type '{targetElem.GetType().Name}', " +
                $"'{baseElem.Id}' is of type '{baseElem.GetType().Name}')"
            );

        public LoadFailedException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}