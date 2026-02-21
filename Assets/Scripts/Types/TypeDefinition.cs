using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Types
{
    /// <summary>
    /// Define a Monster or move type, including its icon and type effectiveness.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Type/Definition")]
    internal sealed class TypeDefinition : ScriptableObject
    {
        [SerializeField, Required] private Sprite icon;
        [SerializeField] private TypeEffectivenessGroups effectivenessGroups;

        internal Sprite Icon => icon;
        internal TypeEffectivenessGroups EffectivenessGroups => effectivenessGroups;
    }
}
