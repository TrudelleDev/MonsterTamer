using System;
using MonsterTamer.Monsters.Enums;
using UnityEngine;

namespace MonsterTamer.Natures.Models
{
    /// <summary>
    /// Defines the stat-shifting multipliers for a specific monster nature.
    /// </summary>
    [Serializable]
    internal struct NatureStatsModifier
    {
        private const float IncreaseMultiplier = 1.1f;
        private const float DecreaseMultiplier = 0.9f;
        private const float NeutralMultiplier = 1.0f;

        [SerializeField, Tooltip("The stat that receives a 10% bonus.")]
        private MonsterStat increase;

        [SerializeField, Tooltip("The stat that receives a 10% penalty.")]
        private MonsterStat decrease;

        internal readonly float GetMultiplier(MonsterStat stat)
        {
            if (stat == MonsterStat.None)
            {
                return NeutralMultiplier;
            }

            if (stat == increase)
            {
                return IncreaseMultiplier;
            }

            if (stat == decrease)
            {
                return DecreaseMultiplier;
            }

            return NeutralMultiplier;
        }
    }
}