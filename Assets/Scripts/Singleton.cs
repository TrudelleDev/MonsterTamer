using MonsterTamer.Utilities;
using UnityEngine;

namespace MonsterTamer
{
    /// <summary>
    /// Generic base class for implementing a singleton MonoBehaviour.
    /// </summary>
    internal abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        internal static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Log.Warning(nameof(Singleton<T>), $"Duplicate {typeof(T).Name} detected, destroying.");
                Destroy(gameObject);
                return;
            }

            Instance = (T)this;
        }
    }
}
