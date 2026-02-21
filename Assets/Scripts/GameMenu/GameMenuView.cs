using System;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.GameMenu
{
    /// <summary>
    /// Main game menu view that raises navigation events.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class GameMenuView : View
    {
        [Title("Game Menu Settings")]
        [SerializeField, Required] private MenuButton partyButton;
        [SerializeField, Required] private MenuButton inventoryButton;
        [SerializeField, Required] private MenuButton exitButton;

        internal event Action PartyOpenRequested;
        internal event Action InventoryOpenRequested;

        private void OnEnable()
        {
            partyButton.Selected += OnPartyOpenRequested;
            inventoryButton.Selected += OnInventoryOpenRequested;
            exitButton.Selected += OnBackRequested;
        }

        private void OnDisable()
        {
            partyButton.Selected -= OnPartyOpenRequested;
            inventoryButton.Selected -= OnInventoryOpenRequested;
            exitButton.Selected -= OnBackRequested;
        }

        protected override void Update()
        {
            base.Update();

            // Only allow closing if this is the active view.
            if (ViewManager.Instance.CurrentView != this) return;

            if (Input.GetKeyDown(KeyBinds.Menu))
            {
                CloseRequest(true);
            }
        }

        private void OnPartyOpenRequested() => PartyOpenRequested?.Invoke();
        private void OnInventoryOpenRequested() => InventoryOpenRequested?.Invoke();
        private void OnBackRequested() => CloseRequest(playSound: false);
    }
}
