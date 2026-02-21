using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Monsters.UI
{
    /// <summary>
    /// Displays trainer-related information for a monster, 
    /// such as the encounter location and level met.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class TrainerMemoUI : MonoBehaviour
    {
        [SerializeField, Required]
        [Tooltip("Text field displaying trainer-related information.")]
        private TextMeshProUGUI memoText;

        /// <summary>
        /// Binds trainer-related data from the monster instance to the memo text.
        /// </summary>m>
        internal void Bind(Monster monster)
        {
            if (monster == null || monster.Meta == null)
            {
                Unbind();
                return;
            }

            string location = string.IsNullOrWhiteSpace(monster.Meta.EncounterLocation)
                ? "an unknown location"
                : monster.Meta.EncounterLocation;

            int levelMet = monster.Experience?.Level ?? 1;

            memoText.text = $"Met at {location} at level {levelMet}.";
        }

        /// <summary>
        /// Clears the trainer memo text.
        /// </summary>
        internal void Unbind()
        {
            memoText.text = string.Empty;
        }
    }
}
