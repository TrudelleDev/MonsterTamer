using System;
using MonsterTamer.Moves.Definitions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Moves.Models
{
    /// <summary>
    /// Represents a move a Monster learns at a specific level.
    /// </summary>
    [Serializable]
    internal struct LevelUpMove
    {
        [SerializeField, Range(1, 100)] private int level;
        [SerializeField, Required] private MoveDefinition moveDefinition;

        internal readonly int Level => level;
        internal readonly MoveDefinition MoveDefinition => moveDefinition;
    }
}
