using Eto.Forms;

namespace TextRpgMaker.Views.Components
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