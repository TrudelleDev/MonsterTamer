using MonsterTamer.Utilities;
using UnityEngine;

namespace MonsterTamer.Map
{
    /// <summary>
    /// Marks a map entry point and registers it with <see cref="MapEntryRegistry"/>.
    /// Provides ID and world position for player spawning or relocation.
    internal sealed class MapEntryPointMarker : MonoBehaviour
    {
        [SerializeField] private MapEntryID entryId;

        internal MapEntryID EntryId => entryId;
        internal Vector3 Position => transform.position;

        private void OnEnable() => MapEntryRegistry.Register(this);
        private void OnDisable() => MapEntryRegistry.Unregister(this);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, MapInfo.CellSize);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position, entryId.ToString());
#endif
        }
    }
}
