using System;
using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Models;
using YamlDotNet.Serialization;

namespace TextRpgMaker
{
    public class LoadException : Exception
    {
        public static LoadException FileMissing(string fileInProject, string triedPath) =>
            new LoadException(
                $"The required project file '{fileInProject}' is missing!\n" +
                $"Expected it at '{triedPath}'"
            );

        public static LoadException DuplicateIds(
            IEnumerable<(string, IEnumerable<Element>)> duplicates)
        {
            string msg = "The project contains duplicate element ids, which is not allowed.\n";
            foreach ((string id, var elements) in duplicates)
            {
                msg += $"- id '{id}', defined in:\n";

                foreach (var elem in elements)
                {
                    msg += $"  - '{elem.OriginalFilePath}'\n";
                }
            }

            return new LoadException(msg);
        }

        public static LoadException BaseElementNotFound(IEnumerable<Element> errorElements)
        {
            string msg = "There are elements based on other elements that could not be found:\n";
            foreach (var elem in errorElements)
            {
                msg += $"- '{elem.Id}' (from '{elem.OriginalFilePath})'\n" +
                       $"  is based on '{elem.BasedOnId}'";
            }

            return new LoadException(msg);
        }

        public static LoadException BaseElementHasDifferentType(
            IEnumerable<(Element Base, Element Target)> errorTuples)
        {
            string msg = "The following elements are based on elements with different types:\n";
            foreach (var (baseElem, targetElem) in errorTuples)
            {
                msg += $"- '{targetElem.Id}' of type '{targetElem.GetType().Name}' " +
                       $"from '{targetElem.OriginalFilePath}'\n" +
                       $"  based on '{baseElem.Id}' of type '{baseElem.GetType().Name}' " +
                       $"from '{baseElem.OriginalFilePath}'";
            }

            return new LoadException(msg);
        }

        public static LoadException MalformedId(List<Element> elems)
        {
            string msg = "The project contains malformed IDs:\n";
            foreach (var element in elems)
            {
                msg += $"- {element.Id}";
            }

            msg += "IDs have to match the following criteria: " +
                   "- IDs can only contain [a-z], '-' and [0-9]\n" +
                   "- IDs cannot start with a number\n" +
                   "- IDs cannot start or end with '-'";

            return new LoadException(msg);
        }

        public static LoadException RequiredPropertyNull(
            IEnumerable<(Element Element, string PropYamlName, string PropCsName)> errors)
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

        public static LoadException RequiredFileEmpty(string absPath)
            => new LoadException($"A required file is empty: {absPath}");

        public static LoadException InheritanceLoopAborted(Queue<Element> realisationQueue)
        {
            string msg = "There was an error realizing the inheritance of elements. This might " +
                         "be a circular reference. Below you can see a list of all elements that" +
                         " were in the queue when the process was aborsysted:\n";
            while (realisationQueue.TryDequeue(out var element))
            {
                msg += $"- id '{element.Id}'" +
                       $"  based on id '{element.BasedOnId}'" +
                       $"  defined in '{element.OriginalFilePath}'";
            }

            return new LoadException(msg);
        }

        private LoadException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }

    public class PreprocessorException : Exception
    {
        private PreprocessorException(string msg, Exception inner = null) : base(msg, inner)
        {
        }

        public static PreprocessorException IncludedFileNotFound(string pathToInclude) =>
            new PreprocessorException($"The included file '{pathToInclude}' was not found");

        public static PreprocessorException TypCommandUnknown(
            string command, string line, string file) =>
            new PreprocessorException(
                $"Unknown TYP command: '{command}' in line '{line}' in file {file}"
            );

        public static PreprocessorException ArgumentMissing(string file, string line) =>
            new PreprocessorException(
                $"Preprocessor argument missing in file '{file}' in line '{line}'"
            );
    }
}