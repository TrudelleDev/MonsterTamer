using MonsterTamer.Shared.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Shared.UI.Definitions
{
    /// <summary>
    /// Defines a menu option representing a "Cancel" action.
    /// </summary>
    [CreateAssetMenu(menuName = "MonsterTamer/Menu Options/Cancel Option")]
    internal class CancelMenuOptionDefinition : ScriptableObject, IDisplayable
    {
        [SerializeField, Required]private string displayName;
        [SerializeField, Required]private Sprite icon;
        [SerializeField, TextArea]private string description;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public string Description => description;
    }
}
