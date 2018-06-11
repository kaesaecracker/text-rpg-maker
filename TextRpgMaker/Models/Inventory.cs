using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class Inventory
    {
        [YamlMember(Alias = "items")]
        public List<ItemIdWithCount> Items { get; private set; }
    }

    public class ItemIdWithCount
    {
        public ItemIdWithCount()
        {
            // empty contructor for serialization
        }

        public ItemIdWithCount(string itemId, int count)
        {
            this.ItemId = itemId;
            this.Count = count;
        }

        [YamlMember(Alias = "id")]
        [YamlProperties(required: true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(required: false, defaultValue: (int) 1)]
        public int Count { get; set; }
    }
}