using System.Collections.Generic;
using System.Linq;
using Serilog;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents an choice entry in a dialog
    /// </summary>
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

        [YamlMember(Alias = "cost-items")]
        public List<ItemGrouping> CostItems { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "change-scenes")]
        public List<ChangeScene> ChangeScenes { get; set; } = new List<ChangeScene>();

        [YamlMember(Alias = "change-characters")]
        public List<ChangeCharacter> ChangeCharacters { get; set; } = new List<ChangeCharacter>();

        public void Handle()
        {
            IO.Write($" >> {this.Text}");

            Log.Debug(
                "Handle Choice {text}, GotoDialog={dlgId}, GotoScene={sceneId}",
                this.Text, this.GotoDialogId, this.GotoSceneId
            );

            // remove required items
            foreach (var requiredItem in this.CostItems)
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
            {
                Log.Debug("Goto dialog {dlg}", this.GotoDialogId);
                Project.Dialogs.GetId(this.GotoDialogId).Start();
                return;
            }

            if (this.GotoSceneId != null)
            {
                Log.Debug("Goto scene {id}", this.GotoSceneId);
                Project.Scenes.GetId(this.GotoSceneId).Handle();
                return;
            }

            Log.Debug("No GotoDialog or GotoScene -> free text input");
            IO.GetTextInput();
        }

        public string CostItemsText => GetItemListText("Costs", this.CostItems);

        public string RequiredItemsText => GetItemListText("Required", this.RequiredItems);

        public string RewardItemsText => GetItemListText("Rewards", this.RewardItems);

        private static string GetItemListText(string seed, IReadOnlyCollection<ItemGrouping> list)
        {
            // return nothing if list is empty
            if (!list.Any()) return string.Empty;

            // make comma separated list
            return $"\n{seed}: " + string.Join(", ", list.Select(ig =>
            {
                // don't show count if only 1
                string count = ig.Count < 2 ? string.Empty : $" [{ig.Count}]";
                // for every element, show item name and created count string
                return $"{Project.ById<BasicElement>(ig.ItemId).Name}{count}";
            }));
        }
    }
}