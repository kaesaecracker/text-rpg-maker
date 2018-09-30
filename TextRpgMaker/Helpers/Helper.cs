using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextRpgMaker.ProjectModels;
using TextRpgMaker.Workers;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Helpers
{
    public static class Helper
    {
        public static object DeserializeSafely(
            this IDeserializer d, Type t, string pathToFile, bool list)
        {
            try
            {
                using (var reader = new StreamReader(pathToFile))
                {
                    if (list) t = typeof(List<>).MakeGenericType(t);

                    return d.Deserialize(reader, t);
                }
            }
            catch (YamlException ex)
            {
                throw LoadException.DeserializationError(pathToFile, ex);
            }
        }

        /// <summary>
        ///     Enables things like AppState.LoadedProject.Characters.GetId("archer")
        /// </summary>
        /// <param name="list">the list where it looks for the id</param>
        /// <param name="id">the id to search</param>
        /// <typeparam name="T">the element type</typeparam>
        /// <returns>the element in the list with the specified id, or null if not found</returns>
        public static T GetId<T>(this IEnumerable<T> list, string id) where T : BasicElement
        {
            return list.FirstOrDefault(e => e.Id == id);
        }

        public static List<T> GetIds<T>(this IEnumerable<T> list, List<string> ids)
            where T : BasicElement
        {
            return list.Where(elem => ids.Contains(elem.Id))
                       .ToList();
        }

        public static IOController.MultiOutput And(this IOController.IOutput a, IOController.IOutput b)
        {
            return new IOController.MultiOutput(a, b);
        }

        public static string ProjectToNormalPath(string pathInProj, string pathToProj)
        {
            return pathToProj + "/" + pathInProj;
        }
    }
}