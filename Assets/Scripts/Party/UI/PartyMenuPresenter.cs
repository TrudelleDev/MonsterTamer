using MonsterTamer.Characters.Core;
using MonsterTamer.Monsters;
using MonsterTamer.Party.Enums;
using MonsterTamer.Party.UI.PartyOptions;
using MonsterTamer.Party.UI.Slots;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MonsterTamer.Party.UI
{
    /// <summary>
    /// Manages party selection logic, handles slot input, swaps, and item targeting.
    /// Controls whether the menu can close based on current mode and state.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PartyMenuPresenter : MonoBehaviour
    {
        [SerializeField, Required] private PartyMenuView partyMenuView;
        [SerializeField, Required] private Character player;

        private PartySelectionMode mode;
        private bool isSwapping;
        private int swapIndexA;

        internal event Action OptionsRequested;
        internal event Action<Monster> ItemTargetRequested;

        internal bool IsForced { get; set; }

        private void OnEnable()
        {
            partyMenuView.SlotRequested += OnSlotRequested;
            partyMenuView.BackRequested += OnBackRequested;
            partyMenuView.ShowChoosePrompt();
            partyMenuView.Refresh();

            OptionsRequested += OnOptionsRequested;
        }

        private void OnDisable()
        {
            partyMenuView.SlotRequested -= OnSlotRequested;
            partyMenuView.BackRequested -= OnBackRequested;

            OptionsRequested -= OnOptionsRequested;
        }

        // Add this so the Controller knows if it can close the menu!
        internal bool RequestClose(bool isForced = false)
        {
            if (isSwapping)
            {
                EndSwap();
                return false; // Just stop swapping, don't close menu
            }

            // In Battle, we can only go back if it's NOT a forced switch
            if (mode == PartySelectionMode.Battle && isForced)
            {
                return false;
            }

            return true; // Safe to close in Overworld or voluntary Battle swap
        }

        internal void SetMode(PartySelectionMode selectionMode) => mode = selectionMode;

        internal void StartSwap()
        {
            if (mode != PartySelectionMode.Overworld) return;

            isSwapping = true;
            swapIndexA = player.Party.SelectedIndex;
            partyMenuView.ShowSwapPrompt(partyMenuView.CurrentSlotButton);
        }

        private void EndSwap()
        {
            isSwapping = false;
            partyMenuView.ClearSwapLock();
        }

        private void OnSlotRequested(PartyMenuSlot slot)
        {
            player.Party.SetSelection(slot.Index);

            switch (mode)
            {
                case PartySelectionMode.Overworld:
                    ProcessOverworldInput(slot);
                    break;
                case PartySelectionMode.Battle:
                    OptionsRequested?.Invoke();
                    break;
                case PartySelectionMode.UseItem:
                    ItemTargetRequested?.Invoke(player.Party.SelectedMonster);
                    break;
            }
        }

        private void ProcessOverworldInput(PartyMenuSlot slot)
        {
            if (isSwapping)
            {
                if (swapIndexA != slot.Index) player.Party.Swap(swapIndexA, slot.Index);
                EndSwap();
                partyMenuView.Refresh();
                return;
            }
            OptionsRequested?.Invoke();
        }

        private void OnOptionsRequested()
        {
            ViewManager.Instance.Show<PartyMenuOptionsView>();
        }

        private void OnBackRequested()
        {
            if (RequestClose(IsForced))
            {
                ViewManager.Instance.Close<PartyMenuView>();
            }
            else
            {
                // If we were swapping, RequestClose() will just stop swap
                partyMenuView.Refresh();
            }
        }
    }
}