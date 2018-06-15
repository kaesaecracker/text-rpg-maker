using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("dialogs.yaml", required: true, isList: true)]
    public class Dialog : Element
    {
        public override string Name => this.Id;

        [YamlMember(Alias = "text")]
        [YamlProperties(required: true)]
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
        [YamlProperties(required: true)]
        public string Text { get; set; }

        [YamlMember(Alias = "goto")]
        public string GotoId { get; set; }

        [YamlMember(Alias = "reward-items")]
        public List<ItemGrouping> RewardItemIds { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "required-items")]
        public List<ItemGrouping> RequiredItemIds { get; set; } = new List<ItemGrouping>();
    }
}