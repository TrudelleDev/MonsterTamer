using UnityEngine;

namespace MonsterTamer.Raycasting
{
    /// <summary>
    /// Provides helper methods for 2D raycasting and interacting with components.
    /// </summary>
    internal static class RaycastUtility
    {
        /// <summary>
        /// Performs a raycast in a direction and invokes the callback for each component of type T found.
        /// Returns true if the callback signals to stop early or if any component was triggered.
        /// </summary>
        internal static bool RaycastAndCall<T>(Vector2 direction, RaycastSettings raycastSettings, Transform origin, System.Func<T, bool> callback)
        {
            if (direction == Vector2.zero) return false;

            Vector3 originPosition = origin.position + new Vector3(0, raycastSettings.RaycastOffset.y, 0);
            RaycastHit2D[] hitBuffer = new RaycastHit2D[5];

            int hitCount = Physics2D.RaycastNonAlloc(
                originPosition,
                direction,
                hitBuffer,
                raycastSettings.RaycastDistance,
                raycastSettings.InteractionMask
            );

            bool anyTriggered = false;

            for (int i = 0; i < hitCount; i++)
            {
                Collider2D collider = hitBuffer[i].collider;
                if (collider == null) continue;

                foreach (T comp in collider.GetComponents<T>())
                {
                    anyTriggered = true;
                    if (callback(comp))
                        return true; // stop if callback signals
                }
            }

            return anyTriggered;
        }
    }
}
