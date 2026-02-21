using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Monsters.UI
{
    /// <summary>
    /// Handles the Monster EXP Panel, including total EXP, EXP to next level, and the EXP bar animation.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class ExperiencePanel : MonoBehaviour
    {
        [SerializeField, Required]
        [Tooltip("Text displaying the Monster's total experience.")]
        private TextMeshProUGUI totalExperiencePointText;

        [SerializeField, Required]
        [Tooltip("Text displaying the experience required for the next level.")]
        private TextMeshProUGUI nextLevelExperiencePointText;

        [SerializeField, Required]
        [Tooltip("Reference to the ExperienceBar component.")]
        private ExperienceBar experienceBar;

        private Monster boundMonster;

        /// <summary>
        /// Binds the UI to a specific Monster instance and updates the display whenever its EXP changes.
        /// </summary>
        internal void Bind(Monster monster)
        {
            if (monster == null)
            {
                Unbind();
                return;
            }

            Unbind(); // Remove previous bindings
            boundMonster = monster;

            boundMonster.Experience.ExperienceChanged += HandleExperienceChanged;
            boundMonster.Experience.LevelChanged += HandleLevelChanged;

            UpdateExperienceUI();
            experienceBar.Bind(boundMonster);
        }

        /// <summary>
        /// Unbinds the current Monster and clears the UI.
        /// </summary>
        internal void Unbind()
        {
            if (boundMonster != null)
            {
                boundMonster.Experience.ExperienceChanged -= HandleExperienceChanged;
                boundMonster.Experience.LevelChanged -= HandleLevelChanged;
                boundMonster = null;
            }

            totalExperiencePointText.text = string.Empty;
            nextLevelExperiencePointText.text = string.Empty;
            experienceBar.Unbind();
        }

        /// <summary>
        /// Updates the EXP text fields to match the bound Monster's current EXP.
        /// </summary>
        private void UpdateExperienceUI()
        {
            if (boundMonster == null) return;

            int totalExp = boundMonster.Experience.TotalExperience;
            int nextLevelExp = Mathf.Max(boundMonster.Experience.GetExpForNextLevel() - totalExp, 0);

            totalExperiencePointText.text = totalExp.ToString();
            nextLevelExperiencePointText.text = nextLevelExp.ToString();
        }

        private void HandleExperienceChanged(int oldExp, int newExp)
        {
            UpdateExperienceUI();
        }

        private void HandleLevelChanged(int newLevel)
        {
            UpdateExperienceUI();
        }
    }
}
