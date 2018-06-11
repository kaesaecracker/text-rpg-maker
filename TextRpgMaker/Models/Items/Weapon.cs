using YamlDotNet.Serialization;

namespace TextRpgMaker.Models.Items
{
    [LoadFromProjectFile("items/weapons.yaml", required: false, isList: true)]
    public class Weapon : Element
    {
        [YamlMember(Alias = "name")]
        [YamlProperties(required: true)]
        public string Name { get; set; }

        [YamlMember(Alias = "attack")]
        [YamlProperties(required: false, defaultValue: (double) 1.0)]
        public double? Attack { get; set; }

        [YamlMember(Alias = "timeout")]
        [YamlProperties(required: false, defaultValue: (double) 1.0)]
        public double? Timeout { get; set; }

        [YamlMember(Alias = "ammo")]
        [YamlProperties(required: false)]
        public string AmmoId { get; set; }
    }
}