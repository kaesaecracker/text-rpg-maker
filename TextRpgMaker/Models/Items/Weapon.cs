using YamlDotNet.Serialization;

namespace TextRpgMaker.Models.Items
{
    public class Weapon : Element
    {
        [YamlMember(Alias = "attack")]
        public double Attack { get; set; }
    }
}