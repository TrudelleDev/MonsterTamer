using System;
using UnityEngine;

namespace MonsterTamer.SceneManagement
{
    /// <summary>
    /// Signals when a scene has completed its first frame of execution.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class SceneReadyNotifier : MonoBehaviour
    {
        internal static event Action SceneReady;

        private void Start()
        {
            SceneReady?.Invoke();
        }
    }
}
