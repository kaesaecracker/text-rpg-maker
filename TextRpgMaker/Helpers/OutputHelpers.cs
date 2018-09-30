using System.Linq;
using TextRpgMaker.ProjectModels;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.Helpers
{
    public static class OutputHelpers
    {
        public static void LookAround()
        {
            IO.Write(">> look around");

            var characters = AppState.Project.Characters
                                     .GetIds(AppState.Game.CurrentScene.Characters)
                                     .Select(c => c.Name)
                                     .ToList();
            if (characters.Any())
                IO.Write("Characters: " +
                             characters.Aggregate((str, cName) => $"{str}, {cName}"));
            // todo look around items etc
        }

        public static void PrintCharacter(Character c)
        {
            IO.Write(
                $"{c.Name}: {c.LookText}\n" +
                $"- Attack: {c.Attack}\n" +
                $"- Evade: {c.Evade}\n" +
                $"- Health: {c.Health}\n" +
                $"- Speed: {c.Speed}\n" +
                $"- Items: {c.Items}"
            );
        }

        public static void PrintInventory()
        {
            var items =
                from ig in AppState.Game.PlayerChar.Items
                select (
                    Element: AppState.Project.TopLevelElements.First(tle => tle.Id == ig.ItemId),
                    ig.Count
                );
            string text = items.Aggregate("Current Inventory:", (s, tuple) =>
                $"{s}\n- {tuple.Element.Name}"
                + (tuple.Count != 1
                    ? $" [{tuple.Count}]"
                    : "")
            );

            IO.Write(text);
        }
    }
}