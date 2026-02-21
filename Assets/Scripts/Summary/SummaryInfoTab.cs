using MonsterTamer.Monsters;
using MonsterTamer.Monsters.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Summary
{
    /// <summary>
    /// Displays monster overview and trainer memo panels within the summary interface. 
    /// </summary>
    internal sealed class SummaryInfoTab : MonoBehaviour
    {
        [SerializeField, Required] private MonsterOverviewPanel monsterOverviewUI;
        [SerializeField, Required] private TrainerMemoUI trainerMemoUI;

        internal void Bind(Monster monster)
        {
            Unbind();

            if (monster?.Definition == null) return;

            monsterOverviewUI.Bind(monster);
            trainerMemoUI.Bind(monster);
        }

        internal void Unbind()
        {
            monsterOverviewUI.Unbind();
            trainerMemoUI.Unbind();
        }
    }
}