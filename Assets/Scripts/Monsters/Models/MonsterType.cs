using System;
using MonsterTamer.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Monsters.Models
{
    /// <summary>
    /// Represents a Monster's elemental typing.
    /// Contains a primary type and an optional secondary type.
    /// </summary>
    [Serializable]
    internal struct MonsterType
    {
        [SerializeField, Required, Tooltip("Primary type of the Monster.")]
        private TypeDefinition firstType;

        [SerializeField, Tooltip("Secondary elemental type (optional).")]
        private TypeDefinition secondType;

        internal readonly TypeDefinition FirstType => firstType;
        internal readonly TypeDefinition SecondType => secondType;
    }
}
