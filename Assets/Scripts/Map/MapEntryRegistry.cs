using System.Collections.Generic;
using UnityEngine;

namespace MonsterTamer.Map
{
    /// <summary>
    /// Global registry for map entry points.
    /// Tracks <see cref="MapEntryPointMarker"/> instances by ID for easy lookup of world positions.
    /// </summary>
    internal static class MapEntryRegistry
    {
        internal static MapEntryID NextEntryId { get; private set; } = MapEntryID.None;

        private static readonly Dictionary<MapEntryID, MapEntryPointMarker> entryMarkers = new();

        internal static void Register(MapEntryPointMarker marker)
        {
            if (marker != null)
            {
                entryMarkers[marker.EntryId] = marker;
            }
        }

        internal static void Unregister(MapEntryPointMarker marker)
        {
            if (marker != null && entryMarkers.TryGetValue(marker.EntryId, out var existing) && existing == marker)
            {
                entryMarkers.Remove(marker.EntryId);
            }
        }

        internal static bool TryGetEntryPosition(MapEntryID entryId, out Vector3 position)
        {
            if (entryMarkers.TryGetValue(entryId, out var marker))
            {
                position = marker.Position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }

        internal static void SetNextEntry(MapEntryID entryId) => NextEntryId = entryId;
        internal static void Clear() => NextEntryId = MapEntryID.None;
    }
}
