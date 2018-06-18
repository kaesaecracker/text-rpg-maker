using Eto.Drawing;
using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class OutputPanel : Panel
    {
        private TextArea _box;

        public OutputPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this._box = new TextArea
            {
                Height = 350,
                Width = 400
            };

            this.Content = new GroupBox
            {
                Text = "Output",
                Font = UiConstants.GroupBoxTitleFont,
                Padding = 3,
                Content = this._box
            };
        }

        public void WriteLine(string text)
        {
            this._box.Text += text + "\n";
        }
    }
}