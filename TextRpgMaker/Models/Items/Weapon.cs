using YamlDotNet.Serialization;

namespace TextRpgMaker.Models.Items
{
    public class Weapon : Element
    {
        [YamlMember(Alias = "attack")]
        public double? Attack { get; set; } = null;

        [YamlMember(Alias = "name")]
        public string Name { get; set; } = null;

        [YamlMember(Alias = "timeout")]
        public double? Timeout { get; set; } = null;

        [YamlMember(Alias = "ammo")]
        public string AmmoId { get; set; } = null;
    }
}