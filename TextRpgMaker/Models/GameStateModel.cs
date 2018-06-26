﻿using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    /// <summary>
    ///     The class containing everything about the current game (not the project), so for example the
    ///     players character with current health, the inventory etc.
    /// </summary>
    [DocumentedType]
    public class GameState
    {
        [YamlMember(Alias = "player-char")]
        public Character PlayerChar { get; set; }

        [YamlMember(Alias = "current-scene")]
        public Scene CurrentScene { get; set; }

        [YamlMember(Alias = "current-dialog")]
        public Dialog CurrentDialog { get; set; }
    }
}