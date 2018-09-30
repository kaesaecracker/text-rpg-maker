using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using TextRpgMaker.Helpers;
using TextRpgMaker.Workers;
using static Serilog.Log;

namespace TextRpgMaker.Views
{
    public class InputPanel : Panel, IOController.IInput
    {
        public InputPanel()
        {
            this.Content = "When the game is running, you can enter text here.";

            AppState.ProjectChangeEvent += (sender, args) => this.Content = null;
            AppState.GameChangedEvent += (sender, args) => this.Content = null;
        }

        public void GetChoice<T>(List<T> possibleChoices,
                                 Func<T, string> textRepresentation,
                                 Action<T> callback)
        {
            var combo = new ComboBox {DataStore = possibleChoices.Select(textRepresentation)};
            if (possibleChoices.Count == 1) combo.SelectedIndex = 0; // preselect if only one option
            combo.KeyDown += (sender, args) =>
            {
                // todo on Gtk, this only triggers then the focus is on the little arrow on the combo box
                if (args.Key == Keys.Enter) RunCallback();
            };

            var btn = new Button {Text = "Confirm"};
            btn.Click += (s, a) => RunCallback();

            void RunCallback()
            {
                if (combo.SelectedIndex == -1)
                    AppState.IO.Write(">> Choose an option from the dropdown below");
                else
                {
                    this.Content = null;
                    callback(possibleChoices[combo.SelectedIndex]);
                }
            }

            this.Content = TableLayout.Horizontal(
                new TableCell {Control = combo, ScaleWidth = true},
                btn
            );

            combo.Focus();
        }

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

            this.Content = TableLayout.Horizontal(
                new TableCell {Control = field, ScaleWidth = true},
                btn
            );

            field.Focus();
        }
    }
}