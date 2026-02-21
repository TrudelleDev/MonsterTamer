using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Contains static Animator paramaters hashes used by character state controller.
    /// </summary>
    internal static class CharacterAnimatorParameters
    {
        internal static readonly int Vertical = Animator.StringToHash("Vertical");
        internal static readonly int Horizontal = Animator.StringToHash("Horizontal");
        internal static readonly int RefaceTrigger = Animator.StringToHash("Reface");
        internal static readonly int WalktStep1Trigger = Animator.StringToHash("WalkFirstStep");
        internal static readonly int WalkStep2Trigger = Animator.StringToHash("WalkSecondStep");
        internal static readonly int CollisionStep1Trigger = Animator.StringToHash("CollideFirstStep");
        internal static readonly int CollisionStep2Trigger = Animator.StringToHash("CollideSecondStep");
    }
}
