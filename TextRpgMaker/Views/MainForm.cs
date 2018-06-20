using System;
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