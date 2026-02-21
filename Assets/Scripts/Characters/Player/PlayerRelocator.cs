using MonsterTamer.Characters.Core;
using MonsterTamer.Map;
using MonsterTamer.SceneManagement;
using MonsterTamer.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Characters.Player
{
    /// <summary>
    /// Relocates the player after a scene load using <see cref="MapEntryRegistry"/>.
    /// Defaults to world origin if no valid entry is found.
    /// </summary>
    internal sealed class PlayerRelocator : Singleton<PlayerRelocator>
    {
        [SerializeField, Required, Tooltip("The player character instance to position after scene load.")]
        private Character player;

        private void OnEnable() => SceneReadyNotifier.SceneReady += RelocatePlayer;
        private void OnDisable() => SceneReadyNotifier.SceneReady -= RelocatePlayer;

        internal void RelocatePlayer()
        {
            var spawnId = MapEntryRegistry.NextEntryId;

            if (spawnId != MapEntryID.None && MapEntryRegistry.TryGetEntryPosition(spawnId, out var position))
            {
                player.Relocate(position);
                MapEntryRegistry.Clear();
                return;
            }

            Log.Warning(nameof(PlayerRelocator), $"No entry marker found for {spawnId}, spawning at (0,0,0).");
            player.Relocate(Vector3.zero);
        }
    }
}
