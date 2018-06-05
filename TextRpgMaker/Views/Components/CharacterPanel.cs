using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class CharacterPanel : GroupBox
    {
        public CharacterPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Character";
            this.Font = UiConstants.GroupBoxTitleFont;
            this.Padding = 3;
            this.Content = new TabControl
            {
                Pages =
                {
                    new EquipTabPage(),
                    new StatsTabPage()
                }
            };
        }
    }
}