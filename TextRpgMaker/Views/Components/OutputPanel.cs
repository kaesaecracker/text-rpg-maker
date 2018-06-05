using Eto.Forms;

namespace TextRpgMaker.Views.Components
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
                Font = UiConstants.GroupBoxTitleFont,
                Padding = 3,
                Content = new TextArea
                {
                    Height = 250
                }
            };
        }
    }
}