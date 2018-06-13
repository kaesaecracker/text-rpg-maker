using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class VitalsPanel : Panel
    {
        public VitalsPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            var table = new TableLayout(columns: 2, rows: 1)
            {
                Padding = 3
            };

            table.Add(new Label {Text = "HP"}, x: 0, y: 0);
            table.Add(new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 90
            }, x: 1, y: 0);

            this.Content = new GroupBox
            {
                Content = table,
                Text = "Vitals",
                Font = UiConstants.GroupBoxTitleFont
            };
        }
    }
}