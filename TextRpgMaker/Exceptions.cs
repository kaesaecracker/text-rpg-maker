using System;
using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Models;
using YamlDotNet.Serialization;

namespace TextRpgMaker
{
    public class LoadFailedException : Exception
    {
        public static LoadFailedException FileMissing(string fileInProject, string triedPath) =>
            new LoadFailedException(
                $"The required project file '{fileInProject}' is missing!\n" +
                $"Expected it at '{triedPath}'"
            );

        public static LoadFailedException DuplicateIds(
            IEnumerable<IGrouping<string, Element>> duplicates) =>
            new LoadFailedException(
                "The project contains duplicate element ids, which is not allowed.\n" +
                $"The duplicate ids are:\n " +
                $"- {string.Join("\n- ", duplicates.Select(d => $"'{d.Key}'"))}"
            );

        public static LoadFailedException BaseElementNotFound(List<Element> errorElements)
        {
            var msg = "There are elements based on other elements that could not be found:\n";
            foreach (var elem in errorElements)
            {
                msg += $"- '{elem.Id}' (from '{elem.OriginalFilePath})'\n" +
                       $"  is based on '{elem.BasedOnId}'";
            }

            return new LoadFailedException(msg);
        }

        public static LoadFailedException BaseElementHasDifferentType(
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

            return new LoadFailedException(msg);
        }

        public static LoadFailedException MalformedId(List<Element> elems)
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

            return new LoadFailedException(msg);
        }

        public static Exception RequiredPropertyNull(
            IEnumerable<(Element Element, string PropYamlName, string PropCsName)> errors)
        {
            string message = errors.Aggregate("The following required fields are not set:\n",
                (current, err) => current +
                                  $"- '{err.Element.Id}' of type '{err.Element.GetType().Name}'\n" +
                                  $"  requires property '{err.PropYamlName}' ({err.PropCsName})\n" +
                                  $"  in file {err.Element.OriginalFilePath}"
            );

            return new LoadFailedException(message);
        }

        private LoadFailedException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }

        public static LoadFailedException RequiredFileEmpty(string absPath) =>
            new LoadFailedException(
                $"A required file is empty: {absPath}"
            );
    }
}