using MonsterTamer.Monsters;
using MonsterTamer.Monsters.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Battle.UI
{
    /// <summary>
    /// Displays the player active Monster's battle HUD.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PlayerBattleHud : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI nameText;
        [SerializeField, Required] private TextMeshProUGUI levelText;
        [SerializeField, Required] private TextMeshProUGUI healthText;
        [SerializeField, Required] private HealthBar healthBar;
        [SerializeField, Required] private ExperienceBar experienceBar;
        [SerializeField, Required] private Image backSprite;

        private Monster activeMonster;

        internal HealthBar HealthBar => healthBar;
        internal ExperienceBar ExperienceBar => experienceBar;

        internal void Bind(Monster monster)
        {
            UnsubscribeCurrentMonster();

            if (monster?.Definition == null)
            {
                Unbind();
                return;
            }

            activeMonster = monster;

            nameText.text = monster.Definition.DisplayName;
            levelText.text = $"L{monster.Experience.Level}";
            backSprite.sprite = monster.Definition.Sprites.BackSprite;

            healthBar.Bind(monster);
            experienceBar.Bind(monster);

            activeMonster.Health.HealthChanged += OnHealthChanged;
            activeMonster.Experience.LevelChanged += OnLevelChanged;

            UpdateHealthText();
        }

        internal void Unbind()
        {
            UnsubscribeCurrentMonster();

            nameText.text = string.Empty;
            levelText.text = string.Empty;
            healthText.text = "- / -";
            backSprite.sprite = null;

            healthBar.Unbind();
            experienceBar.Unbind();
        }

        private void OnHealthChanged(int oldHealth, int newHealth)
        {
            UpdateHealthText();
        }

        private void OnLevelChanged(int newLevel)
        {
            levelText.text = $"L{newLevel}";
            UpdateHealthText();
        }

        private void UpdateHealthText()
        {
            healthText.text = $"{activeMonster.Health.CurrentHealth}/{activeMonster.Health.MaxHealth}";
        }

        private void UnsubscribeCurrentMonster()
        {
            if (activeMonster == null) return;

            activeMonster.Health.HealthChanged -= OnHealthChanged;
            activeMonster.Experience.LevelChanged -= OnLevelChanged;
            activeMonster = null;
        }
    }
}
