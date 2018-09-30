using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Helpers;
using TextRpgMaker.ProjectModels;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.Workers
{
    public class InputLooper
    {
        public InputLooper()
        {
            if (!IsProjectLoaded)
                throw new InvalidOperationException(
                    "Cannot create InputLooper when no project is loaded");
            if (!IsGameRunning)
                throw new InvalidOperationException(
                    "Cannot create InputLooper when game is not running");
        }

        public void StartFromNewGame()
        {
            IO.Write("Choose one of the following characters:");

            var startChars = (
                from id in Project.StartInfo.CharacterIds
                select Project.Characters.GetId(id)
            ).ToList();

            foreach (var c in startChars)
            {
                OutputHelpers.PrintCharacter(c);
                IO.Write("");
            }

            IO.GetChoice(startChars, c => c.Name, choosenChar =>
            {
                Game.PlayerChar = choosenChar;
                IO.Write($">> {choosenChar.Name}\n");
                IO.Write(Project.StartInfo.IntroText
                         ?? "The project does not have an intro text");
                Game.CurrentDialog.Start();
            });
        }

        private void HandleScene(Scene scene)
        {
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
                    IO.Write($">> Could not find character {idOrName} in current scene");
                    break;

                case 1 when matchingChars.First().TalkDialog == null:
                    IO.Write(
                        $">> {matchingChars.First().Name} does not want to talk to you.");
                    break;

                case 1:
                    IO.Write($">> Talking to {matchingChars.First().Name}");
                    Project.ById<Dialog>(matchingChars.First().TalkDialog).Start();
                    return;

                default:
                    IO.Write(matchingChars.Select(c => c.Name).Aggregate(
                        ">> There are multiple characters you could mean:",
                        (str, elem) => str + ", " + elem
                    ));
                    break;
            }

            IO.GetTextInput();
        }

        [InputCommand("look", "look at element with the specified id or name")]
        private void LookAt(string idOrName)
        {
            // todo only allow look at elements in scene / inventory
            var elems = (
                from element in Project.TopLevelElements.OfType<LookableElement>()
                where string.Equals(element.Id, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(element.Name, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                select element
            ).ToList();

            switch (elems.Count)
            {
                case 0:
                    IO.Write($">> Could not find '{idOrName}' in current scene");
                    break;

                case 1 when elems.First().LookText == null:
                    IO.Write(
                        $">> {elems.First().Name} does not have a look text.");
                    break;

                case 1:
                    IO.Write($">> Look at {elems.First().Name}");
                    IO.Write(elems.First().LookText);
                    break;

                default:
                    IO.Write(elems.Select(c => c.Name).Aggregate(
                        ">> There are multiple characters you could mean:",
                        (str, elem) => str + ", " + elem
                    ));
                    break;
            }

            IO.GetTextInput();
        }

        [InputCommand("lookaround", "Lists elements in scene")]
        private void LookAround(string _)
        {
            OutputHelpers.LookAround();
            IO.GetTextInput();
        }

        [InputCommand("inventory", "shows inventory")]
        private void ShowInventory(string _)
        {
            OutputHelpers.PrintInventory();
            IO.GetTextInput();
        }
    }
}