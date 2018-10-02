using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents an weapon entry in YAML files
    /// </summary>
    [LoadFromProjectFile("items/weapons.yaml", false, true)]
    public class Weapon : LookableElement
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