using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TextRpgMaker.Models;
using TextRpgMaker.Models.Items;
using Xamarin.Forms.Internals;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker
{
    public partial class Project
    {
        private readonly Deserializer _deserializer = new DeserializerBuilder().Build();

        private string _projectDir;

        /// <summary>
        /// Loads the project
        /// </summary>
        /// <param name="projectDir">the directory containing the project</param>
        public Project(string projectDir)
        {
            this._projectDir = projectDir;

            this.RawYamlLoad();
            this.ValidateUniqueIds();
            this.RealizeInheritance();
            this.SetDefaultValues();
        }

        /// <summary>
        /// load yaml files without doing anything special
        /// </summary>
        private void RawYamlLoad()
        {
            this.Info = this.LoadFileElement<ProjectInfo>(Const.ProjectInfoFile);
            this.StartCharacters = this.LoadFileList<StartCharacter>(Const.StartCharactersFile);

            this.ArmorTypes = this.LoadFileList<Armor>(Const.ArmorFile);
            this.WeaponTypes = this.LoadFileList<Weapon>(Const.WeaponFile);
            this.ConsumableTypes = this.LoadFileList<Consumable>(Const.ConsumableFile);
        }

        /// <summary>
        /// checks if project contains duplicate ids
        /// </summary>
        /// <exception cref="LoadFailedException">if duplicates are found</exception>
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

        /// <summary>
        /// process 'based-on' fields
        /// </summary>
        /// <exception cref="LoadFailedException">if base element is not found or types do not match</exception>
        private void RealizeInheritance()
        {
            // Set LoadStepDone=RealizeInheritance on all elements that are not based on anything
            this.TopLevelElements
                .Where(e => e.BasedOnId == null)
                .ForEach(e => e.LoadStepDone = LoadStep.RealizeInheritance);

            // for each element that is based on something
            var todo = this.TopLevelElements.Where(e => e.BasedOnId != null);
            foreach (var targetElem in todo)
            {
                Logger.Debug("Realizing inheritance for id {id}, element {@elem}", targetElem.Id, targetElem);

                // find base element, throw exception when not found
                Element baseElem;
                try
                {
                    baseElem = this.TopLevelElements.First(e => e.Id == targetElem.BasedOnId);
                }
                catch (InvalidOperationException ex)
                {
                    throw LoadFailedException.BaseElementNotFound(targetElem, ex);
                }

                // throw exception if base and target do not have the same type
                if (baseElem.GetType() != targetElem.GetType())
                {
                    throw LoadFailedException.BaseElementHasDifferentType(baseElem, targetElem);
                }


                // properties contains all properties of baseElem type that have the YamlMember attribute
                var properties = baseElem.GetType()
                                         .GetProperties()
                                         .Where(prop => prop.IsDefined(typeof(YamlMemberAttribute), true));

                // for each of those properties
                foreach (var prop in properties)
                {
                    var targetValue = prop.GetValue(targetElem);
                    var baseValue = prop.GetValue(baseElem);

                    // set the target value to the base value if target value is not set
                    if (targetValue == null)
                    {
                        prop.SetValue(targetElem, baseValue);
                    }
                }

                // after all props are processed mark element as done
                targetElem.LoadStepDone = LoadStep.RealizeInheritance;
            }

            Logger.Debug("{@elems}", this.TopLevelElements);
        }

        /// <summary>
        /// set the default values where no value is set
        /// </summary>
        private void SetDefaultValues()
        {
            throw new NotImplementedException();
        }

        // cannot be a dictionary because there could be duplicate ids
        private List<Element> TopLevelElements { get; set; } = new List<Element>();

        public ProjectInfo Info { get; private set; }

        public List<StartCharacter> StartCharacters { get; private set; }

        public List<Armor> ArmorTypes { get; private set; }

        public List<Weapon> WeaponTypes { get; private set; }

        public List<Consumable> ConsumableTypes { get; private set; }
    }
}