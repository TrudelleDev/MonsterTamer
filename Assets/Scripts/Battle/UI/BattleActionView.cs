using System;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Battle.UI
{
    /// <summary>
    /// Displays the main battle action options (Fight, Bag, Monsters, Flee).
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class BattleActionView : View
    {
        [SerializeField, Required] private MenuButton moveSelectionButton;
        [SerializeField, Required] private MenuButton inventoryButton;
        [SerializeField, Required] private MenuButton partyButton;
        [SerializeField, Required] private MenuButton escapeButton;

        internal event Action MoveSelectionRequested;
        internal event Action InventoryRequested;
        internal event Action PartyRequested;
        internal event Action EscapeRequested;

        private void OnEnable()
        {
            moveSelectionButton.Selected += OnMoveSelectionRequested;
            inventoryButton.Selected += OnInventoryRequested;
            partyButton.Selected += OnPartyRequested;
            escapeButton.Selected += OnEscapeRequested;          
        }

        private void OnDisable()
        {
            moveSelectionButton.Selected -= OnMoveSelectionRequested;
            inventoryButton.Selected -= OnInventoryRequested;
            partyButton.Selected -= OnPartyRequested;
            escapeButton.Selected -= OnEscapeRequested;
        }

        private void OnMoveSelectionRequested() => MoveSelectionRequested?.Invoke();
        private void OnInventoryRequested() => InventoryRequested?.Invoke();
        private void OnPartyRequested() => PartyRequested?.Invoke();
        private void OnEscapeRequested() => EscapeRequested?.Invoke();
    }
}