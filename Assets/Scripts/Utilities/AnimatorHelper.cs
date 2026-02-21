using System.Collections;
using UnityEngine;

namespace MonsterTamer.Utilities
{
    /// <summary>
    /// A high-level wrapper for common Animator tasks, streamlining state transitions and cleanup.
    /// </summary>
    internal static class AnimatorHelper
    {
        /// <summary>
        /// Plays a state and waits for it to finish, including a safety frame for transition.
        /// </summary>
        internal static IEnumerator PlayAndWait(Animator animator, int state)
        {
            if (animator == null) yield break;

            animator.Play(state, 0, 0f);
            yield return null;
            yield return AnimationUtility.WaitForAnimationSafe(animator, state);
        }

        /// <summary>
        /// Resets multiple animators to their default bind pose.
        /// </summary>
        internal static void RebindAll(params Animator[] animators)
        {
            if (animators == null) return;

            foreach (var animator in animators)
            {
                animator.Rebind();
            }
        }
    }
}