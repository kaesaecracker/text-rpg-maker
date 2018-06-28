using System;
using System.IO;
using Eto.Forms;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
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

        private void OnGenerateTypeDocClick(object sender, EventArgs e)
        {
            var fc = new SaveFileDialog
            {
                CheckFileExists = true,
                Title = "Choose a location for the generated file"
            };

            if (fc.ShowDialog(this) == DialogResult.Ok)
            {
                SelfDocumenter.Document(fc.FileName);
                MessageBox.Show(this, "Done writing type documentation", "Done");
            }
            else
            {
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
    }
}