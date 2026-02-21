using System;
using System.Collections.Generic;
using MonsterTamer.Inventory.Definitions;
using MonsterTamer.Items;
using MonsterTamer.Items.Enums;
using MonsterTamer.Utilities;

namespace MonsterTamer.Inventory
{
    /// <summary>
    /// Manages a single inventory for a player or character.
    /// </summary>
    internal sealed class InventoryManager
    {
        private const int MaxQuantity = 99;
        private readonly List<Item> items = new();

        internal IReadOnlyList<Item> Items => items;
        internal InventoryDefinition InventoryDefinition { get; private set; }

        internal event Action ItemsChanged;

        internal InventoryManager(InventoryDefinition definition)
        {
            InventoryDefinition = definition;
            Initialize();
        }

        /// <summary>
        /// Loads items from the inventory definition.
        /// </summary>
        internal void Initialize()
        {
            Clear();

            if (InventoryDefinition == null || InventoryDefinition.Items == null)
            {
                Log.Warning(nameof(InventoryManager), "InventoryDefinition is missing or empty.");
                return;
            }

            foreach (Item item in InventoryDefinition.Items)
            {
                if (IsValid(item))
                {
                    Add(new Item(item.Definition, item.Quantity));
                }
            }
        }

        /// <summary>
        /// Adds an item to the inventory. If the item already exists,
        /// increases its stack quantity up to a maximum of 99.
        /// </summary>
        internal void Add(Item item)
        {
            if (!IsValid(item))
            {
                return;
            }

            var existingItem = items.Find(i => i.Definition == item.Definition);

            if (existingItem != null)
            {
                existingItem.Quantity = Math.Min(MaxQuantity, existingItem.Quantity + item.Quantity);
            }
            else
            {
                items.Add(new Item(item.Definition, item.Quantity));
            }

            OnItemChanged();
        }

        /// <summary>
        /// Removes a single unit of the specified item from the inventory.
        /// Removes the item entirely if the quantity reaches zero.
        /// </summary>
        internal void Remove(Item item)
        {
            if (!IsValid(item)) return;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == item.ID)
                {
                    items[i].Quantity--;

                    if (items[i].Quantity <= 0)
                    {
                        items.RemoveAt(i);
                    }

                    OnItemChanged();
                    return;
                }
            }
        }

        internal void Clear()
        {
            items.Clear();
            NotifyChanged();
        }

        private bool IsValid(Item item) => item != null && item.ID != ItemId.None;
        private void OnItemChanged() => NotifyChanged();
        private void NotifyChanged() => ItemsChanged?.Invoke();
    }
}
