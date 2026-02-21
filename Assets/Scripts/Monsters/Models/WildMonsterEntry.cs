using System;
using MonsterTamer.Monsters.Definitions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Monsters.Models
{
    /// <summary>
    /// Represents a wild Monster that can appear in a specific area,
    /// including its level range and encounter probability.
    /// </summary>
    [Serializable]
    internal struct WildMonsterEntry
    {
        [SerializeField, Required]
        [Tooltip("The Monster species definition for this wild encounter.")]
        private MonsterDefinition definition;

        [SerializeField, Range(1, 100)]
        [Tooltip("The minimum level this wild Monster can appear at.")]
        private int minLevel;

        [SerializeField, Range(1, 100)]
        [Tooltip("The maximum level this wild Monster can appear at.")]
        private int maxLevel;

        [SerializeField, Range(1, 100)]
        [Tooltip("The encounter rate (percentage) for this Monster in the wild.")]
        private int encounterRate;

        internal readonly MonsterDefinition Definition => definition;
        internal readonly int MinLevel => minLevel;
        internal readonly int MaxLevel => maxLevel;
        internal readonly int EncounterRate => encounterRate;
    }
}
