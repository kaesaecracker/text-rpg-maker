using YamlDotNet.Serialization;

namespace TextRpgMaker.Models.Items
{
    public class Weapon : Element
    {
        [YamlMember(Alias = "name")]
        [Required]
        public string Name { get; set; }

        [YamlMember(Alias = "attack")]
        [DefaultValue(1.0)]
        public double? Attack { get; set; }

        [YamlMember(Alias = "timeout")]
        [DefaultValue(1.0)]
        public double? Timeout { get; set; }

        [YamlMember(Alias = "ammo")]
        public string AmmoId { get; set; }
    }
}