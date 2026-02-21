using System;
using MonsterTamer.Monsters.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Monsters.Models
{
    /// <summary>
    /// Represents a monster's core stats: HP, Attack, Defense,
    /// Special Attack, Special Defense, and Speed.
    /// </summary>
    [Serializable]
    internal struct MonsterStats
    {
        [SerializeField, Tooltip("Hit Points (HP) stat value.")]
        private int healthPoint;

        [SerializeField, Tooltip("Attack stat value.")]
        private int attack;

        [SerializeField, Tooltip("Defense stat value.")]
        private int defense;

        [SerializeField, Tooltip("Special Attack stat value.")]
        private int specialAttack;

        [SerializeField, Tooltip("Special Defense stat value.")]
        private int specialDefense;

        [SerializeField, Tooltip("Speed stat value.")]
        private int speed;

        [ShowInInspector, ReadOnly, Tooltip("Sum of all stat values.")]
        internal readonly int Total => healthPoint + attack + defense + specialAttack + specialDefense + speed;

        internal readonly int HealthPoint => healthPoint;
        internal readonly int Attack => attack;
        internal readonly int Defense => defense;
        internal readonly int SpecialAttack => specialAttack;
        internal readonly int SpecialDefense => specialDefense;
        internal readonly int Speed => speed;

        internal MonsterStats(int healthPoint, int attack, int defense, int specialAttack, int specialDefense, int speed)
        {
            this.healthPoint = healthPoint;
            this.attack = attack;
            this.defense = defense;
            this.specialAttack = specialAttack;
            this.specialDefense = specialDefense;
            this.speed = speed;
        }
    }
}
