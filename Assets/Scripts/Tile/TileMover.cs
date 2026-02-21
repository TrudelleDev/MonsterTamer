using System;
using System.Collections;
using MonsterTamer.Characters.Directions;
using UnityEngine;

namespace MonsterTamer.Tile
{
    /// <summary>
    /// Provides smooth, grid-based movement and directional path checking.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TileRaycaster))]
    internal sealed class TileMover : MonoBehaviour
    {
        private TileRaycaster raycaster;

        internal event Action MoveStarted;
        internal event Action MoveCompleted;

        internal bool IsMoving { get; private set; }

        private void Awake() => raycaster = GetComponent<TileRaycaster>();

        internal bool CanMoveInDirection(FacingDirection direction) => raycaster.IsPathClear(direction);

        /// <summary>
        /// Smoothly interpolates the transform to a new grid position.
        /// </summary>
        /// <param name="destination">The target world-space coordinates.</param>
        /// <param name="duration">Total time to complete the slide.</param>
        internal IEnumerator MoveToTile(Vector3 destination, float duration)
        {
            Vector3 startPosition = transform.position;
            IsMoving = true;
            float elapsed = 0f;

            MoveStarted?.Invoke();

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.Lerp(startPosition, destination, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = destination;
            IsMoving = false;
            MoveCompleted?.Invoke();
        }
    }
}
