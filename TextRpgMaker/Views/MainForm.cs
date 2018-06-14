﻿using System;
using System.IO;
using System.Reflection;
using Eto.Forms;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponents();
            this.InitializeEventHandlers();
        }

        private void OpenProjectClick(object sender, EventArgs e)
        {
            Logger.Debug("Open project click");

            // create and show dialog
            var dialog = new SelectFolderDialog
            {
                Title = "Choose the 'project-info.yaml' and confirm",
                // TODO remove hardcoded path for debugging, replace with last opened path
                Directory = Directory.GetCurrentDirectory() + "/../ExampleProject/"
            };


            // if user does not click on OK when opening, do nothing
            Logger.Debug("Opening file chooser dialog");
            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                this.OpenProject(dialog.Directory);
            }
        }

        private void OpenProject(string pathToProject)
        {
            try
            {
                ProjectLoader.LoadProject(pathToProject);
                MessageBox.Show(this, "Project loaded", caption: "Done");
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case LoadException _:
                    case PreprocessorException _:
                        Logger.Warning(ex, "Load failed");
                        MessageBoxes.LoadFailedExceptionBox(ex);
                        break;
                    case TargetInvocationException tie when tie.InnerException is ValidationFailedException vfe:
                        Logger.Warning(ex, "Validation failed");
                        MessageBoxes.LoadFailedExceptionBox(vfe);
                        break;
                    
                    default: throw;
                }
            }
        }

        internal static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
    }
}