using MonsterTamer.Monsters;
using MonsterTamer.Monsters.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Summary
{
    /// <summary>
    /// Displays monster statistics and experience progress within the summary interface.
    /// </summary>
    internal sealed class SummarySkillTab : MonoBehaviour
    {
        [SerializeField, Required] private MonsterStatsPanel statsPanel;
        [SerializeField, Required] private ExperiencePanel experiencePanel;

        internal void Bind(Monster monster)
        {
            Unbind();

            if (monster?.Definition == null) return;

            statsPanel.Bind(monster);
            experiencePanel.Bind(monster);
        }

        internal void Unbind()
        {
            statsPanel.Unbind();
            experiencePanel.Unbind();
        }
    }
}