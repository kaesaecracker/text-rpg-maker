using Eto.Forms;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// The output panel on the main application window.
    /// </summary>
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
                // Explain how to load a project on start
                Text = "You have not loaded a project yet.\n" +
                       "To load a project, click [Project]->[Load]"
            };

            this.Content = this._box;

            // explain how to start a new game when project is loaded
            AppState.ProjectChangeEvent += (sender, args) =>
                this._box.Text = "You loaded a project, but have not started a new game yet.\n" +
                                 "To start a new game, click [Game]->[Start new]";
            
            // reset output window if new game is started
            AppState.GameChangedEvent += (sender, args) => this._box.Text = string.Empty;
        }

        /// <summary>
        /// Write a string to the output panel.
        /// </summary>
        public void WriteLine(string text)
        {
            this._box.Append(text + "\n", true);
        }
    }
}