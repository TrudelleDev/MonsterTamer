using MonsterTamer.Characters.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer
{
    /// <summary>
    /// Follows the player using a fixed world-space offset.
    /// </summary>
    internal class CameraFollow : MonoBehaviour
    {
        [SerializeField, Required] private Character player;

        [SerializeField, Required]
        [Tooltip("Offset from the player's position (world units). Default: (0, 1, -10).")]
        private Vector3 offset = new Vector3(0f, 1f, -10f);

        private void LateUpdate() => transform.position = player.transform.position + offset;
    }
}
