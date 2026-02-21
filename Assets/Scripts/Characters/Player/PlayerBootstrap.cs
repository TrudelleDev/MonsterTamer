using MonsterTamer.Characters.Core;
using UnityEngine;

namespace MonsterTamer.Characters.Player
{
    /// <summary>
    /// Registers the player character in the PlayerRegistry.
    /// </summary>
    internal sealed class PlayerBootstrap : MonoBehaviour
    {
        private Character player;

        private void Awake()
        {
            player = GetComponent<Character>();
            PlayerRegistry.Register(player);
        }

        private void OnDestroy()
        {
            PlayerRegistry.Unregister(player);
        }
    }
}
