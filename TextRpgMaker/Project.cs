using System;
using System.Collections.Generic;
using TextRpgMaker.Models;
using TextRpgMaker.Models.Items;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker
{
    public partial class Project
    {
        private Deserializer _deserializer = new DeserializerBuilder().Build();

        private string _projectDir;

        public Project(string projectDir)
        {
            this._projectDir = projectDir;

            this.RawYamlLoad();
            this.ValidateUniqueIds();
            this.RealizeInheritance();
        }

        private void ValidateUniqueIds()
        {
            throw new NotImplementedException();
        }

        private void RawYamlLoad()
        {
            this.Info = this.LoadFileElement<ProjectInfo>(Const.ProjectInfoFile);
            this.StartCharacters = this.LoadFileList<StartCharacter>(Const.StartCharactersFile);

            this.ArmorTypes = this.LoadFileList<Armor>(Const.ArmorFile);
            this.WeaponTypes = this.LoadFileList<Weapon>(Const.WeaponFile);
        }



        private void RealizeInheritance()
        {
            throw new NotImplementedException();
        }

        public ProjectInfo Info { get; private set; }

        public List<StartCharacter> StartCharacters { get; private set; }

        public List<Element> TopLevelElements { get; private set; } = new List<Element>();
        
        public List<Armor> ArmorTypes { get; private set; }
        
        public List<Weapon> WeaponTypes { get; private set; }
    }
}