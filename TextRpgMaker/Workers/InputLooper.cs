using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Helpers;
using TextRpgMaker.Models;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.Workers
{
    public class InputLooper
    {
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
                // order by length => "lookaround" shouldnt result in a Look("around")
                orderby attribute.Command.Length descending
                select (
                    attribute.Command.ToLower(),
                    method
                )
            ).ToList();
        }

        private IOutput Output { get; } = Ui.And(new LogOutput());
        private IInput Input { get; } = Ui;

        public void StartFromNewGame()
        {
            this.Output.Write("Choose one of the following characters:");

            var startChars = (
                from id in Project.StartInfo.CharacterIds
                select Project.Characters.GetId(id)
            ).ToList();

            foreach (var c in startChars)
            {
                OutputHelpers.PrintCharacter(c, this.Output);
                this.Output.Write("");
            }

            this.Input.GetChoice(startChars, c => c.Name, choosenChar =>
            {
                Game.PlayerChar = choosenChar;
                this.Output.Write($">> {choosenChar.Name}\n");
                this.Output.Write(Project.StartInfo.IntroText
                                  ?? "The project does not have an intro text");
                this.HandleDialog(Game.CurrentDialog);
            });
        }

        private void HandleDialog(Dialog dlg)
        {
            // was recursive, this is the iterative way (gotos)
            while (true)
            {
                this.Output.Write('"' + dlg.Text + '"');
                if (dlg.GotoId != null)
                {
                    dlg = Project.ById<Dialog>(dlg.GotoId);
                    continue;
                }

                // todo only allow choices that meet the requirements
                if (dlg.Choices != null && dlg.Choices.Count != 0)
                    this.Input.GetChoice(dlg.Choices, c => c.Text, choice =>
                    {
                        this.Output.Write($" >> {choice.Text}");
                        this.HandleChoice(choice);
                    });
                else this.GetTextInput();
                break;
            }
        }

        private void GetTextInput() => this.Input.GetTextInput(this.HandleText);

        private void HandleChoice(Choice choice)
        {
            // todo remove required items
            // todo give reward items
            if (choice.GotoDialogId != null)
                this.HandleDialog(Project.Dialogs.GetId(choice.GotoDialogId));
            else if (choice.GotoSceneId != null)
                this.HandleScene(Project.Scenes.GetId(choice.GotoSceneId));
            else this.GetTextInput();
        }

        private void HandleScene(Scene scene)
        {
            Game.CurrentScene = scene;
            // todo handle scene
        }

        private void HandleText(string line)
        {
            string lineLower = line.Trim().ToLower();
            foreach ((string command, var method) in this._commandMethods)
            {
                if (!lineLower.StartsWith(command)) continue;

                string lineWithoutCommand = line.Substring(command.Length).Trim();
                method.Invoke(this, new object[] {lineWithoutCommand});

                return;
            }

            this.Output.Write($">> Command not known: {line}");
            this.GetTextInput();
        }

        [InputCommand("talk", "<character-id>")]
        private void TalkTo(string idOrName)
        {
            var matchingChars = (
                from character in Project.Characters
                where Game.CurrentScene.Characters.Contains(character.Id)
                where string.Equals(character.Name, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(character.Id, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                select character
            ).ToList();

            switch (matchingChars.Count)
            {
                case 0:
                    this.Output.Write($">> Could not find character {idOrName} in current scene");
                    break;

                case 1 when matchingChars.First().TalkDialog == null:
                    this.Output.Write(
                        $">> {matchingChars.First().Name} does not want to talk to you.");
                    break;

                case 1:
                    this.Output.Write($">> Talking to {matchingChars.First().Name}");
                    this.HandleDialog(
                        Project.ById<Dialog>(matchingChars.First().TalkDialog));
                    return;

                default:
                    this.Output.Write(matchingChars.Select(c => c.Name).Aggregate(
                        ">> There are multiple characters you could mean:",
                        (str, elem) => str + ", " + elem
                    ));
                    break;
            }

            this.GetTextInput();
        }

        [InputCommand("look", "look at element with the specified id or name")]
        private void LookAt(string idOrName)
        {
            // todo only allow look at elements in scene / inventory
            var elems = (
                from Element element in Project.TopLevelElements
                where string.Equals(element.Id, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(element.Name, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                select element
            ).ToList();

            switch (elems.Count)
            {
                case 0:
                    this.Output.Write($">> Could not find '{idOrName}' in current scene");
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

            this.GetTextInput();
        }

        [InputCommand("lookaround", "Lists elements in scene")]
        private void LookAround(string _)
        {
            OutputHelpers.LookAround(this.Output);
            this.GetTextInput();
        }

        [InputCommand("inventory", "shows inventory")]
        private void ShowInventory(string _)
        {
            OutputHelpers.PrintInventory(this.Output);
            this.GetTextInput();
        }
    }
}