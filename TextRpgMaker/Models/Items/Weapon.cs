using YamlDotNet.Serialization;

namespace TextRpgMaker.Models.Items
{
    public class Weapon : Element
    {
        [YamlMember(Alias = "attack")]
        public double Attack { get; set; }
        
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
        
        [YamlMember(Alias = "timeout")]
        public double Timeout { get; set; }
        
        [YamlMember(Alias = "ammo")]
        public string AmmoId { get; set; }
    }
}