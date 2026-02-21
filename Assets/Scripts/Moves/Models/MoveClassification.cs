using System;
using MonsterTamer.Moves.Enums;
using MonsterTamer.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Moves.Models
{
    /// <summary>
    /// Defines how a move behaves in combat by specifying its elemental type
    /// </summary>
    [Serializable]
    internal struct MoveClassification
    {
        [SerializeField, Required] private TypeDefinition typeDefinition;
        [SerializeField] private MoveCategory category;

        internal readonly TypeDefinition TypeDefinition => typeDefinition;
        internal readonly MoveCategory Category => category;
    }
}
