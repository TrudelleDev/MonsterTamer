using System.Collections;
using UnityEngine;

namespace MonsterTamer.Utilities
{
    /// <summary>
    /// Provides fail-safe methods for synchronizing logic with the Unity Animator system.
    /// </summary>
    internal static class AnimationUtility
    {
        internal static IEnumerator WaitForAnimationSafe(Animator animator, int stateHash, float fallback = 3f)
        {
            if (animator == null) yield break;

            float timer = 0f;

            // Wait until animator enters the state OR timeout
            while (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != stateHash && timer < fallback)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            // Wait until animation finishes OR timeout
            timer = 0f;
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            while (info.shortNameHash == stateHash && info.normalizedTime < 1f && timer < fallback)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
                info = animator.GetCurrentAnimatorStateInfo(0);
            }
        }
    }
}
