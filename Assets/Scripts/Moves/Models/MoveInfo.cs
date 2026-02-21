using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Moves.Models
{
    /// <summary>
    /// Represents the numerical attributes of a Monster move,
    /// including base power, accuracy, and maximum uses (PP).
    /// </summary>
    [Serializable]
    internal struct MoveInfo
    {
        [SerializeField, Required] private int power;
        [SerializeField, Required] private int accuracy;
        [SerializeField, Required] private int powerPoint;

        internal readonly int Power => power;
        internal readonly int Accuracy => accuracy;
        internal readonly int PowerPoint => powerPoint;
    }
}
