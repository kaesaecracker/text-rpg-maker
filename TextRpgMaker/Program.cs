using System;
using Eto.Forms;
using Serilog;
using Serilog.Events;

namespace TextRpgMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Verbose()
#else
                .MinimumLevel.Information()
#endif
                .WriteTo.Console()
                .CreateLogger();
            
            Log.Logger.Debug("Startet prgram with parameters {@args}", args);
            
            new Application(Eto.Platform.Detect).Run(new MainForm());
        }
    }
}