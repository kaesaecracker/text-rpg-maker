using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [LoadFromProjectFile("dialogs.yaml", true, true)]
    public class DialogFileModel : ElementFileModel
    {
        public override string Name => this.Id;

        [YamlMember(Alias = "text")]
        [YamlProperties(true)]
        public string Text { get; set; }

        [YamlMember(Alias = "goto")]
        public string GotoId { get; set; }

        [YamlMember(Alias = "choices")]
        public List<ChoiceFileModel> Choices { get; set; }
    }

    [DocumentedType]
    public class ChoiceFileModel
    {
        [YamlMember(Alias = "text")]
        [YamlProperties(true)]
        public string Text { get; set; }

        [YamlMember(Alias = "goto-dialog")]
        public string GotoDialogId { get; set; }

        [YamlMember(Alias = "goto-scene")]
        public string GotoSceneId { get; set; }

        [YamlMember(Alias = "reward-items")]
        public List<ItemGroupingFileModel> RewardItems { get; set; } = new List<ItemGroupingFileModel>();

        [YamlMember(Alias = "required-items")]
        public List<ItemGroupingFileModel> RequiredItems { get; set; } = new List<ItemGroupingFileModel>();
    }
}