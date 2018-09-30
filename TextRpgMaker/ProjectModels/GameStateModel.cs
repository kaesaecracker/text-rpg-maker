using System;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    ///     The class containing everything about the current game (not the project), so for example the
    ///     players character with current health, the inventory etc.
    /// </summary>
    [DocumentedType]
    public class GameState
    {
        [YamlMember(Alias = "player-char")]
        public string PlayerCharId { get; set; }

        [YamlMember(Alias = "current-scene")]
        public string CurrentSceneId { get; set; }

        [YamlMember(Alias = "current-dialog")]
        public string CurrentDialogId { get; set; }

        public Character PlayerChar
        {
            get => AppState.Project.ById<Character>(this.PlayerCharId);
            set => this.PlayerCharId = value.Id;
        }

        public Scene CurrentScene
        {
            get => AppState.Project.ById<Scene>(this.CurrentSceneId);
            set => this.CurrentSceneId = value.Id;
        }

        public Dialog CurrentDialog
        {
            get => AppState.Project.ById<Dialog>(this.CurrentDialogId);
            set => this.CurrentDialogId = value.Id;
        }
    }
}