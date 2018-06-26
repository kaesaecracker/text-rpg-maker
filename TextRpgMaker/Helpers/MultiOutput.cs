namespace TextRpgMaker.Helpers
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
    }
}