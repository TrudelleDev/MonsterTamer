using System.Collections.Generic;
using MonsterTamer.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Inventory.Definitions
{
    /// <summary>
    /// Defines the starting items for an inventory template. 
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Inventory/Definition")]
    internal sealed class InventoryDefinition : ScriptableObject
    {
        [SerializeField, Required] private List<Item> items;

        internal IReadOnlyList<Item> Items => items;
    }
}
