using System;
using MonsterTamer.Party.UI.Slots;
using MonsterTamer.Shared.UI.Core;
using MonsterTamer.Shared.UI.Navigation;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MonsterTamer.Party.UI
{
    /// <summary>
    /// Displays the player's party and raises events for slot selection.
    /// Manages prompts and swap highlights in the UI.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PartyMenuView : View
    {
        private const string SelectMonsterMessage = "Select a Monster.";
        private const string SwapStartMessage = "Move to where?";
        private const string ChooseActionMessage = "Do what with this Monster?";

        [Title("Party View Settings")]
        [SerializeField, Required] private PartyMenuSlotManager menuSlotManager;
        [SerializeField, Required] private VerticalMenuController partySlotController;
        [SerializeField, Required] private TextMeshProUGUI dialogueText;

        private MenuButton lockedSwapButton;

        internal event Action<PartyMenuSlot> SlotRequested;

        internal MenuButton CurrentSlotButton => partySlotController.CurrentButton;

        private void OnEnable()
        {
            partySlotController.Selected += OnSlotRequested;
            dialogueText.text = SelectMonsterMessage;
        }

        private void OnDisable()
        {
            partySlotController.Selected -= OnSlotRequested;
        }

        internal void Refresh() => menuSlotManager.Refresh();
        internal void ShowChoosePrompt() => dialogueText.text = SelectMonsterMessage;

        internal void ShowSwapPrompt(MenuButton selectedButton)
        {
            lockedSwapButton = selectedButton;

            if (lockedSwapButton != null)
            {
                lockedSwapButton.LockSelectSprite = true;
            }

            dialogueText.text = SwapStartMessage;
        }

        internal void ClearSwapLock()
        {
            if (lockedSwapButton != null)
            {
                lockedSwapButton.LockSelectSprite = false;
            }

            lockedSwapButton = null;
            dialogueText.text = SelectMonsterMessage;
        }

        private void OnSlotRequested(MenuButton menuButton)
        {
            if (!menuButton.TryGetComponent(out PartyMenuSlot slot) ||
                slot.BoundMonster == null) return;

            // Only change text to "Choose Action" if we aren't swapping
            if (lockedSwapButton == null)
            {
                dialogueText.text = ChooseActionMessage;
            }

            SlotRequested?.Invoke(slot);
        }
    }
}