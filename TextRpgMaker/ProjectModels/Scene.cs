using System.Collections.Generic;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;
using static TextRpgMaker.AppState;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents the scenes.yaml
    /// </summary>
    [LoadFromProjectFile("scenes.yaml", true, true)]
    public class Scene : BasicElement
    {
        [YamlMember(Alias = "characters")]
        public List<string> Characters { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemGrouping> Items { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "connections-to")]
        public List<string> Connections { get; set; } = new List<string>();

        public void Handle()
        {
            Game.CurrentScene = this;
            // todo handle scene
        }
    }
}