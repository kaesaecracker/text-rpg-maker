using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

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
        public List<Choice> Choices { get; set; }
    }

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
    }
}