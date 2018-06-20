using System;
using System.Collections.Generic;
using Eto.Forms;
using TextRpgMaker.Models;
using Button = Eto.Forms.Button;
using ComboBox = Eto.Forms.ComboBox;
using Label = Eto.Forms.Label;

namespace TextRpgMaker.Views.Components
{
    public class InputPanel : Panel
    {
        public InputPanel()
        {
            this.Content = new Label {Text = "When the game is running, you can enter text here."};

            AppState.ProjectChangeEvent += (sender, args) => this.Content = null;
            AppState.GameChangedEvent += (sender, args) => this.Content = null;
        }

        public void GetChoiceAsync(List<Choice> dlgChoices, Action<Choice> action)
        {
            var combo = new ComboBox {DataStore = dlgChoices};
            if (dlgChoices.Count == 1) combo.SelectedIndex = 0; // preselect if only one option

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
                    action.Invoke(dlgChoices[combo.SelectedIndex]);
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