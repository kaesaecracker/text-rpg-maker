using System;
using System.Collections.Generic;

namespace TextRpgMaker.Helpers
{
    public interface IInput
    {
        void GetChoice<T>(List<T> possibleChoices, 
                          Func<T, string> textRepresentation,
                          Action<T> callback);

        void GetTextInput(Action<string> callback);
    }
    
    public interface IOutput
    {
        void Write(string text);
    }
}