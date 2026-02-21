using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Types
{
    /// <summary>
    /// Groups target types by their damage effectiveness categories for offensive calculations.
    /// </summary>
    [Serializable]
    internal struct TypeEffectivenessGroups
    {
        [SerializeField, Required]
        [Tooltip("Types this type deals 2.0x damage to.")]
        private List<TypeDefinition> superEffectiveAgainst;

        [SerializeField, Required]
        [Tooltip("Types this type deals 0.5x damage to.")]
        private List<TypeDefinition> notEffectiveAgainst;

        [SerializeField, Required]
        [Tooltip("Types this type deals 0x damage to.")]
        private List<TypeDefinition> immuneAgainst;

        /// <summary>
        /// Determines the effectiveness of this type when attacking a specific target type.
        /// </summary>
        public readonly TypeEffectiveness GetEffectiveness(TypeDefinition targetType)
        {
            if (targetType == null)
            {
                return TypeEffectiveness.Normal;
            }
            if (superEffectiveAgainst?.Contains(targetType) == true)
            {
                return TypeEffectiveness.SuperEffective;
            }
            if (notEffectiveAgainst?.Contains(targetType) == true)
            {
                return TypeEffectiveness.NotVeryEffective;
            }
            if (immuneAgainst?.Contains(targetType) == true)
            {
                return TypeEffectiveness.Immune;
            }
             
            return TypeEffectiveness.Normal;
        }
    }
}