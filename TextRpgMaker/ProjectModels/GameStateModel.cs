using Serilog;
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
        private string _playerCharId;
        private string _currentSceneId;
        private string _currentDialogId;

        [YamlMember(Alias = "player-char")]
        public string PlayerCharId
        {
            get => this._playerCharId;
            set
            {
                this._playerCharId = value;
                Log.Verbose("Switched {var} to {val}", nameof(this.PlayerCharId), value);
            }
        }

        [YamlMember(Alias = "current-scene")]
        public string CurrentSceneId
        {
            get => this._currentSceneId;
            set
            {
                this._currentSceneId = value;
                Log.Verbose("Switched {var} to {val}", nameof(this.CurrentSceneId), value);
            }
        }

        [YamlMember(Alias = "current-dialog")]
        public string CurrentDialogId
        {
            get => this._currentDialogId;
            set
            {
                this._currentDialogId = value;
                Log.Verbose("Switched {var} to {val}", nameof(this.CurrentDialogId), value);
            }
        }

        [YamlIgnore]
        public Character PlayerChar
        {
            get => AppState.Project.ById<Character>(this.PlayerCharId);
            set => this.PlayerCharId = value.Id;
        }

        [YamlIgnore]
        public Scene CurrentScene
        {
            get => AppState.Project.ById<Scene>(this.CurrentSceneId);
            set => this.CurrentSceneId = value.Id;
        }

        [YamlIgnore]
        public Dialog CurrentDialog
        {
            get => AppState.Project.ById<Dialog>(this.CurrentDialogId);
            set => this.CurrentDialogId = value.Id;
        }
    }
}