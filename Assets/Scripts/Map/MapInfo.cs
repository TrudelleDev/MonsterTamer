using UnityEngine;

namespace MonsterTamer.Map
{
    /// <summary>
    /// Provides global access to the tile grid cell size.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Grid))]
    [ExecuteAlways]
    internal sealed class MapInfo : MonoBehaviour
    {
        internal static Vector3 CellSize { get; private set; }

        private void OnEnable()
        {
            CellSize = GetComponent<Grid>().cellSize;
        }
    }
}
