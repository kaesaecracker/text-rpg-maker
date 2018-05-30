using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class InfoTabsPanel : Panel
    {
        public InfoTabsPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Content = new TabControl
            {
                Pages =
                {
                    new SceneTabPage(),
                    new PeopleTabPage(),
                    new ObjectsTabPage()
                }
            };
        }
    }
}