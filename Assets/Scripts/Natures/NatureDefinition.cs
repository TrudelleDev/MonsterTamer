using MonsterTamer.Monsters.Enums;
using MonsterTamer.Natures.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Natures
{
    /// <summary>
    /// Defines a Monster's nature, which modifies specific stats by a fixed percentage.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Nature/Nature Definition")]
    internal sealed class NatureDefinition : ScriptableObject
    {
        [SerializeField, Required]
        [Tooltip("The name of the Nature as seen by the player (e.g., Jolly, Adamant).")]
        private string displayName;

        [SerializeField]
        [Tooltip("The specific stat changes associated with this Nature.")]
        private NatureStatsModifier modifiers;

        internal string DisplayName => displayName;

        internal float GetMultiplier(MonsterStat stat) => modifiers.GetMultiplier(stat);
    }
}