using Eto.Drawing;
using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class OutputPanel : Panel
    {
        public OutputPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Content = new GroupBox
            {
                Text = "Output",
                Padding = 3,
                Content = new TextArea
                {
                    Height = 250
                }
            };
        }
    }
}