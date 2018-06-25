using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TextRpgMaker.IO;
using TextRpgMaker.Models;

namespace TextRpgMaker.Views
{
    public partial class MainForm : IInput, IOutput
    {
        public void GetChoice<T>(List<T> possibleChoices, 
                                 Func<T, string> textRepresentation,
                                 Action<T> callback)
            => this._inputPanel.GetChoice(possibleChoices, textRepresentation, callback);

        public void GetTextInput(Action<string> callback)
            => this._inputPanel.GetTextInput(callback);

        public void Write(string text)
            => this._outputPanel.WriteLine(text);

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