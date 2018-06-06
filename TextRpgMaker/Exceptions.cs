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
                $"The required project file {fileInProject} is missing!\n" +
                $" Expected it at {triedPath}"
            );

        public static LoadFailedException DuplicateIds(IEnumerable<IGrouping<string, Element>> duplicates) =>
            new LoadFailedException(
                "The project contains duplicate element ids, which is not allowed. The duplicate ids are: " +
                $"{string.Join(", ", duplicates.Select(d => d.Key))}"
            );

        public static LoadFailedException BaseElementNotFound(Element element, InvalidOperationException ex) =>
            new LoadFailedException(
                $"The item id {element.Id} is based on {element.BasedOnId} which could not be found",
                ex
            );

        public static LoadFailedException BaseElementHasDifferentType(Element baseElem, Element targetElem) =>
            new LoadFailedException(
                $"The item id '{targetElem.Id}' is based on '{baseElem.Id}', but the types are different. (" +
                $"'{targetElem.Id}' has type '{targetElem.GetType().Name}', " +
                $"'{baseElem.Id}' is of type '{baseElem.GetType().Name}')"
            );

        public LoadFailedException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}