using Eto.Forms;
using Serilog;
using static Serilog.Log;

namespace TextRpgMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger = new LoggerConfiguration()
#if DEBUG
                     .MinimumLevel.Verbose()
#else
                .MinimumLevel.Information()
#endif
                     .WriteTo.Console()
                     .CreateLogger();

            Logger.Debug("Startet prgram with parameters {@args}", args);

            new Application(Eto.Platform.Detect)
                .Run(new Views.MainForm());
        }
    }
}