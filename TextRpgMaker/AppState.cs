using System;
using Eto.Forms;
using TextRpgMaker.Models;
using TextRpgMaker.Views;

namespace TextRpgMaker
{
    /// <summary>
    ///     basically global vars.
    ///     This class is supposed to hold the information that represents the current application state.
    /// </summary>
    public static class AppState
    {
        private const bool IsDebugBuild =
#if DEBUG
            true;
#else
            false;
#endif

        public static bool IsDebugRun => IsDebugBuild || Config.Debug;

        private static ProjectModel _project;

        private static GameState _game;
        public static bool IsProjectLoaded => Project != null;

        public static ProjectModel Project
        {
            get => _project;
            set
            {
                _project = value;
                ProjectChangeEvent?.Invoke(null, new ProjectChangedEventArgs(value));
            }
        }

        public static bool IsGameRunning => Game != null;

        public static GameState Game
        {
            get => _game;
            set
            {
                _game = value;
                GameChangedEvent?.Invoke(null, new GameChangedEventArgs(value));
            }
        }

        public static MainForm Ui { get; set; }

        public static Application EtoApp { get; set; }

        public static AppConfig Config { get; set; }
        public static event EventHandler<ProjectChangedEventArgs> ProjectChangeEvent;
        public static event EventHandler<GameChangedEventArgs> GameChangedEvent;

        public class ProjectChangedEventArgs : EventArgs
        {
            public ProjectChangedEventArgs(ProjectModel newProject)
            {
                this.NewProject = newProject;
            }

            public ProjectModel NewProject { get; }
        }

        public class GameChangedEventArgs
        {
            public GameChangedEventArgs(GameState newGame)
            {
                this.NewGame = newGame;
            }

            public GameState NewGame { get; set; }
        }
    }
}