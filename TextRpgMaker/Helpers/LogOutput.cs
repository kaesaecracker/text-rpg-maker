using System.Collections.Generic;
using Serilog;
using TextRpgMaker.Models;

namespace TextRpgMaker.Helpers
{
    public class LogOutput : IOutput
    {
        public void Write(string text)
        {
            Log.Logger.Information("GAME: {text}", text);
        }

        public void Write(List<Choice> choices)
        {
            for (int index = 0; index < choices.Count; index++)
            {
                var c = choices[index];
                Log.Logger.Information(
                    "GAME: {index}. {text} | Requires: {required} | Rewards: {reward}",
                    index + 1, c.Text, c.RequiredItems, c.RewardItems
                );
            }
        }
    }
}