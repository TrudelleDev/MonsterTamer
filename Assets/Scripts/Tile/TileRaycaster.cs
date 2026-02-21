using MonsterTamer.Characters.Directions;
using MonsterTamer.Raycasting;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Tile
{
    /// <summary>
    /// Provides directional raycasting functionality for obstacle detection.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class TileRaycaster : MonoBehaviour
    {
        [SerializeField, Required] private RaycastSettings raycastSettings;

        private FacingDirection lastDirection = FacingDirection.South;
        private bool lastHitStatus;

        internal bool IsPathClear(FacingDirection direction)
        {
            lastDirection = direction;

            // Casting to Vector2 handles the Z-axis strip automatically for 2D physics
            Vector2 origin = (Vector2)transform.position + raycastSettings.RaycastOffset;
            Vector2 rayDir = direction.ToVector2Int();

            RaycastHit2D hit = Physics2D.Raycast(
                origin,
                rayDir,
                raycastSettings.RaycastDistance,
                raycastSettings.InteractionMask
            );

            lastHitStatus = hit.collider != null;
            return !lastHitStatus;
        }

        private void OnDrawGizmosSelected()
        {
            if (raycastSettings == null) return;

            // Visual feedback: Green for clear, Red for blocked
            Gizmos.color = lastHitStatus ? Color.red : Color.green;

            Vector3 origin = transform.position + (Vector3)raycastSettings.RaycastOffset;
            Vector3 rayDir = (Vector3)(Vector2)lastDirection.ToVector2Int();

            Gizmos.DrawRay(origin, rayDir * raycastSettings.RaycastDistance);
        }
    }
}
