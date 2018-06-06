using System;
using System.IO;
using Eto.Drawing;
using Eto.Forms;
using TextRpgMaker.Views.Components;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    public class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Title = "No project loaded [TextRpgMaker]";
            this.Menu = this.InitializeMenu();

            var layout = new DynamicLayout
            {
                Padding = 3,
                DefaultSpacing = new Size(3, 3)
            };

            layout.BeginHorizontal();
            {
                layout.BeginVertical();
                {
                    layout.Add(new OutputPanel());
                    layout.Add(new InputPanel());
                }

                layout.EndBeginVertical();
                {
                    layout.Add(new InventoryPanel());
                    layout.Add(new InfoTabsPanel());
                }

                layout.EndBeginVertical();
                {
                    layout.Add(new VitalsPanel());
                    layout.Add(new CharacterPanel());
                }

                layout.EndVertical();
            }

            layout.EndHorizontal();
            this.Content = layout;
        }

        private MenuBar InitializeMenu() => new MenuBar
        {
            Items =
            {
                new ButtonMenuItem
                {
                    Text = "Game",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "Game Info",
                            Command = new Command(GameInfoClick)
                        },
                        new ButtonMenuItem
                        {
                            Text = "Open Game",
                            Command = new Command(this.OpenProjectClick)
                        }
                    }
                },

                new ButtonMenuItem
                {
                    Text = "Load / Save",
                    Command = new Command(UnimplementedClick)
                }
            },

            AboutItem = new ButtonMenuItem
            {
                Text = "About",
                Command = new Command(UnimplementedClick)
            },

            HelpItems =
            {
                new ButtonMenuItem
                {
                    Text = "Game Help",
                    Command = new Command(UnimplementedClick)
                },
                new ButtonMenuItem
                {
                    Text = "Engine Help",
                    Command = new Command(UnimplementedClick)
                }
            }
        };

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
                Directory = new Uri(Directory.GetCurrentDirectory() + "../ExampleProject/")
            };


            // if user does not click on OK when opening, do nothing
            Logger.Debug("Opening file chooser dialog");
            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                try
                {
                    this.OpenProject(dialog.FileName);
                }
                catch (LoadFailedException ex)
                {
                    Logger.Warning(ex, "Load failed");
                    MessageBoxes.LoadFailedExceptionBox(ex);
                }
            }
        }

        private void OpenProject(string pathToProjectInfo)
        {
            this.AppState = new AppState(pathToProjectInfo);
            Logger.Debug("State after load: {@s}", this.AppState);

            throw new NotImplementedException();
        }

        public AppState AppState { get; private set; }

        private static void GameInfoClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExitClick(object sender, EventArgs e) => Application.Instance.Quit();

        internal static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
    }
}