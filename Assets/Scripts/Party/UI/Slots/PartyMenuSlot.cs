using MonsterTamer.Monsters;
using MonsterTamer.Monsters.UI;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Shared.UI.MenuButtons;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterTamer.Party.UI.Slots
{
    /// <summary>
    /// Handles the visual representation and health tracking of a single monster in the UI.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MenuButton))]
    internal sealed class PartyMenuSlot : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI nameText;
        [SerializeField, Required] private TextMeshProUGUI levelText;
        [SerializeField, Required] private TextMeshProUGUI healthText;
        [SerializeField, Required] private Image menuSprite;
        [SerializeField, Required] private HealthBar healthBar;
        [SerializeField, Required] private GameObject contentRoot;

        private MenuButton menuButton;
       
        internal Monster BoundMonster { get; private set; }
        internal int Index { get; private set; }

        internal void SetSlotIndex(int index) => Index = index;

        internal void Bind(Monster monster)
        {
            Unbind();

            if (monster?.Definition == null) return;

            BoundMonster = monster;
            BoundMonster.Health.HealthChanged += OnHealthChanged;

            UpdateDisplay(BoundMonster);
            SetSlotVisibility(true);
        }

        internal void Unbind()
        {
            if (BoundMonster != null)
            {
                BoundMonster.Health.HealthChanged -= OnHealthChanged;
                BoundMonster = null;
            }

            nameText.text = string.Empty;
            levelText.text = string.Empty;
            healthText.text = string.Empty;

            menuSprite.sprite = null;
            menuSprite.enabled = false;

            healthBar.Unbind();
            SetSlotVisibility(false);
        }

        private void UpdateDisplay(Monster monster)
        {
            nameText.text = monster.Definition.DisplayName;
            levelText.text = $"Lv{monster.Experience.Level}";
            healthText.text = $"{monster.Health.CurrentHealth}/{monster.Health.MaxHealth}";

            menuSprite.sprite = monster.Definition.Sprites.MenuSprite;
            menuSprite.enabled = true;

            healthBar.Bind(monster);
        }

        private void SetSlotVisibility(bool visible)
        {
            contentRoot.SetActive(visible);
            EnsureButton().SetInteractable(visible);
        }

        private MenuButton EnsureButton()
        {
            if (menuButton == null)
                menuButton = GetComponent<MenuButton>();

            return menuButton;
        }

        private void OnHealthChanged(int oldHealth, int newHealth) => UpdateDisplay(BoundMonster);
    }
}