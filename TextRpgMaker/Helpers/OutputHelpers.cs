using System.Linq;
using TextRpgMaker.ProjectModels;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.Helpers
{
    public static class OutputHelpers
    {
        /// <summary>
        /// Print elements in current scene
        /// </summary>
        public static void LookAround()
        {
            IO.Write(">> look around");

            // print characters if any
            var characters = Game.CurrentScene.Characters.Select(c => c.Name).ToList();
            if (characters.Any()) IO.Write("   Characters: " + string.Join(", ", characters));

            // print connections if any
            var connections = Game.CurrentScene.Connections.Select(s => s.Name).ToList();
            if (connections.Any()) IO.Write("   Connections: " + string.Join(", ", connections));

            // print items if any
            var items = Game.CurrentScene.Items
                            .Select(ig => ig.ToString(Project.ById(ig.ItemId).Name))
                            .ToList();
            if (items.Any()) IO.Write("   Items: " + string.Join(", ", items));

            // todo look around items etc
        }

        /// <summary>
        /// print info about a character
        /// </summary>
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

        /// <summary>
        /// Print your inventory
        /// </summary>
        public static void PrintInventory()
        {
            var items =
                from ig in Game.PlayerChar.Items
                select (
                    Element: Project.TopLevelElements.First(tle => tle.Id == ig.ItemId),
                    ig.Count
                );
            string text = items.Aggregate("Current Inventory:", (s, tuple) =>
                $"{s}\n- {tuple.Element.Name}"
                + (tuple.Count != 1
                    ? $" [{tuple.Count}]"
                    : "")
            );

            IO.Write(text);
            IO.Write("Type 'look <item name>'.");
        }
    }
}