using System;
using System.Collections.Generic;
using TextRpgMaker.Models;

namespace TextRpgMaker.IO
{
    public interface IOutput
    {
        void Write(string text);
        
        [Obsolete]
        void Write(List<Choice> choices);
    }
}