using Eto.Forms;
using Serilog;
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

            new Application(Eto.Platform.Detect)
                .Run(new Views.MainForm());
        }
    }
}