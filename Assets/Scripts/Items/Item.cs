using System;
using MonsterTamer.Items.Definitions;
using MonsterTamer.Items.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Items
{
    /// <summary>
    /// Represents a runtime item stack in the player's inventory or world.
    /// </summary>
    [Serializable]
    internal class Item
    {
        [SerializeField, Required] private ItemDefinition definition;

        [SerializeField, Required, Range(1, 99)]
        private int quantity = 1;

        internal ItemDefinition Definition => definition;

        internal ItemId ID => definition != null ? definition.ItemId : ItemId.None;

        internal int Quantity
        {
            get => quantity;
            set => quantity = Mathf.Max(0, value);
        }

        internal Item(ItemDefinition definition, int quantity = 1)
        {
            this.definition = definition;
            this.quantity = Mathf.Max(0, quantity);
        }
    }
}
