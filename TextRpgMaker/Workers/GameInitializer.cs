using System;
using System.Reflection;
using TextRpgMaker.Helpers;
using TextRpgMaker.Models;
using TextRpgMaker.Views;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    public static class GameInitializer
    {
        public static void StartNewGame()
        {
            AppState.Game = new GameState
            {
                CurrentScene = AppState.Project.ById<Scene>(AppState.Project.StartInfo.SceneId),
                CurrentDialog = AppState.Project.Dialogs.GetId(AppState.Project.StartInfo.DialogId)
            };

            var looper = new InputLooper();
            looper.StartFromNewGame();
        }

        public static void StartSavedGame(string pathToSave)
        {
            throw new NotImplementedException("TODO load game");
        }

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
                p = new YamlParser(path).ParseAll();
            }
            catch (LoadException ex)
            {
                Logger.Warning(ex, "LOADER: Load failed");
                MessageBoxes.LoadFailedExceptionBox(ex);
                return;
            }

            try
            {
                ProjectValidator.RunAllValidations(p);
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