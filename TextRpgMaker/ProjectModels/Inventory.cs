using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
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
            
            var names = this.Select(ig => AppState.Project.ById<BasicElement>(ig.ItemId).Name);
            return $"[{names.Aggregate((curr, name) => $"{curr}, {name}")}]";
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

        [YamlMember(Alias = "id")]
        [YamlProperties(true)]
        public string ItemId { get; set; }

        [YamlMember(Alias = "count")]
        [YamlProperties(false, 1)]
        public uint Count { get; set; }
    }
}