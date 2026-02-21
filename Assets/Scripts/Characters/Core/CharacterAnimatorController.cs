using MonsterTamer.Characters.Directions;
using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Handles character animations (idle, walk, collision, refacing) via Animator.
    /// </summary>
    internal sealed class CharacterAnimatorController
    {
        private readonly Animator animator;
        private bool walkFirstStep = true;
        private bool collisionFirstStep = true;

        internal CharacterAnimatorController(Animator animator) => this.animator = animator;

        internal void PlayIdle(FacingDirection direction) => UpdateDirection(direction);

        internal void UpdateDirection(FacingDirection direction)
        {
            Vector2Int dir = direction.ToVector2Int();
            animator.SetFloat(CharacterAnimatorParameters.Vertical, dir.y);
            animator.SetFloat(CharacterAnimatorParameters.Horizontal, dir.x);
        }

        internal void PlayWalkStep()
        {
            animator.SetTrigger(walkFirstStep
                ? CharacterAnimatorParameters.WalktStep1Trigger
                : CharacterAnimatorParameters.WalkStep2Trigger);

            walkFirstStep = !walkFirstStep;
        }

        internal void PlayCollisionStep()
        {
            animator.SetTrigger(collisionFirstStep
                ? CharacterAnimatorParameters.CollisionStep1Trigger
                : CharacterAnimatorParameters.CollisionStep2Trigger);

            collisionFirstStep = !collisionFirstStep;
        }

        internal void PlayRefacing(FacingDirection direction)
        {
            UpdateDirection(direction);
            animator.SetTrigger(CharacterAnimatorParameters.RefaceTrigger);
        }
    }
}
