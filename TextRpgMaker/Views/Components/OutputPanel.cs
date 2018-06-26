using Eto.Forms;

namespace TextRpgMaker.Views.Components
{
    public class OutputPanel : Panel
    {
        private readonly TextArea _box;

        public OutputPanel()
        {
            this._box = new TextArea
            {
                Height = 350,
                Width = 400,
                Enabled = false,
                Text = "You have not loaded a project yet.\n" +
                       "To load a project, click [Project]->[Load]"
            };

            this.Content = this._box;

            AppState.ProjectChangeEvent += (sender, args) =>
                this._box.Text = "You loaded a project, but have not started a new game yet.\n" +
                                 "To start a new game, click [Game]->[Start new]";
            AppState.GameChangedEvent += (sender, args) => this._box.Text = string.Empty;
        }

        public void WriteLine(string text)
        {
            this._box.Append(text + "\n", true);
        }
    }
}