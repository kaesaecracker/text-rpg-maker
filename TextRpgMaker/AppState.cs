using System;
using Eto.Forms;
using TextRpgMaker.Models;
using TextRpgMaker.Views;

namespace TextRpgMaker
{
    /// <summary>
    /// basically global vars.
    /// This class is supposed to hold the information that represents the current application state.
    /// </summary>
    public static class AppState
    {
        public static event EventHandler<ProjectChangedEventArgs> ProjectChangeEvent;

        private static Project _loadedProject;
        public static bool IsProjectLoaded => LoadedProject != null;
        public static Project LoadedProject
        {
            get => _loadedProject;
            set
            {
                _loadedProject = value;
                ProjectChangeEvent?.Invoke(null, new ProjectChangedEventArgs(value));
            }
        }

        public static GameState GameState { get; set; }
        
        public static MainForm Ui { get; set; }

        public static Application EtoApp { get; set; }
        
        public static AppConfig Config { get; set; }

        public class ProjectChangedEventArgs : EventArgs
        {
            public ProjectChangedEventArgs(Project newProject)
            {
                this.NewProject = newProject;
            }

            public Project NewProject { get; }
        }
    }
}