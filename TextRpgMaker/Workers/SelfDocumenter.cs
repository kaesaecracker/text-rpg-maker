using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Eto.Forms;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Workers
{
    public static class SelfDocumenter
    {
        private const string outFile = "documentation.yaml";

        public static void Document()
        {
            string absPath = Path.GetFullPath(outFile);

            if (File.Exists(outFile))
            {
                File.Delete(absPath);
                return;
            }


            using (var writer = new StreamWriter(outFile))
            {
                DocumentTypesToLoad(writer);
                DocumentAnnotatedTypes(writer);
            }

            MessageBox.Show(absPath);
        }

        private static void DocumentAnnotatedTypes(StreamWriter writer)
        {
            // todo types with [DocumentedType]
            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        where type.IsDefined(typeof(DocumentedTypeAttribute), inherit: false)
                        select type;

            DocumentTypes(types, writer);
        }

        private static void DocumentTypesToLoad(StreamWriter writer)
        {
            var types = Helper.TypesToLoad().Select(tuple => tuple.type);
            DocumentTypes(types, writer);
        }

        private static void DocumentTypes(IEnumerable<Type> types, StreamWriter writer)
        {
            foreach (var t in types)
            {
                writer.WriteLine($"---\n" +
                                 $"type: {t.Name}\n" +
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
            if (prop.PropertyType.GetInterfaces().Contains(typeof(IList)))
            {
                // prop is list type
                string ofType = prop.PropertyType.GenericTypeArguments[0].Name;
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