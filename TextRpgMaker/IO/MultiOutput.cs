using System.Collections.Generic;
using TextRpgMaker.Models;

namespace TextRpgMaker.IO
{
    public class MultiOutput : IOutput
    {
        private readonly IOutput _a, _b;

        public MultiOutput(IOutput a, IOutput b)
        {
            this._a = a;
            this._b = b;
        }

        public void Write(string text)
        {
            this._a.Write(text);
            this._b.Write(text);
        }

        public void Write(List<Choice> choices)
        {
            this._a.Write(choices);
            this._b.Write(choices);
        }
    }
}