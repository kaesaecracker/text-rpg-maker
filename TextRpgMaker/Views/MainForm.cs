using System;
using Eto.Drawing;
using Eto.Forms;
using static Serilog.Log;
using TextRpgMaker.Models;
using TextRpgMaker.Views.Components;

namespace TextRpgMaker.Views
{
    public class MainForm : Form
    {
        public MainForm()
        {
            this.InitializeComponents();

            var s = new State();
            Logger.Debug("State: {@s}", s);
        }

        private void InitializeComponents()
        {
            this.Title = "No project loaded [TextRpgMaker]";
            this.DataContext = new MainViewModel();
            this.Menu = InitializeMenu();

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

        private static MenuBar InitializeMenu() => new MenuBar
        {
            Items =
            {
                new ButtonMenuItem
                {
                    Text = "Open Game",
                    Command = new Command(UnimplementedClick)
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

        private void ExitClick(object sender, EventArgs e) => Application.Instance.Quit();

        internal static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
    }
}