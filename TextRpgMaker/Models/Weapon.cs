using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    [LoadFromProjectFile("items/weapons.yaml", false, true)]
    public class Weapon : BasicElement
    {
        [YamlMember(Alias = "name")]
        [YamlProperties(true)]
        public override string Name { get; set; }

        [YamlMember(Alias = "attack")]
        [YamlProperties(false, 1.0)]
        public double Attack { get; set; }

        [YamlMember(Alias = "timeout")]
        [YamlProperties(false, 1.0)]
        public double Timeout { get; set; }

        [YamlMember(Alias = "ammo")]
        [YamlProperties(false)]
        public string AmmoId { get; set; }
    }
}