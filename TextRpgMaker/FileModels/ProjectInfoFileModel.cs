using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [LoadFromProjectFile("project-info.yaml", true)]
    public class ProjectInfoFileModel : ElementFileModel
    {
        public override string Id { get; set; } = "project-info";

        [YamlMember(Alias = "title")]
        [YamlProperties(true)]
        public string Title { get; set; }

        [YamlMember(Alias = "description")]
        [YamlProperties(false, "")]
        public string Description { get; set; }

        [YamlMember(Alias = "start-text")]
        public string IntroText { get; set; }

        [YamlMember(Alias = "start-scene")]
        [YamlProperties(true)]
        public string SceneId { get; set; }

        [YamlMember(Alias = "start-dialog")]
        [YamlProperties(true)]
        public string DialogId { get; set; }

        [YamlMember(Alias = "start-characters")]
        [YamlProperties(true)]
        public List<string> CharacterIds { get; set; }
    }
}