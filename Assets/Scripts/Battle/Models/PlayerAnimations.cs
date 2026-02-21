using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.Models
{
    /// <summary>
    /// Contains references to all animators used for the player during battle.
    /// </summary>
    [Serializable]
    internal struct PlayerAnimations
    {
        [SerializeField, Required] private Animator monsterAnimator;
        [SerializeField, Required] private Animator trainerAnimator;
        [SerializeField, Required] private Animator hudAnimator;

        internal readonly Animator MonsterAnimator => monsterAnimator;
        internal readonly Animator TrainerSpriteAnimator => trainerAnimator;
        internal readonly Animator HudAnimator => hudAnimator;
    }
}
