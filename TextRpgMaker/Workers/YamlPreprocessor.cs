using System.IO;
using Serilog;

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

            var filesToCheck = Helper.TypesToLoad();
            foreach (var tuple in filesToCheck) this.ProcessFile(tuple.pathInProj);
        }

        private void ProcessFile(string pathInProj)
        {
            string absPathToYaml = Helper.ProjectToNormalPath(pathInProj, this._folder);
            string absPathToTyp = Path.ChangeExtension(absPathToYaml, "typ");

            if (!File.Exists(absPathToTyp) && !File.Exists(absPathToYaml))
            {
                Log.Logger.Warning(
                    "PREPROCESSOR: Neither yaml nor typ found: {yamlPath} / {typPath}",
                    absPathToYaml, absPathToTyp);
                return;
            }

            if (!File.Exists(absPathToTyp)) return;

            if (File.Exists(absPathToYaml))
            {
                Log.Logger.Warning("PREPROCESSOR: Deleting YAML file {yaml}", absPathToYaml);
                File.Delete(absPathToYaml);
            }

            this.ProcessTyp(absPathToTyp, absPathToYaml);
        }

        /// <summary>
        /// Assumes the YAML file does not exist. Processes the file at the supplied absolute path
        /// to generate a YAML file with the same name
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

                    // todo temp is not a good var name
                    string temp = line.Trim().Remove(0, 2).TrimStart(); // remove '#!'
                    int spaceIndex = temp.IndexOf(' ');
                    if (spaceIndex == -1)
                    {
                        throw PreprocessorException.ArgumentMissing(fromTyp, line);
                    }

                    string command = temp.Substring(0, spaceIndex).ToLower();
                    string argument = temp.Substring(spaceIndex).Trim();
                    switch (command)
                    {
                        case "include":
                            this.Include(argument, yamlWriter);
                            break;

                        default:
                            throw PreprocessorException.TypCommandUnknown(command, line,
                                fromTyp);
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
            {
                throw PreprocessorException.IncludedFileNotFound(path);
            }

            yamlWriter.WriteLine($"# --- START INCLUDE {path} --- #");
            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yamlWriter.WriteLine(line);
                }
            }

            yamlWriter.WriteLine($"# --- END INCLUDE {path} --- #");
        }
    }
}