using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.Models
{
    /// <summary>
    /// Contains references to all animators used for the opponent during battle.
    /// </summary>
    [Serializable]
    internal struct OpponentAnimations
    {
        [SerializeField, Required] private Animator monsterAnimator;
        [SerializeField, Required] private Animator trainerAnimator;
        [SerializeField, Required] private Animator hudAnimator;

        internal readonly Animator MonsterAnimator => monsterAnimator;
        internal readonly Animator TrainerAnimator => trainerAnimator;
        internal readonly Animator HudAnimator => hudAnimator;
    }
}
