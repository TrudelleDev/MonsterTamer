using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Monsters.UI
{
    /// <summary>
    /// Displays a monster's core stats including HP, Attack, Defense,
    /// Special Attack, Special Defense, and Speed.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class MonsterStatsPanel : MonoBehaviour
    {
        [SerializeField, Required]
        [Tooltip("Displays and animates the monster's health bar.")]
        private HealthBar healthBar;

        [SerializeField, Required]
        [Tooltip("Displays the monster's current and maximum HP.")]
        private TextMeshProUGUI healthText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's Attack stat.")]
        private TextMeshProUGUI attackText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's Defense stat.")]
        private TextMeshProUGUI defenseText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's Special Attack stat.")]
        private TextMeshProUGUI specialAttackText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's Special Defense stat.")]
        private TextMeshProUGUI specialDefenseText;

        [SerializeField, Required]
        [Tooltip("Displays the monster's Speed stat.")]
        private TextMeshProUGUI speedText;

        /// <summary>
        /// Binds a Monster instance and updates the displayed stats.
        /// </summary>
        internal void Bind(Monster monster)
        {
            if (monster == null)
            {
                Unbind();
                return;
            }

            healthBar.Bind(monster);
            UpdateStats(monster);
        }

        /// <summary>
        /// Clears all displayed stat values.
        /// </summary>
        internal void Unbind()
        {
            healthBar.Unbind();

            healthText.text = string.Empty;
            attackText.text = string.Empty;
            defenseText.text = string.Empty;
            specialAttackText.text = string.Empty;
            specialDefenseText.text = string.Empty;
            speedText.text = string.Empty;
        }

        private void UpdateStats(Monster monster)
        {
            healthText.text = $"{monster.Health.CurrentHealth}/{monster.Health.MaxHealth}";
            attackText.text = monster.Stats.Core.Attack.ToString();
            defenseText.text = monster.Stats.Core.Defense.ToString();
            specialAttackText.text = monster.Stats.Core.SpecialAttack.ToString();
            specialDefenseText.text = monster.Stats.Core.SpecialDefense.ToString();
            speedText.text = monster.Stats.Core.Speed.ToString();
        }
    }
}
