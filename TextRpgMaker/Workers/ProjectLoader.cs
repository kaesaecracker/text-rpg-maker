using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public static class ProjectLoader
    {
        public static void LoadProject(string path)
        {
            Logger.Information("LOADER: Starting to load {p}", path);
            
            new YamlPreprocessor(path).ProcessAll();
            var p = new ProjectParser(path).ParseAll();
            new Validator(p).ValidateAll();
            
            // if something went wrong, exceptions would have been raised -> p is valid here
            Logger.Information("LOADER: load finished without exceptions");
            AppState.LoadedProject = p;
        }
    }
}