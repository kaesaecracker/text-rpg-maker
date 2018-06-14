using System;
using System.Collections.Generic;
using System.IO;
using TextRpgMaker.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace TextRpgMaker
{
    public static class Extensions
    {
        public static object DeserializeSafely(
            this Deserializer d, Type t, string pathToFile, bool list)
        {
            try
            {
                using (var reader = new StreamReader(pathToFile))
                {
                    if (list)
                    {
                        t = typeof(List<>).MakeGenericType(t);
                    }

                    return d.Deserialize(reader, t);
                }
            }
            catch (YamlException ex)
            {
                throw LoadException.DeserializationError(pathToFile, ex);
            }
        }
    }
}