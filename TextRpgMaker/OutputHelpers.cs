using System.Linq;
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
    }
}