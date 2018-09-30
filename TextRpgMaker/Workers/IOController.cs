using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog;
using TextRpgMaker.Helpers;

namespace TextRpgMaker.Workers
{
    public class IOController : IOController.IOutput, IOController.IInput
    {
        private IOutput Output { get; } = AppState.Ui.And(new LogOutput());
        private IInput Input { get; } = AppState.Ui;
        private readonly List<(string command, MethodInfo method)> _commandMethods;

        public IOController()
        {
            this._commandMethods = (
                from method in typeof(InputLooper)
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                let attribute = method.GetCustomAttribute<InputCommandAttribute>()
                where attribute != null
                // order by length => "lookaround" shouldnt result in a Look("around")
                orderby attribute.Command.Length descending
                select (
                    attribute.Command.ToLower(),
                    method
                )
            ).ToList();
        }


        public void Write(string text)
        {
            this.Output.Write(text);
        }

        public void GetChoice<T>(List<T> possibleChoices, Func<T, string> textRepresentation,
                                 Action<T> callback)
        {
            this.Input.GetChoice(possibleChoices, textRepresentation, callback);
        }

        public void GetTextInput() => this.GetTextInput(HandleText);

        public void GetTextInput(Action<string> callback)
        {
            this.Input.GetTextInput(callback);
        }

        public void HandleText(string line)
        {
            string lineLower = line.Trim().ToLower();
            foreach ((string command, var method) in this._commandMethods)
            {
                if (!lineLower.StartsWith(command)) continue;

                string lineWithoutCommand = line.Substring(command.Length).Trim();
                method.Invoke(this, new object[] {lineWithoutCommand});

                return;
            }

            this.Write($">> Command not known: {line}");
            this.GetTextInput();
        }

        // TODO remove old interfaces as they are not really needed anymore
        public interface IInput
        {
            void GetChoice<T>(List<T> possibleChoices,
                              Func<T, string> textRepresentation,
                              Action<T> callback);

            void GetTextInput(Action<string> callback);
        }

        public interface IOutput
        {
            void Write(string text);
        }

        public class LogOutput : IOutput
        {
            public void Write(string text)
            {
                if (string.IsNullOrWhiteSpace(text)) return;
                Log.Logger.Information("GAME: {text}", text.Replace("\n", "\\n "));
            }
        }

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
}