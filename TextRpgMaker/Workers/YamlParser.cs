using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TextRpgMaker.Helpers;
using TextRpgMaker.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public class YamlParser
    {
        private static readonly Deserializer Deserializer = new DeserializerBuilder().Build();
        private readonly string _folder;
        private readonly List<BasicElement> _tles;

        public static List<string> ParseHelpFile(string path)
        {
            Logger.Debug("PARSER: ParseHelpFile {f}", path);
            return Deserializer.Deserialize<List<string>>(File.ReadAllText(path));
        }

        public YamlParser(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
                throw new LoadException(
                    $"The specified project folder {pathToFolder} does not exist");

            this._folder = pathToFolder;
            this._tles = new List<BasicElement>();
        }

        public ProjectModel ParseAll()
        {
            Logger.Information(
                "PARSER: Starting parsing of project in folder {fldr}",
                this._folder);

            // TODO for errors: print out path where id is defined
            this.RawYamlLoad();
            this.ValidateUniqueWellformedIds();
            this.ValidateBaseIdsExist();
            this.RealizeInheritance();
            this.ValidateRequiredFields();
            this.SetDefaultValues();

            return new ProjectModel(this._folder, this._tles);
        }

        /// <summary>
        ///     load yaml files without doing anything special
        /// </summary>
        private void RawYamlLoad()
        {
            var typesToLoad = (
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                let fileAnnotation = type.GetCustomAttribute<LoadFromProjectFileAttribute>()
                where fileAnnotation != null
                select (
                    Type: type,
                    PathInProj: fileAnnotation.ProjectRelativePath,
                    fileAnnotation.Required,
                    fileAnnotation.IsList
                )
            ).ToList();

            foreach (var tuple in typesToLoad)
            {
                string absPath = Helper.ProjectToNormalPath(tuple.PathInProj, this._folder);
                Logger.Debug("PARSER: Parsing file {f}", absPath);

                if (!File.Exists(absPath))
                {
                    if (tuple.Required)
                        throw new LoadException(
                            $"The required project file '{tuple.PathInProj}' is missing!\n" +
                            $"Expected it at '{absPath}'"
                        );

                    Logger.Warning(
                        "File {f} does not exist, but is not required",
                        tuple.PathInProj);
                    continue;
                }

                var elementOrList =
                    Deserializer.DeserializeSafely(tuple.Type, absPath, tuple.IsList);
                switch (elementOrList)
                {
                    case null when tuple.Required:
                        throw new LoadException($"A required file is empty: {absPath}");
                    case null:
                        continue;

                    case BasicElement elem:
                        this.AddToList(elem, absPath);
                        continue;
                    case IList list:
                        foreach (var elem in list) this.AddToList((BasicElement) elem, absPath);

                        continue;
                }
            }
        }

        /// <summary>
        ///     checks for duplicate ids and whether all ids are well-formed
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

            if (duplicates.Any())
            {
                string msg = "The project contains duplicate element ids, which is not allowed.\n";
                foreach ((string id, var elements) in duplicates)
                {
                    msg += $"- id '{id}', defined in:\n";
                    msg = elements.Aggregate(msg, (current, elem)
                        => current + $"  - '{elem.OriginalFilePath}'\n");
                }

                throw new LoadException(msg);
            }

            // matches 'id', 'some-id', 'id-9-test', but not ' id ', '%KHGSI'
            var idRegex = new Regex("[a-z][a-z]+(-([a-z]|[0-9])+)*"); // good regex tool: regexr.com
            var mismatches = this._tles.Where(tle => !idRegex.IsMatch(tle.Id)).ToList();
            if (mismatches.Any()) throw LoadException.MalformedId(mismatches);
        }

        /// <summary>
        ///     Checks for all elements with a based-on field that the referenced ids exist and have the
        ///     same type
        /// </summary>
        /// <exception cref="LoadException">
        ///     if base id does not exist or base element does not have the same type
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

            if (typeErrors.Any()) throw LoadException.BaseElementHasDifferentType(typeErrors);
        }

        /// <summary>
        ///     process 'based-on' fields
        /// </summary>
        /// <exception cref="LoadException"></exception>
        private void RealizeInheritance()
        {
            var realisationQueue = new Queue<BasicElement>(this._tles);
            int stepsBeforeAbort = realisationQueue.Count;
            while (realisationQueue.Count > 0 && stepsBeforeAbort > 0)
            {
                stepsBeforeAbort--;
                bool processedElement = false;
                var targetElem = realisationQueue.Dequeue();

                if (targetElem.BasedOnId == null)
                {
                    processedElement = true;
                }
                else
                {
                    var baseElem = this._tles.First(e => e.Id == targetElem.BasedOnId);

                    // if base element is not done yet, postpone target element
                    if (realisationQueue.Contains(baseElem))
                    {
                        realisationQueue.Enqueue(targetElem);
                    }
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
                throw LoadException.InheritanceLoopAborted(realisationQueue);
        }

        /// <summary>
        ///     Checks if all properties with the [YamlProperties] attribute and Required set to true
        ///     have a value for all of the loaded elements
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

            if (errors.Any()) throw LoadException.RequiredPropertyNull(errors);
        }

        /// <summary>
        ///     set the default values where no value is set
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

            foreach (var tuple in props) tuple.property.SetValue(tuple.element, tuple.DefaultValue);
        }

        private void AddToList(BasicElement e, string absPath)
        {
            e.OriginalFilePath = absPath;
            this._tles.Add(e);
        }
    }

    public class LoadException : Exception
    {
        public LoadException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }

        public static LoadException BaseElementNotFound(IEnumerable<BasicElement> errorElements)
        {
            string msg = errorElements.Aggregate(
                "There are elements based on other elements that could not be found:\n",
                (current, elem) => current + $"- '{elem.Id}' (from '{elem.OriginalFilePath})'\n" +
                                   $"  is based on '{elem.BasedOnId}'"
            );

            return new LoadException(msg);
        }

        public static LoadException BaseElementHasDifferentType(
            IEnumerable<(BasicElement Base, BasicElement Target)> errorTuples)
        {
            string msg = "The following elements are based on elements with different types:\n";
            foreach (var (baseElem, targetElem) in errorTuples)
                msg += $"- '{targetElem.Id}' of type '{targetElem.GetType().Name}' " +
                       $"from '{targetElem.OriginalFilePath}'\n" +
                       $"  based on '{baseElem.Id}' of type '{baseElem.GetType().Name}' " +
                       $"from '{baseElem.OriginalFilePath}'";

            return new LoadException(msg);
        }

        public static LoadException MalformedId(IEnumerable<BasicElement> elems)
        {
            string msg = "The project contains malformed IDs:\n";
            foreach (var element in elems) msg += $"- {element.Id}";

            msg += "IDs have to match the following criteria: " +
                   "- IDs can only contain [a-z], '-' and [0-9]\n" +
                   "- IDs cannot start with a number\n" +
                   "- IDs cannot start or end with '-'";

            return new LoadException(msg);
        }

        public static LoadException RequiredPropertyNull(
            IEnumerable<(BasicElement Element, string PropYamlName, string PropCsName)> errors)
        {
            string message = errors.Aggregate("The following required fields are not set:\n",
                (current, err) =>
                    current +
                    $"- '{err.Element.Id}' of type '{err.Element.GetType().Name}'\n" +
                    $"  requires property '{err.PropYamlName}' ({err.PropCsName})\n" +
                    $"  in file {err.Element.OriginalFilePath}"
            );

            return new LoadException(message);
        }

        public static LoadException InheritanceLoopAborted(Queue<BasicElement> realisationQueue)
        {
            string msg = "There was an error realizing the inheritance of elements. This might " +
                         "be a circular reference. Below you can see a list of all elements that" +
                         " were in the queue when the process was aborsysted:\n";
            while (realisationQueue.TryDequeue(out var element))
                msg += $"- id '{element.Id}'" +
                       $"  based on id '{element.BasedOnId}'" +
                       $"  defined in '{element.OriginalFilePath}'";

            return new LoadException(msg);
        }


        public static LoadException DeserializationError(string pathInProj, YamlException ex)
        {
            return new LoadException(
                $"Could not parse a file in the project: {pathInProj}",
                ex
            );
        }
    }
}