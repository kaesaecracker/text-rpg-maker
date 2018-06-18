using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Eto.Drawing;
using Eto.Forms;
using TextRpgMaker.Views.Components;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    public partial class MainForm
    {
        private OutputPanel _outputPanel;
        private InputPanel _inputPanel;

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
                                Directory.GetCurrentDirectory() + "/../ExampleProject/"
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "LogIDs",
                            Command = new Command((s, e) => Logger.Debug(
                                "IDs: {@ids}", AppState.LoadedProject?.TopLevelElements
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
                            Text = "SelfDocument",
                            Command = new Command((s, e) =>
                                SelfDocumenter.Document("documentation.yaml"))
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
                            Command = new Command(OnStartNewGameClick)
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