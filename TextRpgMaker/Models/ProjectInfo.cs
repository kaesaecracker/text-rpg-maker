using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("project-info.yaml", required: true)]
    public class ProjectInfo : Element
    {
        public override string Id { get; set; } = "project-info";

        [YamlMember(Alias = "title")]
        [YamlProperties(required: true)]
        public string Title { get; set; }

        [YamlMember(Alias = "description")]
        [YamlProperties(required: false, defaultValue: "")]
        public string Description { get; set; }

        [YamlMember(Alias = "start-info")]
        [YamlProperties(required: true)]
        public StartInfoContainer StartInfo { get; set; }

        public class StartInfoContainer
        {
            [YamlMember(Alias = "scene")]
            [YamlProperties(required: true)]
            public string SceneId { get; set; }

            [YamlMember(Alias = "dialog")]
            [YamlProperties(required: true)]
            public string DialogId { get; set; }

            [YamlMember(Alias = "characters")]
            [YamlProperties(required: true)]
            public List<string> CharacterIds { get; set; }
        }
    }
}