using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [LoadFromProjectFile("scenes.yaml", true, true)]
    public class SceneFileModel : ElementFileModel
    {
        [YamlMember(Alias = "characters")]
        public List<string> Characters { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemGroupingFileModel> Items { get; set; }
    }
}