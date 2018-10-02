using Eto.Drawing;
using Eto.Forms;
using Gtk;
using Button = Eto.Forms.Button;
using Label = Eto.Forms.Label;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// Provides an easy way to get a yes/no answer from the user
    /// </summary>
    public class ConfirmationDialog
    {
        /// <summary>
        /// The title of the modal window.
        /// </summary>
        public string Title { get; set; } = "Confirmation required";

        /// <summary>
        /// The body of the dialog
        /// </summary>
        public string Text { get; set; } = "Please confirm this action";

        /// <summary>
        /// The caption on the button resulting in the return value being true
        /// </summary>
        public string Yes { get; set; } = "Yes";

        /// <summary>
        /// The caption on the button resulting in the return value being false
        /// </summary>
        public string No { get; set; } = "No";

        /// <summary>
        /// Show the dialog and return true or false
        /// </summary>
        /// <returns>
        /// true if yesBtn was clicked,
        /// false if noBtn was clicked or window was closed
        /// </returns>
        public bool ShowModal()
        {
            var dlg = new Dialog<bool> {Title = this.Title};

            var layout = new DynamicLayout
            {
                Padding = 3,
                DefaultSpacing = new Size(3, 3)
            };

            // build layout
            layout.BeginVertical(xscale: true, yscale: true);
            {
                layout.Add(new Label {Text = this.Text});
                layout.Add(new Label {Text = " "}); // spacer

                layout.BeginHorizontal();
                {
                    layout.Add(new Button
                    {
                        Text = this.Yes,
                        Command = new Command((e, s) => dlg.Close(true))
                    });

                    layout.Add(new Button
                    {
                        Text = this.No,
                        Command = new Command((e, s) => dlg.Close(false))
                    });
                }
            }

            // add layout to Dialog
            dlg.Content = layout;
            // show dialog and return result
            return dlg.ShowModal(AppState.Ui);
        }
    }
}