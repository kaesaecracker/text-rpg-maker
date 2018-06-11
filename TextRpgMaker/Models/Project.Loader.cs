using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using static Serilog.Log;

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

        /// <summary>
        /// Loads the project
        /// </summary>
        /// <param name="pathToProjectInfo">the directory containing the project</param>
        public Project(string pathToProjectInfo)
        {
            this._projectDir = Path.GetDirectoryName(pathToProjectInfo);

            // TODO for errors: print out defined paths
            this.RawYamlLoad();
            this.ValidateWellformedIds();
            this.ValidateUniqueIds();
            this.RealizeInheritance();
            this.ValidateRequiredFields();
            this.SetDefaultValues();

            Logger.Debug("Loaded {path}, TopLevelElements: {@tles}", pathToProjectInfo,
                this.TopLevelElements);
        }

        /// <summary>
        /// load yaml files without doing anything special
        /// </summary>
        private void RawYamlLoad()
        {
            var typesToLoad = (
                from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsDefined(typeof(LoadFromProjectFileAttribute), inherit: false)
                let fileAnnotation = type.GetCustomAttribute<LoadFromProjectFileAttribute>()
                select (
                    Type: type,
                    fileAnnotation.ProjectRelativePath,
                    fileAnnotation.Required,
                    fileAnnotation.IsList
                )
            ).ToList();

            Logger.Debug("Types to load: {@ttl}", typesToLoad);
            foreach (var tuple in typesToLoad)
            {
                var absPath = this.ProjectToNormalPath(tuple.ProjectRelativePath);
                if (!File.Exists(absPath))
                {
                    if (tuple.Required)
                    {
                        throw LoadFailedException.FileMissing(absPath, tuple.ProjectRelativePath);
                    }

                    Logger.Warning("Skipping non-existing file {f}", tuple.ProjectRelativePath);
                    continue;
                }

                this.Load(tuple.Type, absPath, tuple.IsList, tuple.Required);
            }
        }

        private void ValidateWellformedIds()
        {
            // TODO search all malformed ids then throw error with list
            // matches 'id', 'some-id', 'id-9-test', but not ' id ', '%KHGSI'
            var idRegex = new Regex("[a-z][a-z]+(-([a-z]|[0-9])+)*"); // good regex tool: regexr.com

            var mismatches = this.TopLevelElements.Where(tle => !idRegex.IsMatch(tle.Id)).ToList();
            if (mismatches.Any())
            {
                throw LoadFailedException.MalformedId(mismatches);
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
                var baseElem =
                    this.TopLevelElements.FirstOrDefault(e => e.Id == targetElem.BasedOnId);
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
                    // set the target value to the base value if target value is not set
                    var targetValue = prop.GetValue(targetElem);
                    if (targetValue == null)
                    {
                        var baseValue = prop.GetValue(baseElem);
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
                    where property.GetValue(element) == null
                          && property.IsDefined(typeof(YamlPropertiesAttribute), inherit: true)
                    let yamlPropsAttr = property.GetCustomAttribute<YamlPropertiesAttribute>()
                    where yamlPropsAttr.Required
                    let yamlMember = property.GetCustomAttribute(
                        typeof(YamlMemberAttribute),
                        inherit: true
                    ) as YamlMemberAttribute
                    select (
                        YamlName: yamlMember.Alias,
                        CsName: property.Name
                    )
                where requiredPropsWithoutVal.Any()
                select (
                    element.Id,
                    Type: type.Name,
                    requiredPropsWithoutVal
                )
            ).ToList();

            if (errors.Any())
            {
                throw LoadFailedException.RequiredPropertyNull(errors);
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

        public void Load(Type t, string absPath, bool isList, bool required)
        {
            Logger.Debug("Load path: {absPath} list: {list}", absPath, isList);

            using (var reader = new StreamReader(absPath))
            {
                if (isList)
                {
                    var listType = typeof(List<>).MakeGenericType(t);
                    var deserialize = this._deserializer.Deserialize(reader, listType);
                    if (deserialize is IEnumerable elems)
                    {
                        this.TopLevelElements.AddRange(elems.Cast<Element>());
                        return;
                    }

                    Logger.Debug("Not a list: {@d}", deserialize);
                }
                else
                {
                    var elem = (Element) this._deserializer.Deserialize(reader, t);
                    if (elem != null)
                    {
                        this.TopLevelElements.Add(elem);
                        return;
                    }
                }

                Logger.Warning("file empty: {p}", absPath);
                if (required) throw LoadFailedException.RequiredFileEmpty(absPath);
            }
        }

        private string ProjectToNormalPath(string pathInProj) =>
            this._projectDir + "/" + pathInProj;
    }
}