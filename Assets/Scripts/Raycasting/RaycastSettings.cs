using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Raycasting
{
    /// <summary>
    /// Holds common raycasting settings for interaction and pathfinding.
    /// </summary>
    [CreateAssetMenu(fileName = "RaycastSettings", menuName = "Game/RaycastSettings", order = 1)]
    public class RaycastSettings : ScriptableObject
    {
        [Header("Raycast Configuration")]

        [Tooltip("The maximum distance the raycast will travel.")]
        [SerializeField, Required]
        private float raycastDistance = 1f;

        [SerializeField, Required]
        [Tooltip("The offset applied to the raycast origin (relative to the source transform).")]
        private Vector2 raycastOffset;

        [SerializeField, Required]
        [Tooltip("Determines which layers the raycast will hit.")]
        private LayerMask interactionMask = Physics2D.DefaultRaycastLayers;

        internal Vector2 RaycastOffset => raycastOffset; 
        internal float RaycastDistance => raycastDistance;
        internal LayerMask InteractionMask => interactionMask;
    }
}
