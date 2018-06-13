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
            this.ProjectDir = Path.GetDirectoryName(pathToProjectInfo);

            // TODO for errors: print out path where id is defined
            // "Hard" errors - file not found, inheritance errors, etc
            this.RawYamlLoad();
            this.ValidateUniqueWellformedIds();
            this.ValidateBaseIdsExist();
            this.RealizeInheritance();
            this.ValidateRequiredFields();
            this.SetDefaultValues();

            // "soft" errors - for example there is an item in a scene that does not exist
            // TODO check if weapon ammo exists
        }

        /// <summary>
        /// load yaml files without doing anything special
        /// </summary>
        private void RawYamlLoad()
        {
            foreach (var tuple in Helper.TypesToLoad())
            {
                var absPath = tuple.pathInProj.ProjectToNormalPath(this.ProjectDir);
                if (!File.Exists(absPath))
                {
                    if (tuple.required)
                    {
                        throw LoadException.FileMissing(absPath, tuple.pathInProj);
                    }

                    Logger.Warning("Skipping non-existing file {f}", tuple.pathInProj);
                    continue;
                }

                this.Load(tuple.type, absPath, tuple.isList, tuple.required);
            }
        }

        /// <summary>
        /// checks for duplicate ids and whether all ids are well-formed
        /// </summary>
        /// <exception cref="LoadException">if one or more elements does not fufill these rules</exception>
        private void ValidateUniqueWellformedIds()
        {
            var duplicates = (
                from tle in this.TopLevelElements
                group tle by tle.Id
                into grouped
                where grouped.Count() > 1
                select (
                    grouped.Key,
                    grouped.AsEnumerable()
                )
            ).ToList();

            if (duplicates.Any()) throw LoadException.DuplicateIds(duplicates.AsEnumerable());

            // matches 'id', 'some-id', 'id-9-test', but not ' id ', '%KHGSI'
            var idRegex = new Regex("[a-z][a-z]+(-([a-z]|[0-9])+)*"); // good regex tool: regexr.com
            var mismatches = this.TopLevelElements.Where(tle => !idRegex.IsMatch(tle.Id)).ToList();
            if (mismatches.Any()) throw LoadException.MalformedId(mismatches);
        }

        /// <summary>
        /// Checks for all elements with a based-on field that the referenced ids exist and have the
        /// same type
        /// </summary>
        /// <exception cref="LoadException">
        /// if base id does not exist or base element does not have the same type
        /// </exception>
        private void ValidateBaseIdsExist()
        {
            var errors = (
                from element in this.TopLevelElements
                where element.BasedOnId != null
                      && this.TopLevelElements.All(elem => elem.Id != element.BasedOnId)
                select element
            ).ToList();

            if (errors.Any()) throw LoadException.BaseElementNotFound(errors);

            var typeErrors = (
                from element in this.TopLevelElements
                where element.BasedOnId != null
                let baseElem = this.TopLevelElements.First(e => e.Id == element.BasedOnId)
                where !baseElem.GetType().IsInstanceOfType(element)
                select (
                    Base: baseElem,
                    Target: element
                )
            ).ToList();

            if (typeErrors.Any())
            {
                throw LoadException.BaseElementHasDifferentType(typeErrors);
            }
        }

        /// <summary>
        /// process 'based-on' fields
        /// </summary>
        /// <exception cref="LoadException"></exception>
        private void RealizeInheritance()
        {
            var realisationQueue = new Queue<Element>(this.TopLevelElements);
            int stepsBeforeAbort = realisationQueue.Count;
            while (realisationQueue.Count > 0 && stepsBeforeAbort > 0)
            {
                stepsBeforeAbort--;
                bool processedElement = false;
                var targetElem = realisationQueue.Dequeue();

                if (targetElem.BasedOnId == null) processedElement = true;
                else
                {
                    var baseElem = this.TopLevelElements.First(e => e.Id == targetElem.BasedOnId);

                    // if base element is not done yet, postpone target element
                    if (realisationQueue.Contains(baseElem)) realisationQueue.Enqueue(targetElem);
                    else
                    {
                        // base element done, do the actual work
                        // properties contains all properties of baseElem type that have the YamlMember attribute
                        var properties =
                            from prop in baseElem.GetType().GetProperties()
                            where prop.IsDefined(typeof(YamlMemberAttribute), true)
                            select prop;

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

                        processedElement = true;
                    }
                }

                if (processedElement) stepsBeforeAbort = realisationQueue.Count;
            }

            if (realisationQueue.Count > 0)
            {
                throw LoadException.InheritanceLoopAborted(realisationQueue);
            }
        }

        /// <summary>
        /// Checks if all properties with the [YamlProperties] attribute and Required set to true
        /// have a value for all of the loaded elements
        /// </summary>
        /// <exception cref="LoadException">if a required property is null</exception>
        private void ValidateRequiredFields()
        {
            var errors = (
                from element in this.TopLevelElements
                from property in element.GetType().GetProperties()
                where property.GetValue(element) == null &&
                      property.IsDefined(typeof(YamlPropertiesAttribute), true)
                let yamlPropsAttr = property.GetCustomAttribute<YamlPropertiesAttribute>()
                where yamlPropsAttr.Required
                let yamlMember = property.GetCustomAttribute<YamlMemberAttribute>()
                select (
                    Element: element,
                    PropYamlName: yamlMember.Alias,
                    PropCsName: property.Name
                )
            ).ToList();

            if (errors.Any())
            {
                throw LoadException.RequiredPropertyNull(errors);
            }
        }

        /// <summary>
        /// set the default values where no value is set
        /// </summary>
        private void SetDefaultValues()
        {
            var props =
                from element in this.TopLevelElements
                from property in element.GetType().GetProperties()
                where property.GetValue(element) == null
                      && property.IsDefined(typeof(YamlPropertiesAttribute))
                let yamlPropAtt = property.GetCustomAttribute<YamlPropertiesAttribute>()
                where yamlPropAtt.DefaultValue != null
                select (element, property, yamlPropAtt.DefaultValue);

            foreach (var tuple in props)
            {
                Logger.Debug("Default value for {elem}.{p} = {val}", tuple.element.Id,
                    tuple.property.Name, tuple.DefaultValue.ToString());
                tuple.property.SetValue(tuple.element, tuple.DefaultValue);
            }
        }

        /// <summary>
        /// This is the method that loads a file into a type via the type variable.
        /// If the file contains a list it will try to deserialize a List and add all elements
        /// to TopLevelElements, otherwise it will deserialize the type t directly.
        /// </summary>
        /// <param name="t">The type that gets deserialized out of the file</param>
        /// <param name="absPath">The absolute path to the file</param>
        /// <param name="isList">Whether to load a List or not</param>
        /// <param name="throwIfEmpty">Whether to throw an error if file is empty</param>
        /// <exception cref="LoadException">If a required file cannot be loaded</exception>
        private void Load(Type t, string absPath, bool isList, bool throwIfEmpty)
        {
            // TODO catch exception if the file is not in valid yaml format or does not fit type
            Logger.Debug("Load file: {absPath} list: {list}", absPath, isList);

            void AddToList(Element e)
            {
                e.OriginalFilePath = absPath;
                this.TopLevelElements.Add(e);
            }

            using (var reader = new StreamReader(absPath))
            {
                if (isList)
                {
                    var listType = typeof(List<>).MakeGenericType(t);
                    var deserialize = this._deserializer.Deserialize(reader, listType);
                    if (deserialize is IEnumerable elemsEnumerable)
                    {
                        foreach (var e in elemsEnumerable.Cast<Element>())
                        {
                            AddToList(e);
                        }
                    }
                    else Logger.Warning("Not a list: {@d}", deserialize);
                }
                else
                {
                    var elem = (Element) this._deserializer.Deserialize(reader, t);
                    if (elem != null)
                    {
                        AddToList(elem);
                    }
                    else
                    {
                        Logger.Warning("file empty: {p}", absPath);
                        if (throwIfEmpty) throw LoadException.RequiredFileEmpty(absPath);
                    }
                }
            }
        }
    }
}