using System.Collections.Generic;
using System.Linq;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    [DocumentedType]
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
        [YamlProperties(true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(false, 1)]
        public int Count { get; set; }
    }
}