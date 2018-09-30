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
                this.HandleDialog(Game.CurrentDialog);
            });
        }

        private void HandleDialog(Dialog dlg)
        {
            // was recursive, this is the iterative way (gotos)
            while (true)
            {
                IO.Write('"' + dlg.Text + '"');
                if (dlg.GotoId != null)
                {
                    dlg = Project.ById<Dialog>(dlg.GotoId);
                    continue;
                }

                var choicesThatMeetRequirements = (
                    from c in dlg.Choices
                    let mismatchedItems = (
                        from reqItem in c.RequiredItems
                        where !Game.PlayerChar.Items.HasItem(reqItem)
                        select reqItem
                    )
                    where !mismatchedItems.Any()
                    select c
                ).ToList();

                if (choicesThatMeetRequirements.Count == 0)
                    this.GetTextInput();
                else
                    IO.GetChoice(choicesThatMeetRequirements, c => c.Text, choice =>
                    {
                        IO.Write($" >> {choice.Text}");
                        this.HandleChoice(choice);
                    });

                break;
            }
        }

        private void GetTextInput() => IO.GetTextInput(this.HandleText);

        private void HandleChoice(Choice choice)
        {
            // remove required items
            foreach (var requiredItem in choice.RequiredItems)
                Game.PlayerChar.Items.RemoveItem(requiredItem);
            
            // give reward items
            foreach (var rewardItem in choice.RewardItems)
                Game.PlayerChar.Items.AddItem(rewardItem);
            
            // apply scene changes
            foreach (var changeCharacter in choice.ChangeCharacters)
                changeCharacter.Apply();
            
            // apply char changes
            foreach (var changeScene in choice.ChangeScenes)
                changeScene.Apply();
           
            
            // Priority: GotoDialog, GotoScene. If none specified, wait for input
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

            IO.Write($">> Command not known: {line}");
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
                    IO.Write($">> Could not find character {idOrName} in current scene");
                    break;

                case 1 when matchingChars.First().TalkDialog == null:
                    IO.Write(
                        $">> {matchingChars.First().Name} does not want to talk to you.");
                    break;

                case 1:
                    IO.Write($">> Talking to {matchingChars.First().Name}");
                    this.HandleDialog(
                        Project.ById<Dialog>(matchingChars.First().TalkDialog));
                    return;

                default:
                    IO.Write(matchingChars.Select(c => c.Name).Aggregate(
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

            this.GetTextInput();
        }

        [InputCommand("lookaround", "Lists elements in scene")]
        private void LookAround(string _)
        {
            OutputHelpers.LookAround();
            this.GetTextInput();
        }

        [InputCommand("inventory", "shows inventory")]
        private void ShowInventory(string _)
        {
            OutputHelpers.PrintInventory();
            this.GetTextInput();
        }
    }
}