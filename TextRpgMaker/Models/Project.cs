using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Eto.Forms;
using TextRpgMaker.Models.Items;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker.Models
{
    public partial class Project
    {
        private readonly Deserializer _deserializer = new DeserializerBuilder().Build();

        private readonly string _projectDir;

        /// <summary>
        /// Loads the project
        /// </summary>
        /// <param name="projectDir">the directory containing the project</param>
        public Project(string projectDir)
        {
            this._projectDir = projectDir;

            this.RawYamlLoad();
            this.ValidateWellformedIds();
            this.ValidateUniqueIds();
            this.RealizeInheritance();
            this.ValidateRequiredFields();
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

        private void ValidateWellformedIds()
        {
            // matches 'id', 'some-id', 'id-9-test', but not ' id ', '%KHGSI'
            var idRegex = new Regex("[a-z][a-z]+(-([a-z]|[0-9])+)*"); // good regex tool: regexr.com
            foreach (var element in this.TopLevelElements)
            {
                if (!idRegex.IsMatch(element.Id))
                {
                    throw LoadFailedException.MalformedId(element.Id);
                }
            }
        }

        /// <summary>
        /// checks if project contains duplicate ids
        /// </summary>
        /// <exception cref="LoadFailedException">if duplicates are found</exception>
        private void ValidateUniqueIds()
        {
            var duplicates = (
                from tle in this.TopLevelElements
                group tle by tle.Id
                into grouped
                where grouped.Count() > 1
                select grouped
            ).ToList();

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
            Logger.Warning(
                "RealizeInheritance cannot handle based-ons for things that get loaded later");

            // Set LoadStepDone=RealizeInheritance on all elements that are not based on anything
            foreach (var elem in this.TopLevelElements
                                     .Where(e => e.BasedOnId == null))
            {
                elem.LoadStepDone = LoadStep.RealizeInheritance;
            }

            // for each element that is based on something
            var todo = this.TopLevelElements.Where(e => e.BasedOnId != null);
            foreach (var targetElem in todo)
            {
                Logger.Debug("Realizing inheritance for id {id}", targetElem.Id);

                // find base element, throw exception when not found
                var baseElem = this.TopLevelElements
                                   .FirstOrDefault(e => e.Id == targetElem.BasedOnId);
                if (baseElem == null)
                {
                    throw LoadFailedException.BaseElementNotFound(targetElem);
                }

                // throw exception if base and target do not have the same type
                if (baseElem.GetType() != targetElem.GetType())
                {
                    throw LoadFailedException.BaseElementHasDifferentType(baseElem, targetElem);
                }

                // properties contains all properties of baseElem type that have the YamlMember attribute
                var properties = baseElem.GetType()
                                         .GetProperties()
                                         .Where(prop =>
                                             prop.IsDefined(typeof(YamlMemberAttribute), true));

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
        }

        private void ValidateRequiredFields()
        {
            var errors = (
                from element in this.TopLevelElements
                let type = element.GetType()
                let requiredPropsWithoutVal =
                    from property in type.GetProperties()
                    where property.IsDefined(typeof(RequiredAttribute), inherit: true)
                    where property.GetValue(element) == null
                    let yamlMember = property.GetCustomAttribute(
                        typeof(YamlMemberAttribute),
                        inherit: true
                    ) as YamlMemberAttribute
                    select (YamlName: yamlMember.Alias,
                        CsName: property.Name)
                where requiredPropsWithoutVal.Any()
                select (
                    Id: element.Id,
                    Type: type.Name,
                    requiredPropsWithoutVal
                )
            ).ToList();

            if (errors.Any())
            {
                throw LoadFailedException.RequiredButNull(errors);
            }
        }

        /// <summary>
        /// set the default values where no value is set
        /// </summary>
        private void SetDefaultValues()
        {
            // TODO set default values
            Logger.Warning("SetDefaultValue not implemented");
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