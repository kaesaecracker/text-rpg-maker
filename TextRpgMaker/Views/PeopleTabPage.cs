using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class PeopleTabPage:TabPage
    {
        public PeopleTabPage()
        {
            this.InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "People";
            this.Content = new Label {Text = "Persons panel goes here"};
        }
    }
}