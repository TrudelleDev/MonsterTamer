using System;
using UnityEngine;

namespace MonsterTamer.Monsters.Models
{
    /// <summary>
    /// Settings for animating a monster's health bar.
    /// Controls the speed and tick delays during damage or healing.
    /// </summary>
    [Serializable]
    internal struct HealthBarAnimationSettings
    {
        [SerializeField, Tooltip("Speed factor for damage animation. Higher = slower.")]
        private float damageSpeedFactor;

        [SerializeField, Tooltip("Speed factor for healing animation. Higher = slower.")]
        private float healSpeedFactor;

        [SerializeField, Range(0f, 0.1f)]
        [Tooltip("Minimum tick delay (seconds) when animating health change.")]
        private float minTickDelay;

        [SerializeField, Range(0f, 0.1f)]
        [Tooltip("Maximum tick delay (seconds) when animating health change.")]
        private float maxTickDelay;

        internal readonly float DamageSpeedFactor => damageSpeedFactor;
        internal readonly float HealSpeedFactor => healSpeedFactor;
        internal readonly float MinTickDelay => minTickDelay;
        internal readonly float MaxTickDelay => maxTickDelay;
    }
}
