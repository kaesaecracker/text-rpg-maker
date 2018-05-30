using System.Runtime.CompilerServices;
using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class SceneTabPage : TabPage
    {
        public SceneTabPage()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Scene";
            this.Content = new Label {Text = "scene info goes here"};
        }
    }
}