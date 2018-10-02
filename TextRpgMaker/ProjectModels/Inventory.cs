using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TextRpgMaker.Helpers;
using YamlDotNet.Serialization;

namespace TextRpgMaker.ProjectModels
{
    [DocumentedType]
    // TODO make List<> invisible from outside (probably big refactor)
    public class Inventory : List<ItemGrouping>
    {
        public override string ToString()
        {
            if (this.Count == 0)
            {
                return "[(empty)]";
            }

            var builder = new StringBuilder("[");
            for (int i = 0; i < this.Count; i++)
            {
                var ig = this[i];

                string name = AppState.Project.ById(ig.ItemId).Name;
                string count = this.Count < 2 ? string.Empty : $" [{ig.Count}]";

                builder.Append($"{name}{count}");

                if (i + 1 < this.Count)
                {
                    builder.Append(", ");
                }
            }

            builder.Append("]");
            return builder.ToString();
        }

        private ItemGrouping GetItemGrouping(string id)
        {
            foreach (var ig in this)
            {
                if (ig.ItemId == id) return ig;
            }

            return null;
        }

        /// <summary>
        /// Checks whether the inventory contains the item type, and whether it contains at least the amount specified
        /// </summary>
        public bool HasItem(ItemGrouping groupingToFind)
        {
            var ig = this.GetItemGrouping(groupingToFind.ItemId);
            return ig != null && ig.Count >= groupingToFind.Count;
        }

        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        public void AddItem(ItemGrouping item)
        {
            var ig = this.GetItemGrouping(item.ItemId);
            if (ig != null) ig.Count += item.Count;
            else this.Add(item);
        }

        /// <summary>
        /// Removes the specified item (count) from the inventory.
        /// </summary>
        /// <returns>false if inventory does not contain enough to remove</returns>
        public bool RemoveItem(ItemGrouping item)
        {
            if (!this.HasItem(item)) return false;

            var ig = this.GetItemGrouping(item.ItemId);
            ig.Count -= item.Count;

            if (ig.Count <= 0) this.Remove(item);
            return true;
        }
    }

    [DocumentedType]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification =
        "Is instantiated by YAML lib")]
    public class ItemGrouping
    {
        /// <summary>
        /// empty constructor for serialization
        /// </summary>
        public ItemGrouping()
        {
        }

        public ItemGrouping(string itemId, uint count)
        {
            this.ItemId = itemId;
            this.Count = count;
        }

        public override string ToString() => this.ToString(null);

        /// <summary>
        /// A user friendly text representation.
        /// </summary>
        /// <param name="name">The name part.</param>
        /// <returns>something like 'Coin [6]'</returns>
        public string ToString(string name)
        {
            string namePart = name ?? this.ItemId;
            string count = this.Count < 2 ? string.Empty : $" [{this.Count}]";

            return $"{namePart}{count}";
        }

        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        public uint Count { get; set; } = 1;
    }
}