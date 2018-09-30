using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.ProjectModels
{
    [DocumentedType]
    public class Choice
    {
        [YamlMember(Alias = "text")]
        [YamlProperties(true)]
        public string Text { get; set; }

        [YamlMember(Alias = "goto-dialog")]
        public string GotoDialogId { get; set; }

        [YamlMember(Alias = "goto-scene")]
        public string GotoSceneId { get; set; }

        [YamlMember(Alias = "reward-items")]
        public List<ItemGrouping> RewardItems { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "required-items")]
        public List<ItemGrouping> RequiredItems { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "change-scenes")]
        public List<ChangeScene> ChangeScenes { get; set; } = new List<ChangeScene>();

        [YamlMember(Alias = "change-characters")]
        public List<ChangeCharacter> ChangeCharacters { get; set; } = new List<ChangeCharacter>();

        public void Handle()
        {
            // remove required items
            foreach (var requiredItem in this.RequiredItems)
                Game.PlayerChar.Items.RemoveItem(requiredItem);

            // give reward items
            foreach (var rewardItem in this.RewardItems)
                Game.PlayerChar.Items.AddItem(rewardItem);

            // apply scene changes
            foreach (var changeCharacter in this.ChangeCharacters)
                changeCharacter.Apply();

            // apply char changes
            foreach (var changeScene in this.ChangeScenes)
                changeScene.Apply();


            // Priority: GotoDialog, GotoScene. If none specified, wait for input
            if (this.GotoDialogId != null)
                Project.Dialogs.GetId(this.GotoDialogId).Start();
            else if (this.GotoSceneId != null)
                Project.Scenes.GetId(this.GotoSceneId).Handle();
            else IO.GetTextInput();
        }
    }
}