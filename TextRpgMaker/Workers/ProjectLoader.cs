using System.Reflection;
using TextRpgMaker.Models;
using TextRpgMaker.Views;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public static class ProjectLoader
    {
        public static void LoadProject(string path)
        {
            Logger.Information("LOADER: Starting to load {p}", path);

            try
            {
                new YamlPreprocessor(path).ProcessAll();
            }
            catch (PreprocessorException ex)
            {
                Logger.Warning(ex, "LOADER: Preprocessing failed");
                MessageBoxes.LoadFailedExceptionBox(ex);
                return;
            }

            ProjectModel p;
            try
            {
                p = new ProjectParser(path).ParseAll();
            }
            catch (LoadException ex)
            {
                Logger.Warning(ex, "LOADER: Load failed");
                MessageBoxes.LoadFailedExceptionBox(ex);
                return;
            }

            try
            {
                Validator.ValidateAll(p);
            }
            catch (TargetInvocationException ex)
            {
                if (!(ex.InnerException is ValidationFailedException)) throw;

                Logger.Warning(ex, "LOADER: Validation failed");
                MessageBoxes.LoadFailedExceptionBox((ValidationFailedException) ex.InnerException);
                return;
            }

            Logger.Information("LOADER: load finished without exceptions");
            AppState.Project = p;
        }
    }
}