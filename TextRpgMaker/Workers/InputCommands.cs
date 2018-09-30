using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using TextRpgMaker.Helpers;
using TextRpgMaker.ProjectModels;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.Workers
{
    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "These methods get called via reflection.")]
    public class InputCommands
    {
        [InputCommand("talk", "<character-id>")]
        private static void TalkTo(string idOrName)
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
        private static void LookAt(string idOrName)
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
        private static void LookAround(string _)
        {
            OutputHelpers.LookAround();
            IO.GetTextInput();
        }

        [InputCommand("inventory", "shows inventory")]
        private static void ShowInventory(string _)
        {
            OutputHelpers.PrintInventory();
            IO.GetTextInput();
        }
    }
}