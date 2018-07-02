using Serilog;

namespace TextRpgMaker.Helpers
{
    public class LogOutput : IOutput
    {
        public void Write(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            Log.Logger.Information("GAME: {text}", text.Replace("\n", "\\n "));
        }
    }
}