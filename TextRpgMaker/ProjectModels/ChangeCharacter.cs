using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents an change-character entry in a dialog choice
    /// </summary>
    [DocumentedType]
    public class ChangeCharacter
    {
        // TODO validation that checks whether all those ids exist
        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public string TargetCharacter { get; set; }
        
        [YamlMember(Alias = "set-talk-dialog")]
        public string NewTalkDialog { get; set; }

        public void Apply()
        {
            var character = AppState.Project.Characters.GetId(this.TargetCharacter);

            if (!string.IsNullOrWhiteSpace(this.NewTalkDialog))
                character.TalkDialog = this.NewTalkDialog;
        }
    }
}