using MonsterTamer.GameMenu;
using MonsterTamer.Inventory.UI;
using MonsterTamer.Party.Enums;
using MonsterTamer.Party.UI;
using UnityEngine;

namespace MonsterTamer.Views
{
    /// <summary>
    /// Handles game menu events and navigates to other menus.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameMenuView))]
    internal sealed class GameMenuPresenter : MonoBehaviour
    {
        private GameMenuView menuView;

        private void Awake() => menuView = GetComponent<GameMenuView>();

        private void OnEnable()
        {
            menuView.PartyOpenRequested += OnPartyOpenRequested;
            menuView.InventoryOpenRequested += OnInventoryOpenRequested;
            menuView.BackRequested += OnCloseRequested;
        }

        private void OnDisable()
        {
            menuView.PartyOpenRequested -= OnPartyOpenRequested;
            menuView.InventoryOpenRequested -= OnInventoryOpenRequested;
            menuView.BackRequested -= OnCloseRequested;
        }

        private void OnPartyOpenRequested()
        {
            var partyMenuView = ViewManager.Instance.Show<PartyMenuView>();
            var partyMenuPresenter = partyMenuView.GetComponent<PartyMenuPresenter>();

            partyMenuPresenter.SetMode(PartySelectionMode.Overworld);
        }

        private void OnInventoryOpenRequested() => ViewManager.Instance.Show<InventoryView>();
        private void OnCloseRequested() => ViewManager.Instance.Close<GameMenuView>();
    }
}
