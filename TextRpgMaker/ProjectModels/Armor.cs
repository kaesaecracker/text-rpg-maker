using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    /// <summary>
    /// Represents an armor entry in YAML files
    /// </summary>
    [LoadFromProjectFile("items/armor.yaml", false, true)]
    public class Armor : BasicElement
    {
        public enum ArmorSlot
        {
            /* Workaround for YamlDotNet issue: enum values do not get serialized of the int value is 0
             * see https://github.com/aaubry/YamlDotNet/issues/251 */
            Chest = 1,
            Legs = 2,
            Arms = 3,
            Head = 4
        }

        [YamlMember(Alias = "slot")]
        [YamlProperties(true)]
        public ArmorSlot Slot { get; set; }

        [YamlMember(Alias = "defense")]
        [YamlProperties(true)]
        public double Defense { get; set; }
    }
}