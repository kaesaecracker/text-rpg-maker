using TextRpgMaker.FileModels;
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
        public CharacterFileModel PlayerChar { get; set; }

        [YamlMember(Alias = "current-scene")]
        public SceneFileModel CurrentSceneFileModel { get; set; }

        [YamlMember(Alias = "current-dialog")]
        public DialogFileModel CurrentDialogFileModel { get; set; }
    }
}