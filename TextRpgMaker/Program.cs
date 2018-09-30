using System;
using System.IO;
using Eto;
using Eto.Forms;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using TextRpgMaker.ProjectModels;
using TextRpgMaker.Views;
using static Serilog.Log;

namespace TextRpgMaker
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppState.Config = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddYamlFile("settings.yaml")
                              .Build()
                              .Get<AppConfig>();
            Logger = new LoggerConfiguration()
                     .MinimumLevel.Is(AppState.IsDebugRun
                         ? LogEventLevel.Debug
                         : LogEventLevel.Information)
                     .WriteTo.Console()
                     .CreateLogger();
            Logger.Debug("PROGRAM: Startet prgram with parameters {@args}", args);

            // todo
            AppState.Config.ValueChangedEvent += (s, e) => { };

            var app = new Application(Platform.Detect);
            AppState.Ui = new MainForm();
            app.Run(AppState.Ui);
        }
    }
}