using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class ItemGrouping
    {
        public ItemGrouping()
        {
            // empty contructor for serialization
        }

        public ItemGrouping(string itemId, int count)
        {
            this.ItemId = itemId;
            this.Count = count;
        }

        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(required: false, defaultValue: 1)]
        public int Count { get; set; }
    }
}