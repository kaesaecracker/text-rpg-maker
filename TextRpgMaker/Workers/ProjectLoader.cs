using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Serilog.Sinks.SystemConsole.Themes;
using TextRpgMaker.Models;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public class ProjectLoader
    {
        private readonly string _folder;
        private readonly List<Element> _tles;
        private readonly Deserializer _deserializer = new DeserializerBuilder().Build();

        public ProjectLoader(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
            {
                throw LoadException.ProjectFolderMissing(pathToFolder);
            }

            this._folder = pathToFolder;
            this._tles = new List<Element>();
        }

        public Project ParseProject()
        {
            Logger.Information(
                "PARSER: Starting parsing of project in folder {fldr}",
                this._folder);

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

            return new Project(this._folder, this._tles);
        }

        /// <summary>
        /// load yaml files without doing anything special
        /// </summary>
        private void RawYamlLoad()
        {
            foreach (var tuple in Helper.TypesToLoad())
            {
                string absPath = Helper.ProjectToNormalPath(tuple.pathInProj, this._folder);
                Logger.Debug("PARSER: Parsing file {f}", absPath);

                if (!File.Exists(absPath))
                {
                    if (tuple.required)
                    {
                        throw LoadException.FileMissing(tuple.pathInProj, absPath);
                    }

                    Logger.Warning(
                        "File {f} does not exist, but is not required",
                        tuple.pathInProj);
                    continue;
                }

                var elementOrList =
                    this._deserializer.DeserializeSafely(tuple.type, absPath, tuple.isList);
                switch (elementOrList)
                {
                    case null when tuple.required:
                        throw LoadException.RequiredFileEmpty(absPath);
                    case null:
                        continue;

                    case Element elem:
                        this.AddToList(elem, absPath);
                        continue;
                    case IList list:
                        foreach (var elem in list)
                        {
                            this.AddToList((Element) elem, absPath);
                        }

                        continue;
                }
            }
        }

        /// <summary>
        /// checks for duplicate ids and whether all ids are well-formed
        /// </summary>
        /// <exception cref="LoadException">if one or more elements does not fufill these rules</exception>
        private void ValidateUniqueWellformedIds()
        {
            var duplicates = (
                from tle in this._tles
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
            var mismatches = this._tles.Where(tle => !idRegex.IsMatch(tle.Id)).ToList();
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
                from element in this._tles
                where element.BasedOnId != null
                      && this._tles.All(elem => elem.Id != element.BasedOnId)
                select element
            ).ToList();

            if (errors.Any()) throw LoadException.BaseElementNotFound(errors);

            var typeErrors = (
                from element in this._tles
                where element.BasedOnId != null
                let baseElem = this._tles.First(e => e.Id == element.BasedOnId)
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
            var realisationQueue = new Queue<Element>(this._tles);
            int stepsBeforeAbort = realisationQueue.Count;
            while (realisationQueue.Count > 0 && stepsBeforeAbort > 0)
            {
                stepsBeforeAbort--;
                bool processedElement = false;
                var targetElem = realisationQueue.Dequeue();

                if (targetElem.BasedOnId == null) processedElement = true;
                else
                {
                    var baseElem = this._tles.First(e => e.Id == targetElem.BasedOnId);

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
                from element in this._tles
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
                from element in this._tles
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

        private void AddToList(Element e, string absPath)
        {
            e.OriginalFilePath = absPath;
            this._tles.Add(e);
        }
    }
}