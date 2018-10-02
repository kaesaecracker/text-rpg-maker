using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using Serilog;
using TextRpgMaker.Workers;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// The input panel in the main application window.
    /// </summary>
    public class InputPanel : Panel, IOController.IInput
    {
        public InputPanel()
        {
            this.Content = "When the game is running, you can enter text here.";

            AppState.ProjectChangeEvent += (sender, args) => this.Content = null;
            AppState.GameChangedEvent += (sender, args) => this.Content = null;
        }

        /// <summary>
        /// Let the user choose from a number of possibilities.
        /// Pre-selects first entry if only one is provided.
        /// </summary>
        /// <param name="possibleChoices">A list of choices</param>
        /// <param name="textRepresentation">A function that returns a text representation for each choice</param>
        /// <param name="callback">The method that is called with the chosen element as a parameter</param>
        /// <typeparam name="T">The type of element to chose from</typeparam>
        public void GetChoice<T>(List<T> possibleChoices,
                                 Func<T, string> textRepresentation,
                                 Action<T> callback)
        {
            Log.Debug("InputPanel: Get choice in {num}", possibleChoices.Count);
            
            var combo = new ComboBox {DataStore = possibleChoices.Select(textRepresentation)};
            if (possibleChoices.Count == 1) combo.SelectedIndex = 0; // preselect if only one option
            combo.KeyDown += (sender, args) =>
            {
                // todo on Gtk, this only triggers then the focus is on the little arrow on the combo box
                if (args.Key == Keys.Enter) RunCallback();
            };

            // confirm btn
            var btn = new Button {Text = "Confirm"};
            btn.Click += (s, a) => RunCallback();

            // helper method for running callback
            void RunCallback()
            {
                // nothing chosen or user entered own text -> do not do it
                if (combo.SelectedIndex == -1)
                {
                    AppState.IO.Write(">> Choose an option from the dropdown below");
                    return; // return in helper method, not GetChoice
                }

                // remove choice box and run callback
                this.Content = null;
                callback(possibleChoices[combo.SelectedIndex]);
            }

            // set layout on UI
            this.Content = TableLayout.Horizontal(
                new TableCell {Control = combo, ScaleWidth = true},
                btn
            );

            // set focus to the combo box so you can easily choose with arrow keys, press tab and enter to confirm
            combo.Focus();
        }

        /// <summary>
        /// Get arbitrary text input
        /// </summary>
        /// <param name="action">The callback to be called with the entered text as a parameter</param>
        public void GetTextInput(Action<string> action)
        {
            var field = new TextBox();
            field.KeyDown += (sender, args) =>
            {
                // On enter press, run supplied action
                if (args.Key == Keys.Enter)
                {
                    RunAction();
                }
            };

            var btn = new Button {Text = "Enter"};
            // on button press, run supplied action
            btn.Click += (sender, args) => RunAction();

            void RunAction()
            {
                this.Content = null;
                action.Invoke(field.Text);
            }

            // set layout on UI
            this.Content = TableLayout.Horizontal(
                new TableCell {Control = field, ScaleWidth = true},
                btn
            );

            // set focus to text box so you can enter text without clicking it first
            field.Focus();
        }
    }
}