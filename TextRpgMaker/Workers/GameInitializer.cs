using System;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Helpers;
using TextRpgMaker.ProjectModels;
using TextRpgMaker.Views;
using static Serilog.Log;

namespace TextRpgMaker.Workers
{
    /// <summary>
    /// Everything needed to start a project on disk
    /// </summary>
    public static class GameInitializer
    {
        public static void StartNewGame()
        {
            AppState.Game = new GameState
            {
                CurrentScene = AppState.Project.ById<Scene>(AppState.Project.StartInfo.SceneId),
                CurrentDialog = AppState.Project.Dialogs.GetId(AppState.Project.StartInfo.DialogId)
            };

            StartFromNewGame();
        }
        
        /// <summary>
        /// Choose character etc
        /// </summary>
        private static void StartFromNewGame()
        {
            AppState.IO.Write("Choose one of the following characters:");

            var startChars = (
                from id in AppState.Project.StartInfo.CharacterIds
                select AppState.Project.Characters.GetId(id)
            ).ToList();

            foreach (var c in startChars)
            {
                OutputHelpers.PrintCharacter(c);
                AppState.IO.Write("");
            }

            AppState.IO.GetChoice(startChars, c => c.Name, choosenChar =>
            {
                AppState.Game.PlayerChar = choosenChar;
                AppState.IO.Write($">> {choosenChar.Name}\n");
                AppState.IO.Write(AppState.Project.StartInfo.IntroText
                         ?? "The project does not have an intro text");
                AppState.Game.CurrentDialog.Start();
            });
        }

        public static void StartSavedGame(string pathToSave)
        {
            throw new NotImplementedException("TODO load game");
        }

        /// <summary>
        /// Load the specified project and run validations
        /// </summary>
        public static void LoadProject(string path)
        {
            Logger.Information("LOADER: Starting to load {p}", path);

            // run the TextRpgCreator Yaml Preprocessor on .typ files to generate .typ.yaml files
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

            // parse the .yaml files
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

            // run validations on the project
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

            // set it as the current project
            Logger.Information("LOADER: load finished without exceptions");
            AppState.Project = p;
        }
    }
}