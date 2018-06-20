using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextRpgMaker.IO;
using TextRpgMaker.Models;
using static Serilog.Log;
using static TextRpgMaker.AppState;
using Dialog = TextRpgMaker.Models.Dialog;

namespace TextRpgMaker.Workers
{
    public class InputLooper
    {
        private IOutput Output { get; } = Ui.And(new LogOutput());
        private IInput Input { get; } = Ui;
        private readonly List<(string command, MethodInfo method)> _commandMethods;

        public InputLooper()
        {
            if (!IsProjectLoaded)
                throw new InvalidOperationException(
                    "Cannot create InputLooper when no project is loaded");
            if (!IsGameRunning)
                throw new InvalidOperationException(
                    "Cannot create InputLooper when game is not running");

            this._commandMethods = (
                from method in this.GetType()
                                   .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                let attribute = method.GetCustomAttribute<InputCommandAttribute>()
                where attribute != null
                select (
                    attribute.Command.ToLower(),
                    method
                )
            ).ToList();

            this.HandleDialog(AppState.Project.ById<Dialog>(AppState.Project.StartInfo.DialogId));
        }

        private void HandleDialog(Dialog dlg)
        {
            // was recursive, this is the iterative way (gotos)
            while (true)
            {
                this.Output.Write(dlg.Text);
                if (dlg.GotoId != null)
                {
                    dlg = AppState.Project.ById<Dialog>(dlg.GotoId);
                    continue;
                }

                // todo only allow choices that meet the requirements
                this.Input.GetChoiceAsync(dlg.Choices, choice =>
                {
                    this.Output.Write($" >> {choice.Text}");
                    this.HandleChoice(choice);
                });
                break;
            }
        }

        private void HandleChoice(Choice choice)
        {
            // todo remove required items
            // todo give reward items
            if (choice.GotoId != null)
                this.HandleDialog(AppState.Project.ById<Dialog>(choice.GotoId));
            else this.Input.GetTextInput(this.HandleText);
        }

        private void HandleText(string line)
        {
            Logger.Debug("input {line}", line);
            // todo parse command

            string lineLower = line.Trim().ToLower();
            foreach ((string command, var method) in this._commandMethods)
            {
                if (!lineLower.StartsWith(command)) continue;

                string lineWithoutCommand = line.Substring(command.Length).Trim();
                method.Invoke(this, new object[] {lineWithoutCommand});

                return;
            }

            this.Output.Write($">> Command not known: {line}");
            this.Input.GetTextInput(this.HandleText);
        }

        [InputCommand("talk", "<character-id>")]
        private void TalkCommand(string restOfLine)
        {
            var matchingChars = (
                from character in AppState.Project.Characters
                where Game.CurrentScene.Characters.Contains(character.Id)
                where string.Equals(character.Name, restOfLine,
                          StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(character.Id, restOfLine,
                          StringComparison.InvariantCultureIgnoreCase)
                select character
            ).ToList();

            switch (matchingChars.Count)
            {
                case 0:
                    this.Output.Write($">> Could not find character {restOfLine} in current scene");
                    break;

                case 1 when matchingChars.First().TalkDialog == null:
                    this.Output.Write(
                        $">> {matchingChars.First().Name} does not want to talk to you.");
                    break;

                case 1:
                    this.Output.Write($">> Talking to {matchingChars.First().Name}");
                    this.HandleDialog(
                        AppState.Project.ById<Dialog>(matchingChars.First().TalkDialog));
                    return;

                default:
                    this.Output.Write(matchingChars.Select(c => c.Name).Aggregate(
                        ">> There are multiple characters you could mean:",
                        (str, elem) => str + ", " + elem
                    ));
                    break;
            }

            this.Input.GetTextInput(this.HandleText);
        }

        [InputCommand("look")]
        private void LookCommand(string args)
        {
            var elems = (
                from Element element in AppState.Project.TopLevelElements
                where string.Equals(element.Id, args,
                          StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(element.Name, args,
                          StringComparison.InvariantCultureIgnoreCase)
                select element
            ).ToList();

            switch (elems.Count)
            {
                case 0 when string.IsNullOrWhiteSpace(args):
                    OutputHelpers.LookAround(this.Output);
                    break;

                case 0:
                    this.Output.Write($">> Could not find {args} in current scene");
                    break;

                case 1 when elems.First().LookText == null:
                    this.Output.Write(
                        $">> {elems.First().Name} does not have a look text.");
                    break;

                case 1:
                    this.Output.Write($">> Look at {elems.First().Name}");
                    this.Output.Write(elems.First().LookText);
                    break;

                default:
                    this.Output.Write(elems.Select(c => c.Name).Aggregate(
                        ">> There are multiple characters you could mean:",
                        (str, elem) => str + ", " + elem
                    ));
                    break;
            }

            this.Input.GetTextInput(this.HandleText);
        }
    }
}