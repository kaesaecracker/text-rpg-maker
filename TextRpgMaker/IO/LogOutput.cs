using System.Collections.Generic;
using TextRpgMaker.Models;
using static Serilog.Log;

namespace TextRpgMaker.IO
{
    public class LogOutput : IOutput
    {
        public void Write(string text)
        {
            Logger.Information("GAME: {text}", text);
        }

        public void Write(List<Choice> choices)
        {
            for (int index = 0; index < choices.Count; index++)
            {
                var c = choices[index];
                Logger.Information(
                    "GAME: {index}. {text} | Requires: {required} | Rewards: {reward}",
                    index + 1, c.Text, c.RequiredItems, c.RewardItems
                );
            }
        }
    }
}