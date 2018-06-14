using System.IO;
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
                MessageBox.Show(AppState.Ui, outFile + " already exists: " + absPath);
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
        }

        private static void DocumentTypesToLoad(StreamWriter writer)
        {
            var types = Helper.TypesToLoad();
            foreach (var tuple in types)
            {
                writer.WriteLine($"---\n" +
                                 $"type: {tuple.type.Name}\n" +
                                 $"properties:");
                foreach (var prop in tuple.type.GetProperties())
                {
                    var yamlMemberAtt = prop.GetCustomAttribute<YamlMemberAttribute>();
                    if (yamlMemberAtt == null) continue;

                    writer.WriteLine($"- name: {yamlMemberAtt.Alias}\n" +
                                     $"  type: {prop.PropertyType.Name}");
                    var yamlPropsAtt = prop.GetCustomAttribute<YamlPropertiesAttribute>();
                    if (yamlPropsAtt == null)
                    {
                        writer.WriteLine("  required: false\n" +
                                         "  default-val: null");
                        continue;
                    }

                    writer.WriteLine($"  required: {yamlPropsAtt.Required}\n" +
                                     $"  default-val: {yamlPropsAtt.DefaultValue}");
                }
            }
        }
    }
}