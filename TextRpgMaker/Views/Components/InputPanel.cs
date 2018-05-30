using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class InputPanel : Panel
    {
        public InputPanel()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Content = new GroupBox
            {
                Text = "Input",
                Padding = 3,
                Content = new StackLayout
                {
                    Orientation = Orientation.Horizontal,
                    Items =
                    {
                        new ComboBox(),
                        new Button
                        {
                            Text = "Enter",
                            Command = new Command(MainForm.UnimplementedClick)
                        }
                    }
                }
            };
        }
    }
}