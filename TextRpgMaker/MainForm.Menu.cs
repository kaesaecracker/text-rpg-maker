using Eto.Forms;

namespace TextRpgMaker
{
    public partial class MainForm
    {
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
    }
}