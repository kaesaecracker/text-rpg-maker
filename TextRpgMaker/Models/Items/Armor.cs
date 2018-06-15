using YamlDotNet.Serialization;

namespace TextRpgMaker.Models.Items
{
    [LoadFromProjectFile("items/armor.yaml", required: false, isList: true)]
    public class Armor : Element
    {
        [YamlMember(Alias = "slot")]
        [YamlProperties(required: true)]
        public ArmorSlot Slot { get; set; }

        [YamlMember(Alias = "defense")]
        [YamlProperties(required: true)]
        public double Defense { get; set; }

        public enum ArmorSlot
        {
            /* Workaround for YamlDotNet issue: enum values do not get serialized of the int value is 0
             * see https://github.com/aaubry/YamlDotNet/issues/251 */
            Chest = 1,
            Legs = 2,
            Arms = 3,
            Head = 4
        }
    }
}