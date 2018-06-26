using System.IO;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using TextRpgMaker.Views.Components;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    public partial class MainForm
    {
        private InputPanel _inputPanel;
        private OutputPanel _outputPanel;

        private void InitializeComponents()
        {
            this.Title = "TextRpgCreator";
            this.Menu = this.InitializeMenu();

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
                                Directory.GetCurrentDirectory() + "/../ExampleProject/"
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "StartExampleProject",
                            Command = new Command((sender, args) =>
                            {
                                this.OpenProject(Directory.GetCurrentDirectory() +
                                                 "/../ExampleProject/");
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
                        }
                    }
                },
#endif

                new ButtonMenuItem
                {
                    Text = "Project",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "Load",
                            Command = new Command(this.OpenProjectClick)
                        },
                        new SeparatorMenuItem(),
                        new ButtonMenuItem
                        {
                            Text = "Project Statistics",
                            Command = new Command((s, e) => MessageBoxes.InfoAboutLoadedProject())
                        }
                    }
                },

                new ButtonMenuItem
                {
                    Text = "Game",
                    Items =
                    {
                        new ButtonMenuItem
                        {
                            Text = "Start new",
                            Enabled = false,
                            Command = new Command(this.OnStartNewGameClick)
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
                },
                new ButtonMenuItem
                {
                    Text = "Export yaml type documentation",
                    Command = new Command(this.OnGenerateTypeDocClick)
                }
            }
        };
    }
}