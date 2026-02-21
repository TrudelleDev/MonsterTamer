using System;
using MonsterTamer.Battle.Animations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.Models
{
    /// <summary>
    /// References core battle systems: animations and audio.
    /// </summary>
    [Serializable]
    internal struct BattleComponents
    {
        [SerializeField, Required] private BattleAnimation animation;
        [SerializeField, Required] private BattleAudio audio;

        internal readonly BattleAnimation Animation => animation;
        internal readonly BattleAudio Audio => audio;
    }
}
