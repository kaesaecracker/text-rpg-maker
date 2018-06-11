using System.IO;
using TextRpgMaker.Models;

namespace TextRpgMaker
{
    public static class AppState
    {
        public static Project LoadedProject { get; set; }

        public static bool IsProjectLoaded => LoadedProject != null;
    }
}