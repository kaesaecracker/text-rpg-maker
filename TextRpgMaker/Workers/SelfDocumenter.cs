using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Helpers;
using TextRpgMaker.Models;
using YamlDotNet.Serialization;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public static class SelfDocumenter
    {
        public static void Document(string absPath)
        {
            Logger.Information("DOCUMENTER: Starting, writing to {f}", absPath);

            if (File.Exists(absPath))
            {
                Logger.Warning("DOCUMENTER: File already exists -> replacing");
                File.Delete(absPath);
            }

            using (var writer = new StreamWriter(absPath))
            {
                var types =
                    from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    let loadFromFileAtt =
                        type.IsDefined(typeof(LoadFromProjectFileAttribute), inherit: false)
                    where type.IsDefined(typeof(DocumentedTypeAttribute), inherit: false)
                          || loadFromFileAtt
                    orderby loadFromFileAtt descending, type.Name
                    select type;

                DocumentTypes(types, writer);
            }
        }

        private static void DocumentTypes(IEnumerable<Type> types, StreamWriter writer)
        {
            foreach (var t in types)
            {
                string extraLines = "";
                LoadFromProjectFileAttribute attribute;
                if ((attribute = t.GetCustomAttribute<LoadFromProjectFileAttribute>()) != null)
                {
                    extraLines += $"in file: {attribute.ProjectRelativePath}\n";
                }

                writer.WriteLine($"---\n" +
                                 $"type: {t.Name}\n" +
                                 extraLines +
                                 $"properties:");
                foreach (var prop in t.GetProperties())
                {
                    DocumentProperty(prop, writer);
                }
            }
        }

        private static void DocumentProperty(PropertyInfo prop, StreamWriter writer)
        {
            var yamlMemberAtt = prop.GetCustomAttribute<YamlMemberAttribute>();
            if (yamlMemberAtt == null) return;

            writer.WriteLine($"- name: {yamlMemberAtt.Alias}");
            if (typeof(IList).IsAssignableFrom(prop.PropertyType))
            {
                // prop is list type
                string ofType = prop.PropertyType == typeof(Inventory)
                    // Inventory is IList, but does not have generic type parameter because it
                    // inherits from List<ItemGrouping> directly
                    ? "Inventory (= List<ItemGrouping>)"
                    : prop.PropertyType.GenericTypeArguments[0].Name;
                writer.WriteLine($"  type: List of {ofType}");
            }
            else
            {
                writer.WriteLine($"  type: {prop.PropertyType.Name}");
            }

            var yamlPropsAtt = prop.GetCustomAttribute<YamlPropertiesAttribute>();
            if (yamlPropsAtt == null)
            {
                writer.WriteLine("  required: false");
                return;
            }

            writer.WriteLine($"  required: {yamlPropsAtt.Required}");
            if (yamlPropsAtt.DefaultValue != null)
                writer.WriteLine($"  default-val: {yamlPropsAtt.DefaultValue}");
        }
    }
}