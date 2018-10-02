using System;
using Eto.Forms;
using Gtk;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// The main application window. This file contains the methods called on click etc.
    /// </summary>
    public partial class MainForm
    {
        private void InitializeEventHandlers()
        {
            AppState.ProjectChangeEvent += this.OnProjectChange;
        }

        private void OnProjectChange(object sender, AppState.ProjectChangedEventArgs e)
        {
            this.Title = e.NewProject != null
                ? $"{e.NewProject.Info.Title} - TextRpgCreator"
                : "TextRpgCreator";
        }

        /// <summary>
        /// Choose a path and then generate the type documentation and save to that file.
        /// </summary>
        private void OnGenerateTypeDocClick(object sender, EventArgs e)
        {
            var fc = new SaveFileDialog
            {
                CheckFileExists = true,
                Title = "Choose a location for the generated file"
            };

            if (fc.ShowDialog(this) == DialogResult.Ok)
            {
                // user has chosen a file and clicked OK
                SelfDocumenter.Document(fc.FileName);
                MessageBox.Show(this, "Done writing type documentation", "Done");
            }
            else
            {
                // user clicked abort
                MessageBox.Show(this, "Aborted", "Aborted");
            }
        }

        private void OpenProjectClick(object sender, EventArgs e)
        {
            // todo if project is already loaded, confirm action (current save state is lost)
            // create and show dialog
            var dialog = new SelectFolderDialog
            {
                Title = "Choose the project folder and confirm"
            };

            // if user does not click on OK when opening, do nothing
            if (dialog.ShowDialog(this) == DialogResult.Ok) this.OpenProject(dialog.Directory);
        }

        private void OnStartNewGameClick(object sender, EventArgs e)
        {
            Logger.Debug("Start new game");

            // if game is already running, get confirmation from user
            if (AppState.IsGameRunning)
            {
                var dlg = new ConfirmationDialog
                {
                    Text = "A game is currently running. All progress will be lost!",
                    Title = "Game already running",
                    Yes = "Continue, loose progress",
                    No = "Cancel"
                };

                if (!dlg.ShowModal()) return;
            }

            GameInitializer.StartNewGame();
        }

        private void OnCreatorsHelpClick(object sender, EventArgs e)
        {
            this.OpenHelp("creators-help.yaml");
        }

        private void OnPlayersHelpClick(object sender, EventArgs e)
        {
            this.OpenHelp("players-help.yaml");
        }

        private void OnProjectHelpClick(object sender, EventArgs e)
        {
            if (!AppState.IsProjectLoaded)
            {
                MessageBox.Show(this, "Project help cannot be opened - no project is loaded");
                return;
            }

            this.OpenHelp(AppState.Project.ProjectDir + "/project-help.yaml", isAbsPath: true);
        }
    }
}