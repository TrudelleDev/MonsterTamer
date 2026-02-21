using MonsterTamer.Items.Enums;
using MonsterTamer.Items.Models;
using MonsterTamer.Monsters;
using MonsterTamer.Shared.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Items.Definitions
{
    /// <summary>
    /// Abstract base definition for all game items.
    /// </summary>
    [DisallowMultipleComponent]
    internal abstract class ItemDefinition : ScriptableObject, IDisplayable
    {
        protected const string FailMessage = "But it failed...";
        protected const string NoEffectMessage = "It won't have any effect.";

        [SerializeField] private ItemId id;
        [SerializeField, Required] private string displayName;
        [SerializeField, Required] private Sprite icon;

        [SerializeField, Required, TextArea(5, 10)] 
        private string description;

        /// <summary>
        /// Applies the item's effect to the target.
        /// </summary>
        internal abstract ItemUseResult Use(Monster target);

        internal ItemId ItemId => id;
        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
    }
}
