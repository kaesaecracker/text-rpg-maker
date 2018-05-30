using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class EquipTabPage : TabPage
    {
        public EquipTabPage()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Equip";
            this.Padding = 3;
            this.Content = new TableLayout
            {
                Rows =
                {
                    new TableRow()
                    {
                        Cells =
                        {
                            new Label {Text = "Weapon"},
                            new Label {Text = "Some weapon"}
                        }
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            new Label {Text = "Head"},
                            new Label {Text = ""}
                        }
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            new Label {Text = "Body"},
                            new Label {Text = ""}
                        }
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            new Label {Text = "Hand"},
                            new Label {Text = "Simple leather gloves"}
                        }
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            new Label {Text = "Legs"},
                            new Label {Text = ""}
                        }
                    },
                    new TableRow()
                    {
                        Cells =
                        {
                            new Label {Text = "Shoes"},
                            new Label {Text = "Simple leather shoes of speed"}
                        }
                    },
                }
            };
        }
    }
}