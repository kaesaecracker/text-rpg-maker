using System;
using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class ItemIdWithCount
    {
        public ItemIdWithCount()
        {
        }

        public ItemIdWithCount(string itemId, int count)
        {
            this.ItemId = itemId;
            this.Count = count;
        }

        [YamlMember(Alias = "id")]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        public int Count { get; set; } = 1;
    }
}