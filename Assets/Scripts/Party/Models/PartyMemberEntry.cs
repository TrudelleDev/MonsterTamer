using System;
using MonsterTamer.Monsters;
using MonsterTamer.Monsters.Definitions;
using UnityEngine;

namespace MonsterTamer.Party.Models
{
    /// <summary>
    /// Defines a data entry representing a monster and its level for party initialization.
    /// </summary>
    [Serializable]
    internal struct PartyMemberEntry
    {
        [SerializeField] private MonsterDefinition monsterDefinition;
        [SerializeField, Range(1, 100)] private int level;

        internal readonly MonsterDefinition MonsterDefinition => monsterDefinition;
        internal readonly int Level => level;
    }
}
