using MonsterTamer.Summary;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Party.UI.PartyOptions
{
    /// <summary>
    /// Coordinates party options by handling view events and invoking presenter actions.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PartyMenuOptionsPresenter : MonoBehaviour
    {
        [SerializeField, Required] private PartyMenuOptionsView partyOptionsView;
        [SerializeField, Required] private PartyMenuPresenter partyPresenter;

        private void OnEnable()
        {
            partyOptionsView.SwapRequested += OnSwapRequested;
            partyOptionsView.InfoRequested += OnInfoRequested;
            partyOptionsView.BackRequested += OnBackRequested;
        }

        private void OnDisable()
        {
            partyOptionsView.SwapRequested -= OnSwapRequested;
            partyOptionsView.InfoRequested -= OnInfoRequested;
            partyOptionsView.BackRequested -= OnBackRequested;
        }

        private void OnSwapRequested()
        {
            partyPresenter.StartSwap();
            ViewManager.Instance.Close<PartyMenuOptionsView>();
        } 

        private void OnInfoRequested() => ViewManager.Instance.Show<SummaryView>();
        private void OnBackRequested() => ViewManager.Instance.Close<PartyMenuOptionsView>();
    }
}