using System;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace TextRpgMaker
{
    public class MainForm : Form
    {
        public MainForm()
        {
            XamlReader.Load(this);
        }

        protected void HandleTodo(object sender, EventArgs e)
        {
            MessageBox.Show("This functionality is not yet implemented");
        }
        
        protected void HandleAbout(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog(this);
        }

        protected void HandleQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}