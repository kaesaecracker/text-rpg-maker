using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using OpenTK.Graphics;
using Serilog;
using TextRpgMaker.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static Serilog.Log;

namespace TextRpgMaker
{
    public class LoadTester
    {
        private const string PathToProject = "../ExampleProject";

        public static void Main(string[] args)
        {
            Logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Console()
                     .CreateLogger();
            Logger.Verbose("State: {@consts}", new[]
            {
                ("Project: ", PathToProject),
                ("Current path: ", Directory.GetCurrentDirectory()),
                ("resulting path: ",
                    Path.GetFullPath(Directory.GetCurrentDirectory() + "/" + PathToProject))
            });

            var project = new Project(PathToProject);
            Logger.Debug("Project: ", project);
        }
    }
}