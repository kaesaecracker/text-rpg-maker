using System;
using System.Collections.Generic;
using TextRpgMaker.Models;

namespace TextRpgMaker.IO
{
    public interface IInput
    {
        void GetChoiceAsync(List<Choice> dlgChoices, Action<Choice> callback);

        void GetTextInput(Action<string> callback);
    }
}