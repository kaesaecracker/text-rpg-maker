using System;
using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Models;
using TextRpgMaker.Models.Items;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker
{
    public partial class Project
    {
        private readonly Deserializer _deserializer = new DeserializerBuilder().Build();

        private string _projectDir;

        public Project(string projectDir)
        {
            this._projectDir = projectDir;

            this.RawYamlLoad();
            this.ValidateUniqueIds();
            this.RealizeInheritance();
            this.SetDefaultValues();
        }

        private void RawYamlLoad()
        {
            this.Info = this.LoadFileElement<ProjectInfo>(Const.ProjectInfoFile);
            this.StartCharacters = this.LoadFileList<StartCharacter>(Const.StartCharactersFile);

            this.ArmorTypes = this.LoadFileList<Armor>(Const.ArmorFile);
            this.WeaponTypes = this.LoadFileList<Weapon>(Const.WeaponFile);
            this.ConsumableTypes = this.LoadFileList<Consumable>(Const.ConsumableFile);
        }

        private void ValidateUniqueIds()
        {
            var duplicates =
                from tle in this.TopLevelElements
                group tle by tle.Id
                into grouped
                where grouped.Count() > 1
                select grouped;

            if (duplicates.Any())
            {
                throw LoadFailedException.DuplicateIds(duplicates);
            }
        }

        private void RealizeInheritance()
        {
            throw new NotImplementedException();
        }
        
        private void SetDefaultValues()
        {
            throw new NotImplementedException();
        }
        
        private List<Element> TopLevelElements { get; set; } = new List<Element>();

        public ProjectInfo Info { get; private set; }

        public List<StartCharacter> StartCharacters { get; private set; }

        public List<Armor> ArmorTypes { get; private set; }

        public List<Weapon> WeaponTypes { get; private set; }

        public List<Consumable> ConsumableTypes { get; private set; }
    }
}