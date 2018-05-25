using System;
using Eto.Forms;

namespace TextRpgMaker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            this.Title = "No project loaded [TextRpgMaker]";
            this.DataContext = new MainViewModel();
            this.Menu = InitializeMenu();
            this.Content = this.InitializeLayout();
        }

        private void ExitClick(object sender, EventArgs e) => Application.Instance.Quit();

        private static void UnimplementedClick(object sender, EventArgs e)
            => MessageBox.Show("Not implemented yet");
    }
}