using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Monsters.UI
{
    /// <summary>
    /// Displays general monster information such as codex number, name, types,
    /// original trainer, level, and nature.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MonsterOverviewPanel : MonoBehaviour
    {
        [SerializeField, Required]
        [Tooltip("Displays the monster's index number.")]
        private TextMeshProUGUI monsterIndexText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's name.")]
        private TextMeshProUGUI nameText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's primary type icon.")]
        private MonsterTypeIcon primaryTypeIcon;

        [SerializeField, Required]
        [Tooltip("Displays the monster's secondary type icon.")]
        private MonsterTypeIcon secondaryTypeIcon;

        [SerializeField, Required]
        [Tooltip("Displays the name of the monster's original trainer.")]
        private TextMeshProUGUI originalTrainerText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's current level.")]
        private TextMeshProUGUI levelText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's nature.")]
        private TextMeshProUGUI natureText;

        /// <summary>
        /// Binds a Monster instance to the UI, updating all fields.
        /// </summary>
        internal void Bind(Monster monster)
        {
            if (monster == null)
            {
                Unbind();
                return;
            }

            monsterIndexText.text = monster.Definition.CodexNumber.ToString("D3");
            nameText.text = monster.Definition.DisplayName;
            natureText.text = monster.Nature.Definition.DisplayName;
            originalTrainerText.text = monster.Meta.OwnerName;
            levelText.text = monster.Experience.Level.ToString();

            primaryTypeIcon.Bind(monster);
            secondaryTypeIcon.Bind(monster);
        }

        /// <summary>
        /// Clears all displayed monster information.
        /// </summary>
        internal void Unbind()
        {
            monsterIndexText.text = string.Empty;
            nameText.text = string.Empty;
            natureText.text = string.Empty;
            originalTrainerText.text = string.Empty;
            levelText.text = string.Empty;

            primaryTypeIcon.Unbind();
            secondaryTypeIcon.Unbind();
        }
    }
}
