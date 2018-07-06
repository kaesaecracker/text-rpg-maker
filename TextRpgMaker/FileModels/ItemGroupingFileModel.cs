using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [DocumentedType]
    public class ItemGroupingFileModel
    {
        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(false, 1)]
        public int Count { get; set; }
    }
}