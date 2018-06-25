﻿using System;
using System.Collections.Generic;

namespace TextRpgMaker.IO
{
    public interface IInput
    {
        void GetChoice<T>(List<T> possibleChoices, 
                          Func<T, string> textRepresentation,
                          Action<T> callback);

        void GetTextInput(Action<string> callback);
    }
}