using System;
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

        private void OnStartNewGameClick(object sender, EventArgs e)
        {
            Logger.Debug("Start new game");
            var looper = new InputLooper();
        }
    }
}