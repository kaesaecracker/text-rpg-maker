using Eto;
using Eto.Forms;
using Serilog;
using TextRpgMaker.Views;
using static Serilog.Log;

namespace TextRpgMaker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logger = new LoggerConfiguration()
                     .MinimumLevel.Verbose()
                     .WriteTo.Console()
                     .CreateLogger();

            Logger.Debug("Startet prgram with parameters {@args}", args);

            AppState.EtoApp = new Application(Platform.Detect);
            AppState.Ui = new MainForm();
            AppState.EtoApp.Run(AppState.Ui);
        }
    }
}