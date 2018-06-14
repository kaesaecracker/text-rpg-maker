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
                                Directory.GetCurrentDirectory() + "/../ExampleProject/"
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "LogIDs",
                            Command = new Command((s, e) => Logger.Debug(
                                "IDs: {@ids}", AppState.LoadedProject.TopLevelElements
                                                       .Select(tle => tle.Id)
                            ))
                        },
                        new ButtonMenuItem
                        {
                            Text = "Break",
                            Command= new Command((s, e) =>
                            {
                                Logger.Debug("BREAK");
                            })
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
                            Command = new Command((s, e) => MessageBoxes.InfoAboutLoadedProject())
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
    }
}