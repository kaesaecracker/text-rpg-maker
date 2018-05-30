using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class ProjectInfo
    {
        [YamlMember(Alias = "title")]
        public string Title { get; set; }
        
        [YamlMember(Alias = "description")]
        public string Description { get; set; }
        
        [YamlMember(Alias = "start-info")]
        public StartInfoContainer StartInfo { get; set; }

        public class StartInfoContainer
        {
            [YamlMember(Alias = "scene")]
            public string SceneId { get; set; }
            
            [YamlMember(Alias = "dialog")]
            public string DialogId { get; set; }
        }
    }
    
}