using System;
using System.Collections.Generic;
using TextRpgMaker.ProjectModels;
using TextRpgMaker.Workers;

namespace TextRpgMaker.Views
{
    /// <summary>
    /// The main application window. This file implements the IInput and IOutput interfaces, so the
    /// window can be used to output text or input something.
    /// </summary>
    public partial class MainForm : IOController.IInput, IOController.IOutput
    {
        public void GetChoice<T>(List<T> possibleChoices,
                                 Func<T, string> textRepresentation,
                                 Action<T> callback)
        {
            this._inputPanel.GetChoice(possibleChoices, textRepresentation, callback);
        }

        public void GetTextInput(Action<string> callback)
        {
            this._inputPanel.GetTextInput(callback);
        }

        public void Write(string text)
        {
            this._outputPanel.WriteLine(text);
        }

        public void Write(List<Choice> choices)
        {
            for (int index = 0; index < choices.Count; index++)
            {
                var c = choices[index];
                this._outputPanel.WriteLine($"{index}. {c.Text}");
            }
        }
    }
}