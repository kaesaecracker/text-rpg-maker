namespace TextRpgMaker.Views
{
    public partial class MainForm
    {
        private void InitializeEventHandlers()
        {
            AppState.ProjectChangeEvent += this.OnProjectChange;
        }

        private void OnProjectChange(object sender, AppState.ProjectChangedEventArgs e)
        {
            this.Title = e.NewProject != null
                ? $"{e.NewProject.Info.Title} - TextRpgCreator"
                : "TextRpgCreator";
        }
    }
}