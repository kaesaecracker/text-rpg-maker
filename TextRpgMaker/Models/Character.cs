using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class Character : Element
    {
        [YamlMember(Alias = "current-hp")]
        public double CurrentHp { get; set; } = 100;

        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "attack")]
        public double Health { get; set; } = 1;

        [YamlMember(Alias = "evade")]
        public double Evade { get; set; } = 1;

        [YamlMember(Alias = "attack")]
        public double Attack { get; set; } = 1;

        [YamlMember(Alias = "speed")]
        public double Speed { get; set; } = 1;
    }
}