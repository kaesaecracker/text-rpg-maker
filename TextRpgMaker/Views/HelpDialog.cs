using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace TextRpgMaker.Views
{
    public class HelpDialog : Dialog
    {
        private readonly TextArea _textArea;
        private readonly Label _indexLabel;
        private readonly List<string> _pages;
        private int _index = 0;

        public HelpDialog(List<string> pages)
        {
            if (pages == null || pages.Count == 0)
                throw new ArgumentException("Should not be null or empty", nameof(pages));
            this._pages = pages;

            this.Resizable = true;
            this.Title = "Help";
            this._indexLabel = new Label {Text = "1"};
            this._textArea = new TextArea
            {
                Text = pages[0],
                TextAlignment = TextAlignment.Left,
                Width = 400,
                Height = 400
            };

            // todo layout breaks when index >= 10
            var topLayout = new DynamicLayout {Width = 400};
            topLayout.BeginHorizontal();
            {
                topLayout.Add(new Button(this.PrevClick) {Text = "<-"}, xscale: false);
                topLayout.Add(
                    // todo center this properly
                    new StackLayout
                    {
                        MinimumSize = new Size(250, 20),
                        Orientation = Orientation.Horizontal,
                        Items =
                        {
                            this._indexLabel,
                            $" / {this._pages.Count}"
                        }
                    },
                    xscale: true
                );

                topLayout.Add(new Button(this.NextClick) {Text = "->"}, xscale: false);
            }
            topLayout.EndHorizontal();

            this.Content = new StackLayout
            {
                Orientation = Orientation.Vertical,
                Items =
                {
                    topLayout,
                    this._textArea
                }
            };
        }

        private void NextClick(object sender, EventArgs e)
        {
            if (this._pages.Count <= this._index + 1) return;

            this._index++;
            this.UpdateUi();
        }

        private void PrevClick(object sender, EventArgs e)
        {
            if (this._index == 0) return;

            this._index--;
            this.UpdateUi();
        }

        private void UpdateUi()
        {
            this._textArea.Text = this._pages[this._index];
            this._indexLabel.Text = (this._index + 1).ToString();
        }
    }
}