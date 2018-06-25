using System.Linq;
using System.Runtime.CompilerServices;
using TextRpgMaker.IO;
using TextRpgMaker.Models;
using static TextRpgMaker.AppState;

namespace TextRpgMaker
{
    public static class OutputHelpers
    {
        public static void LookAround(IOutput output)
        {
            output.Write(">> look around");

            var characters = AppState.Project
                                     .ById<Character>(Game.CurrentScene.Characters)
                                     .Select(c => c.Name)
                                     .ToList();
            if (characters.Any())
                output.Write("Characters: " +
                             characters.Aggregate((str, cName) => $"{str}, {cName}"));
            // todo look around
        }

        public static void PrintCharacter(Character c, IOutput output) => output.Write(
            $"{c.Name}: {c.LookText}\n" +
            $"- Attack: {c.Attack}\n" +
            $"- Evade: {c.Evade}\n" +
            $"- Health: {c.Health}\n" +
            $"- Speed: {c.Speed}\n" +
            $"- Items: {c.Items}"
        );

        public static void PrintInventory(IOutput output)
        {
            var items =
                from ig in Game.PlayerChar.Items
                select (
                    Element: Project.TopLevelElements.First(tle => tle.Id == ig.ItemId),
                    ig.Count
                );
            string text = items.Aggregate("Current Inventory:", (s, tuple) =>
                $"{s}\n- {tuple.Element.Name}"
                + ((tuple.Count != 1)
                    ? $" [{tuple.Count}]"
                    : "")
            );

            output.Write(text);
        }
    }
}