using System.Collections.Generic;
using System.Linq;
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

        [YamlMember(Alias = "goto")]
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

            var choicesThatMeetRequirements = (
                from c in this.Choices
                let mismatchedItems = (
                    from reqItem in c.RequiredItems
                    where !Game.PlayerChar.Items.HasItem(reqItem)
                    select reqItem
                )
                where !mismatchedItems.Any()
                select c
            ).ToList();

            if (choicesThatMeetRequirements.Count == 0)
                IO.GetTextInput();
            else
                IO.GetChoice(choicesThatMeetRequirements, c => c.Text, choice =>
                {
                    IO.Write($" >> {choice.Text}");
                    choice.Handle();
                });

        }
    }
}