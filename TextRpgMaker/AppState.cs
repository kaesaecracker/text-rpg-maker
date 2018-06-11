using Eto.Forms;
using TextRpgMaker.Models;
using TextRpgMaker.Views;

namespace TextRpgMaker
{
    public static class AppState
    {
        public static Project LoadedProject { get; set; }

        public static bool IsProjectLoaded => LoadedProject != null;

        public static MainForm Ui { get; set; }

        public static Application EtoApp { get; set; }
    }
}