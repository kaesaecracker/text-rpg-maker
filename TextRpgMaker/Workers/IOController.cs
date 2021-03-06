using System;
using System.Collections.Generic;
using Serilog;
using TextRpgMaker.Helpers;

namespace TextRpgMaker.Workers
{
    public class IOController : IOController.IOutput, IOController.IInput
    {
        private IOutput Output { get; set; } = new LogOutput();
        private IInput Input { get; set; } = AppState.Ui;

        public void RegisterOutput(IOutput additionalOutput)
        {
            this.Output = this.Output.And(additionalOutput);
        }

        public void ReplaceInput(IInput newInput)
        {
            this.Input = newInput;
        }

        public void Write(string text)
        {
            this.Output.Write(text);
        }

        public void GetChoice<T>(List<T> possibleChoices, Func<T, string> textRepresentation,
                                 Action<T> callback)
        {
            Log.Debug("Get one of {num} choices", possibleChoices.Count);
            this.Input.GetChoice(possibleChoices, textRepresentation, callback);
        }

        public void GetTextInput() => this.GetTextInput(this.HandleText);

        public void GetTextInput(Action<string> callback)
        {
            AppState.Game.CurrentDialogId = null;
            this.Input.GetTextInput(callback);
        }

        private void HandleText(string line)
        {
            string lineLower = line.Trim().ToLower();
            foreach ((string command, var method) in InputCommands.CommandMethods)
            {
                if (!lineLower.StartsWith(command)) continue;

                string lineWithoutCommand = line.Substring(command.Length).Trim();
                method.Invoke(null, new object[] {lineWithoutCommand});

                return;
            }

            this.Write(
                $">> Command not known.\n" +
                $"   Try 'help' or '/?' for a list of commands.\n" +
                $"   Input: {line}"
            );
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
                this._a = a ?? throw new ArgumentNullException(nameof(a));
                this._b = b ?? throw new ArgumentNullException(nameof(b));
            }

            public void Write(string text)
            {
                this._a.Write(text);
                this._b.Write(text);
            }
        }
    }
}