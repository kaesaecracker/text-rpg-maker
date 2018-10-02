using System.Collections.Generic;
using System.Linq;
using Serilog;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.ProjectModels
{
    [LoadFromProjectFile("dialogs.yaml", true, true)]
    public class Dialog : BasicElement
    {
        public override string Name => this.Id;

        [YamlMember(Alias = "text")]
        [YamlProperties(true)]
        public string Text { get; set; }

        [YamlMember(Alias = "goto-dialog")]
        public string GotoId { get; set; }

        [YamlMember(Alias = "choices")]
        public List<Choice> Choices { get; set; } = new List<Choice>();

        /// <summary>
        /// Start the dialog.
        /// </summary>
        public void Start()
        {
            Game.CurrentDialog = this;

            IO.Write('"' + this.Text + '"');
            if (this.GotoId != null)
            {
                Project.ById<Dialog>(this.GotoId).Start();
                return;
            }

            // find available choices
            var choicesThatMeetRequirements = (
                from c in this.Choices
                // where the player has the cost items
                where !(
                    from costItem in c.CostItems
                    where !Game.PlayerChar.Items.HasItem(costItem)
                    select costItem
                ).Any()
                // and the required items
                where !(
                    from reqItem in c.RequiredItems
                    where !Game.PlayerChar.Items.HasItem(reqItem)
                    select reqItem
                ).Any()
                select c
            ).ToList();

            Log.Debug(
                "Choices in dialog: {dlgChoices}, Choices that meet requirements {matchingChoices}",
                this.Choices.Count, choicesThatMeetRequirements.Count
            );

            // no choice available
            if (choicesThatMeetRequirements.Count == 0)
            {
                Log.Debug("No choices match");
                IO.GetTextInput();
                return;
            }

            IO.GetChoice(choicesThatMeetRequirements,
                c => $"{c.Text}{c.CostItemsText}{c.RequiredItemsText}{c.RewardItemsText}", choice =>
                {
                    Log.Debug("Chosen a choice: {text}", choice.Text);
                    choice.Handle();
                }
            );
        }
    }
}