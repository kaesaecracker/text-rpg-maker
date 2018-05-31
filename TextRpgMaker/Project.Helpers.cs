using System;
using System.Collections.Generic;
using System.IO;
using TextRpgMaker.Models;
using static Serilog.Log;

namespace TextRpgMaker
{
    public partial class Project
    {
        public Project()
        {
            // empty constructor for deserialisation
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
                $" Expected it at {triedPath}",
                null);


        public LoadFailedException(string message,
                                   Exception innerException) : base(message, innerException)
        {
        }
    }
}