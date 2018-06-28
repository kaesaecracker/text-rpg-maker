using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;
using TextRpgMaker.Helpers;

namespace TextRpgMaker.Workers
{
    // TYP = TextRpgCreator YAML Preprocessor
    public class YamlPreprocessor
    {
        private readonly string _folder;

        public YamlPreprocessor(string folder)
        {
            this._folder = folder;
        }

        public void ProcessAll()
        {
            Log.Logger.Information(
                "PREPROCESSOR: Starting preprocessing of .typ files in folder {f}",
                this._folder);

            var filesToCheck =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                let fileAnnotation = type.GetCustomAttribute<LoadFromProjectFileAttribute>()
                where fileAnnotation != null
                select fileAnnotation.ProjectRelativePath;

            foreach (string file in filesToCheck) this.ProcessFile(file);
        }

        private void ProcessFile(string pathInProj)
        {
            string yaml = Helper.ProjectToNormalPath(pathInProj, this._folder);
            string typ = yaml + ".typ";

            if (!File.Exists(typ) && !File.Exists(yaml))
            {
                Log.Logger.Warning("PREPROCESSOR: Not found: {yamlPath} / {typPath}", yaml, typ);
                return;
            }

            if (!File.Exists(typ)) return;
            if (File.Exists(yaml))
            {
                Log.Logger.Warning("PREPROCESSOR: Deleting yaml because typ was found: {p}", yaml);
                File.Delete(yaml);
            }

            this.ProcessTyp(typ, yaml);
        }

        /// <summary>
        ///     Assumes the YAML file does not exist. Processes the file at the supplied absolute path
        ///     to generate a YAML file with the same name
        /// </summary>
        /// <param name="fromTyp">path to TYP</param>
        /// <param name="toYaml">path to resulting yaml</param>
        private void ProcessTyp(string fromTyp, string toYaml)
        {
            Log.Logger.Debug("PREPROCESSOR: Processing .typ {typ}", fromTyp);

            using (var typReader = new StreamReader(fromTyp))
            using (var yamlWriter = new StreamWriter(toYaml))
            {
                while (true)
                {
                    string line = typReader.ReadLine();
                    if (line == null) break;

                    if (!line.Trim().StartsWith("#!"))
                    {
                        yamlWriter.WriteLine(line);
                        continue;
                    }

                    // Octothorpe is another name for the pound symbol. Yes, that is a bad code joke.
                    string withoutOctothorpe = line.Trim().Remove(0, 2).TrimStart(); // remove '#!'
                    int spaceIndex = withoutOctothorpe.IndexOf(' ');
                    if (spaceIndex == -1)
                        throw new PreprocessorException(
                            $"Preprocessor argument missing in file '{fromTyp}' in line '{line}'"
                        );

                    string command = withoutOctothorpe.Substring(0, spaceIndex).ToLower();
                    string argument = withoutOctothorpe.Substring(spaceIndex).Trim();
                    switch (command)
                    {
                        case "include":
                            this.Include(argument, yamlWriter);
                            break;

                        default:
                            throw new PreprocessorException(
                                $"Unknown TYP command: '{command}' in line '{line}' in file {fromTyp}"
                            );
                    }
                }
            }
        }

        // Todo include other TYP files
        private void Include(string pathInProj, TextWriter yamlWriter)
        {
            pathInProj = pathInProj.Trim();
            if (pathInProj.StartsWith('"'))
                pathInProj = pathInProj.Remove(0, 1);
            if (pathInProj.EndsWith('"'))
                pathInProj = pathInProj.Substring(0, pathInProj.Length - 1);
            string path = Helper.ProjectToNormalPath(pathInProj, this._folder);

            if (!File.Exists(path))
                throw new PreprocessorException($"The included file '{path}' was not found");

            yamlWriter.WriteLine($"# --- START INCLUDE {path} --- #");
            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; // skip empty lines
                    yamlWriter.WriteLine(line);
                }
            }

            yamlWriter.WriteLine($"# --- END INCLUDE {path} --- #");
        }
    }

    public class PreprocessorException : Exception
    {
        public PreprocessorException(string msg, Exception inner = null) : base(msg, inner)
        {
        }
    }
}