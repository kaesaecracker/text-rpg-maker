using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class InventoryPanel : GroupBox
    {
        public InventoryPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Inventory";
            this.Padding = 3;
            this.Content = new ListBox
            {
                Items =
                {
                    "[5] Potion",
                    "[3] Gold"
                }
            };
        }
    }
}