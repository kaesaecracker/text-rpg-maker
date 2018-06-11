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
#if DEBUG
                new ButtonMenuItem
                {
                    Text = "DEBUG",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "LoadExampleProject",
                            Command = new Command((s, e) => this.OpenProject(
                                Directory.GetCurrentDirectory() +
                                "/../ExampleProject/project-info.yaml"
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "ShowProjectInfo",
                            Command = new Command((s, e) => MessageBoxes.InfoAboutLoadedProject())
                        },
                        new ButtonMenuItem
                        {
                            Text = "LogIDs",
                            Command = new Command((s, e) => Logger.Debug(
                                "IDs: {@ids}", AppState.LoadedProject.TopLevelElements
                                                       .Select(tle => tle.Id)
                            ))
                        }
                    }
                },
#endif

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

        private static void GameInfoClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        internal static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
    }
}