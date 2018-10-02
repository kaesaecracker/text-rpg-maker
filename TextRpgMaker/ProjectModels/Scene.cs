using System.Collections.Generic;
using System.Collections.Immutable;
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
        public List<string> CharacterIds { get; set; }

        [YamlMember(Alias = "items")]
        public List<ItemGrouping> Items { get; set; } = new List<ItemGrouping>();

        [YamlMember(Alias = "connections")]
        public List<string> ConnectionIds { get; set; } = new List<string>();

        [YamlIgnore]
        public ImmutableList<Character> Characters => this.CharacterIds != null
            ? Project.Characters.GetIds(this.CharacterIds).ToImmutableList()
            : new List<Character>().ToImmutableList();

        [YamlIgnore]
        public ImmutableList<Scene> Connections => this.ConnectionIds != null
            ? Project.Scenes.GetIds(this.ConnectionIds).ToImmutableList()
            : new List<Scene>().ToImmutableList();

        public void Handle()
        {
            Game.CurrentScene = this;
            IO.Write($">> You are now in {this.Name}. You look around and see:");
            OutputHelpers.LookAround();
        }
    }
}