using System;
using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Models;

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

        public static LoadFailedException BaseElementNotFound(Element element) =>
            new LoadFailedException(
                $"The item id '{element.Id}' " +
                $"is based on '{element.BasedOnId}' which could not be found"
            );

        public static LoadFailedException BaseElementHasDifferentType(
            Element baseElem, Element targetElem) =>
            new LoadFailedException(
                $"The item id '{targetElem.Id}' is based on '{baseElem.Id}', " +
                $"but the types are different. (" +
                $"'{targetElem.Id}' has type '{targetElem.GetType().Name}', " +
                $"'{baseElem.Id}' is of type '{baseElem.GetType().Name}')"
            );

        public static LoadFailedException MalformedId(string elementId) =>
            new LoadFailedException(
                $"The ID '{elementId} is not well formed.\n" +
                "- IDs can only contain [a-z], '-' and [0-9]\n" +
                "- IDs cannot start with a number\n" +
                "- IDs cannot start or end with '-'"
            );

        public static Exception RequiredButNull(
            IEnumerable<(string Id, string Type, IEnumerable<(string YamlName, string CsName)>Props)
            > errors)
        {
            string message = "The following required fields are not set:\n";
            foreach (var e in errors)
            {
                message += $"-\t id: '{e.Id}'\n" +
                           $" \t type: {e.Type}\n" +
                           $" \t unset required properties:\n";

                message = e.Props.Aggregate(message, (current, prop) =>
                    current + $" \t-\t '{prop.YamlName}' ({prop.CsName})");
            }

            return new LoadFailedException(message);
        }

        private LoadFailedException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}