using System;
using System.IO;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using TextRpgMaker.Models;
using TextRpgMaker.Views.Components;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponents();
        }
        
        private void OpenProjectClick(object sender, EventArgs e)
        {
            Logger.Debug("Open project click");

            // create and show dialog
            var dialog = new OpenFileDialog
            {
                Title = "Choose the 'project-info.yaml' and confirm",
                CheckFileExists = true,
                Filters =
                {
                    new FileFilter("YAML files", "yaml", "yml"),
                    new FileFilter("All files", "*")
                },
                CurrentFilterIndex = 0,
                // TODO remove hardcoded path for debugging, replace with last opened path
                Directory = new Uri(Directory.GetCurrentDirectory() + "/../ExampleProject/")
            };


            // if user does not click on OK when opening, do nothing
            Logger.Debug("Opening file chooser dialog");
            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                this.OpenProject(dialog.FileName);
            }
        }

        private void OpenProject(string pathToProjectInfo)
        {
            pathToProjectInfo = Path.GetFullPath(pathToProjectInfo);

            try
            {
                AppState.LoadedProject = new Project(pathToProjectInfo);
                MessageBox.Show(this, "Project loaded", caption: "Done");
            }
            catch (LoadFailedException ex)
            {
                Logger.Warning(ex, "Load failed");
                MessageBoxes.LoadFailedExceptionBox(ex);
            }
        }

        internal static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
    }
}