using UnityEngine;

namespace MonsterTamer.Natures
{
    /// <summary>
    /// Provide a list of all available Natures and provides utility methods.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Nature/Nature Database")]
    internal sealed class NatureDatabase : ScriptableObject
    {
        [SerializeField, Tooltip("All available nature definitions.")]
        private NatureDefinition[] natures;

        internal NatureDefinition GetRandomNature()
        {
            return natures[UnityEngine.Random.Range(0, natures.Length)];
        }
    }
}
