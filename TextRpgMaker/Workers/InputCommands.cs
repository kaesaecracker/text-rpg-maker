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
    /// <summary>
    /// The commands you can run when there is the free text box
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification =
        "These methods get called via reflection.")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local", Justification =
        "Parameters are required for the reflection part, but might not be needed by the command.")]
    public class InputCommands
    {
        public static readonly List<(string command, MethodInfo method)> CommandMethods = (
            from method in typeof(InputCommands)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            let attribute = method.GetCustomAttribute<InputCommandAttribute>()
            where attribute != null
            // order by length => "lookaround" shouldnt result in a Look("around")
            orderby attribute.Command descending
            select (
                attribute.Command.ToLower(),
                method
            )
        ).ToList();

        /// <summary>
        /// Show list of available commands
        /// TODO add help for specific command
        /// </summary>
        [InputCommand("help")]
        private static void Help(string _)
        {
            IO.Write("Available commands:");
            foreach (var tuple in CommandMethods)
            {
                IO.Write($"- {tuple.command}");
            }

            IO.GetTextInput();
        }

        /// <summary>
        /// Start talk-dialog of a character
        /// </summary>
        /// <param name="idOrName"></param>
        [InputCommand("talk", "<character-id>")]
        private static void TalkTo(string idOrName)
        {
            // find characters that match ID or name
            var matchingChars = (
                from character in Project.Characters
                where Game.CurrentScene.Characters.Any(c => c.Id == character.Id)
                where string.Equals(character.Name, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(character.Id, idOrName,
                          StringComparison.InvariantCultureIgnoreCase)
                select character
            ).ToList();

            switch (matchingChars.Count)
            {
                case 0:
                    // none found
                    IO.Write($">> Could not find character {idOrName} in current scene");
                    break;

                case 1 when matchingChars.First().TalkDialog == null:
                    // 1 found, does not have dialog
                    IO.Write(
                        $">> {matchingChars.First().Name} does not want to talk to you.");
                    break;

                case 1:
                    // 1 found, has dialog -> start
                    IO.Write($">> Talking to {matchingChars.First().Name}");
                    Project.ById<Dialog>(matchingChars.First().TalkDialog).Start();
                    return;

                default:
                    // more than one found, print all that match
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

        [InputCommand("go")]
        private static void GotoScene(string scene)
        {
            // look for connected scenes by name or id
            var matches = (
                from conn in Game.CurrentScene.Connections
                where string.Equals(conn.Id, scene, StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(conn.Name, scene,
                          StringComparison.InvariantCultureIgnoreCase)
                select conn
            ).ToList();

            switch (matches.Count)
            {
                case 0:
                    // print error message
                    IO.Write("No connection found that matches your input.");
                    break;
                case 1:
                    // goto scene
                    matches.First().Handle();
                    break;
                default:
                    // print error message, write out matches
                    IO.Write(
                        "More than one connection found that matches your input: "
                        + string.Join(", ", matches.Select(m => m.Name + " ID=" + m.Id))
                    );
                    break;
            }

            IO.GetTextInput();
        }

        [InputCommand("take")]
        private static void TakeItem(string item)
        {
            // get items in scene
            var matches = (
                from ig in Game.CurrentScene.Items
                where string.Equals(item, ig.ItemId, StringComparison.InvariantCultureIgnoreCase)
                      || string.Equals(item, Project.ById(item)?.Name)
                select ig
            ).ToList();

            switch (matches.Count)
            {
                case 0:
                    // not found
                    IO.Write("Could not find an item that matches your input. Try 'lookaround'.");
                    break;

                case 1:
                    // found
                    var ig = matches.First();
                    var basicElement = Project.ById(ig.ItemId);

                    Game.PlayerChar.Items.AddItem(ig);
                    new ChangeScene()
                    {
                        TargetScene = Game.CurrentSceneId,
                        ItemToRemove = ig
                    }.Apply();

                    IO.Write($"You found {ig.Count}x {basicElement.Name}. Type 'inventory'.");
                    break;

                default:
                    // more than one match
                    IO.Write(
                        "Found multiple items you could mean: "
                        + string.Join(", ",
                            matches.Select(m => Project.ById(m.ItemId).Name + $" ID={m.ItemId}"))
                    );
                    break;
            }

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