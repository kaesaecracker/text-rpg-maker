using System;
using System.IO;
using System.Reflection;
using Eto.Forms;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// The main application window. This file is for general stuff.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponents();
            this.InitializeEventHandlers();
            
            // Add the UI as In/Output
            AppState.IO.RegisterOutput(this);
            AppState.IO.ReplaceInput(this);
        }

        private void OpenProject(string pathToProject)
        {
            try
            {
                GameInitializer.LoadProject(pathToProject);
                MessageBox.Show(this, "Project loaded", "Done");
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
                    case TargetInvocationException tie
                        when tie.InnerException is ValidationFailedException vfe:
                        Logger.Warning(ex, "Validation failed");
                        MessageBoxes.LoadFailedExceptionBox(vfe);
                        break;

                    default: throw;
                }
            }
        }

        private void NotImplementedClick(object sender, EventArgs e)
        {
            MessageBox.Show(
                parent: this,
                text: "Not implemented yet",
                type: MessageBoxType.Warning
            );
        }


        private void OpenHelp(string pathToYaml, bool isAbsPath = false)
        {
            string absPath = isAbsPath
                ? pathToYaml
                : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                  $"/{pathToYaml}";
            if (!File.Exists(absPath))
            {
                Logger.Error("Help file {h} not found", absPath);
                MessageBox.Show(
                    parent: this,
                    caption: "Error",
                    text: $"Could not find help file '{absPath}'",
                    type: MessageBoxType.Error
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(File.ReadAllText(absPath)))
            {
                Logger.Error("Could not load help", absPath);
                MessageBox.Show(
                    parent: this,
                    caption: "Error",
                    text: $"Help file '{absPath}' is empty",
                    type: MessageBoxType.Error
                );
                return;
            }

            // todo error catching
            new HelpDialog(YamlParser.ParseHelpFile(absPath)).ShowModal(this);
        }
    }
}