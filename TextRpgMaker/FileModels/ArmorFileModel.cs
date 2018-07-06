using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.FileModels
{
    [LoadFromProjectFile("items/armor.yaml", false, true)]
    public class ArmorFileModel : ElementFileModel
    {
        [YamlMember(Alias = "slot")]
        [YamlProperties(true)]
        public string Slot { get; set; }

        [YamlMember(Alias = "defense")]
        [YamlProperties(true)]
        public double? Defense { get; set; }
    }
}