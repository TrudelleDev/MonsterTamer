using MonsterTamer.Characters.Core;
using MonsterTamer.Monsters;
using MonsterTamer.Party.UI.PartyOptions;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Summary
{
    /// <summary>
    /// Display detailed monster data, including statistics and move-sets.
    /// </summary>
    internal sealed class SummaryView : View
    {
        [Title("Summary View Settings")]
        [SerializeField, Required] private SummaryTabGroup summaryTabs;
        [SerializeField, Required] private Character player;

        private void OnEnable()
        {
            summaryTabs.Unbind();

            Monster selectedMonster = player.Party.SelectedMonster;
            if (selectedMonster?.Definition == null) return;

            summaryTabs.Bind(selectedMonster);
            summaryTabs.ResetControllers();
            BackRequested += OnBackRequested;
        }

        private void OnDisable()
        {
            BackRequested -= OnBackRequested;
        }

        private void OnBackRequested() => ViewManager.Instance.Close<SummaryView>();
    }
}