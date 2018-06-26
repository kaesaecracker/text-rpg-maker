using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using TextRpgMaker.Helpers;

namespace TextRpgMaker.Views
{
    public class InputPanel : Panel, IInput
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

            var btn = new Button {Text = "Confirm"};
            btn.Click += (s, a) =>
            {
                if (combo.SelectedIndex == -1) // nothing selected
                {
                    AppState.Ui.Write(">> Choose an option from the dropdown below");
                }
                else
                {
                    this.Content = null;
                    callback(possibleChoices[combo.SelectedIndex]);
                }
            };

            this.Content = TableLayout.Horizontal(
                new TableCell {Control = combo, ScaleWidth = true},
                btn
            );
        }

        public void GetTextInput(Action<string> action)
        {
            var field = new TextBox();
            var btn = new Button {Text = "Enter"};
            btn.Click += (s, a) =>
            {
                this.Content = null;
                action.Invoke(field.Text);
            };

            this.Content = TableLayout.Horizontal(
                new TableCell {Control = field, ScaleWidth = true},
                btn
            );
        }
    }
}