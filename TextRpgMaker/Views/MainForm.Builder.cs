using System.IO;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// The main application window. This file is for building the UI.
    /// </summary>
    public partial class MainForm
    {
        private InputPanel _inputPanel;
        private OutputPanel _outputPanel;

        private void InitializeComponents()
        {
            this.Title = "TextRpgCreator";
            this.Menu = this.InitializeMenu();
            this.Icon = new Icon("icon.ico");

            var layout = new DynamicLayout
            {
                Padding = 3,
                DefaultSpacing = new Size(3, 3)
            };

            this._outputPanel = new OutputPanel();
            this._inputPanel = new InputPanel();
            layout.BeginHorizontal();
            {
                layout.BeginVertical();
                {
                    layout.Add(this._outputPanel);
                    layout.Add(this._inputPanel);
                }

                layout.EndVertical();
            }

            layout.EndHorizontal();
            this.Content = layout;
        }

        /// <summary>
        /// The menu bar in the main application window.
        /// </summary>
        private MenuBar InitializeMenu() => new MenuBar
        {
            Items =
            {
// TODO find a way to enable debug menu according to AppState.IsDebugRun without breaking this structure
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
                                Path.GetFullPath("../ExampleProject")
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "StartExampleProject",
                            Command = new Command((sender, args) =>
                            {
                                this.OpenProject(Path.GetFullPath("../ExampleProject"));
                                this.OnStartNewGameClick(sender, args);
                            })
                        },
                        new ButtonMenuItem
                        {
                            Text = "LogIDs",
                            Command = new Command((s, e) => Logger.Debug(
                                "IDs: {@ids}", AppState.Project?.TopLevelElements
                                                       .Select(tle => tle.Id)
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "Break",
                            Command = new Command((s, e) => { Logger.Debug("BREAK"); })
                        },
                        new ButtonMenuItem
                        {
                            Text = "ConfirmationDialogTest",
                            Command = new Command(
                                (s, e) => new ConfirmationDialog
                                {
                                    Text = "This is the text",
                                    Title = "This is the title",
                                    Yes = "Yes Btn",
                                    No = "No Btn"
                                }.ShowModal()
                            )
                        },
                        new ButtonMenuItem
                        {
                            Text = "SaveToTest",
                            Command = new Command((s, e) =>
                            {
                                SaveManager.Save("Test");
                            })
                        }
                    }
                },
#endif

                // items related to project
                new ButtonMenuItem
                {
                    Text = "&Project",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "&Load",
                            Command = new Command(this.OpenProjectClick)
                        },
                        new SeparatorMenuItem(),
                        new ButtonMenuItem
                        {
                            Text = "Project &Statistics",
                            Command = new Command((s, e) => MessageBoxes.InfoAboutLoadedProject())
                        }
                    }
                },

                // items related to the game state
                new ButtonMenuItem
                {
                    Text = "&Game",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "Start &new",
                            Enabled = false,
                            Command = new Command(this.OnStartNewGameClick)
                        }
                    }
                },

                new ButtonMenuItem
                {
                    Text = "Load / Save",
                    Command = new Command(this.NotImplementedClick)
                }
            },

            AboutItem = new ButtonMenuItem
            {
                Text = "&About",
                Command = new Command(this.NotImplementedClick)
            },

            // Help menu
            HelpItems =
            {
                new ButtonMenuItem
                {
                    Text = "&Creators",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "&Help Text",
                            Command = new Command(this.OnCreatorsHelpClick)
                        },
                        new ButtonMenuItem
                        {
                            Text = "Export yaml &type documentation",
                            Command = new Command(this.OnGenerateTypeDocClick)
                        }
                    }
                },
                new ButtonMenuItem
                {
                    Text = "&Players",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "&TextRpgCreator Help",
                            Command = new Command(this.OnPlayersHelpClick)
                        },
                        new ButtonMenuItem
                        {
                            Text = "&Project Help",
                            Command = new Command(this.OnProjectHelpClick)
                        }
                    }
                }
            }
        };
    }
}