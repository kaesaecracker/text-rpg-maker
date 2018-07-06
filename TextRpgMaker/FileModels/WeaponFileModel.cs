using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [LoadFromProjectFile("items/weapons.yaml", false, true)]
    public class WeaponFileModel : ElementFileModel
    {
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