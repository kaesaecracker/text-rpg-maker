using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("project-info.yaml", true)]
    public class ProjectInfo : Element
    {
        public override string Id { get; set; } = "project-info";

        [YamlMember(Alias = "title")]
        [YamlProperties(true)]
        public string Title { get; set; }

        [YamlMember(Alias = "description")]
        [YamlProperties(false, "")]
        public string Description { get; set; }

        [YamlMember(Alias = "start-info")]
        [YamlProperties(true)]
        public StartInfoContainer StartInfo { get; set; }

        [DocumentedType]
        public class StartInfoContainer
        {
            [YamlMember(Alias = "intro-text")]
            public string IntroText { get; set; }

            [YamlMember(Alias = "scene")]
            [YamlProperties(true)]
            public string SceneId { get; set; }

            [YamlMember(Alias = "dialog")]
            [YamlProperties(true)]
            public string DialogId { get; set; }

            [YamlMember(Alias = "characters")]
            [YamlProperties(true)]
            public List<string> CharacterIds { get; set; }
        }
    }
}