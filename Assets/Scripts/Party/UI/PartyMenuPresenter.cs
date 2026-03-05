using System;
using MonsterTamer.Characters.Core;
using MonsterTamer.Monsters;
using MonsterTamer.Party.Enums;
using MonsterTamer.Party.UI.PartyOptions;
using MonsterTamer.Party.UI.Slots;
using MonsterTamer.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Party.UI
{
    /// <summary>
    /// Manages party menu logic, translating UI inputs into PartyManager actions.
    /// Coordinates states (Selection, Swap, Item) and enforces Battle/Overworld rules.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class PartyMenuPresenter : MonoBehaviour
    {
        [SerializeField, Required] private PartyMenuView partyMenuView;
        [SerializeField, Required] private Character player;

        private PartyMenuState currentState;
        private int swapIndexA;
        private bool isForcedContext;

        internal event Action<Monster> ItemTargetRequested;

        private bool CanClose => currentState == PartyMenuState.Selection;

        private void OnEnable()
        {
            partyMenuView.SlotRequested += OnSlotRequested;
            partyMenuView.BackRequested += OnBackRequested;
            player.Party.PartyChanged += OnPartyChanged;
        }

        private void OnDisable()
        {
            partyMenuView.SlotRequested -= OnSlotRequested;
            partyMenuView.BackRequested -= OnBackRequested;
            player.Party.PartyChanged -= OnPartyChanged;
        }

        internal void StartSwap()
        {
            // 1. Check if we are currently in a forced state BEFORE changing it
            bool wasForced = (currentState == PartyMenuState.BattleForcedSelection);

            // 2. Now change to Swap state
            currentState = PartyMenuState.Swap;
            swapIndexA = player.Party.SelectedIndex;

            if (!wasForced)
            {
                partyMenuView.ShowSwapPrompt(partyMenuView.CurrentSlotButton);
            }
            else
            {
                // If in battle swap, immediately handle swap when slot is clicked
                partyMenuView.ClearSwapLock();
            }
        }

        private void HandleSwap(PartyMenuSlot slot)
        {
            if (swapIndexA != slot.Index)
            {
                player.Party.Swap(swapIndexA, slot.Index);
            }

            ReturnToBaseSelection();
        }

        private void CancelSwap()
        {
            currentState = isForcedContext ? PartyMenuState.BattleForcedSelection : PartyMenuState.Selection;
            partyMenuView.ClearSwapLock();
        }

        internal void StartBattleSelection(bool forced)
        {
            isForcedContext = forced;

            // Initial state setup
            currentState = forced ? PartyMenuState.BattleForcedSelection : PartyMenuState.Selection;

            ViewManager.Instance.Show<PartyMenuView>();
            partyMenuView.ShowSelectionPrompt();
        }

        internal void StartItemSelection()
        {
            currentState = PartyMenuState.Item;
            partyMenuView.ShowSelectionPrompt();
        }


        internal void ReturnToBaseSelection()
        {
            // Use the context flag we set in StartBattleSelection
            currentState = isForcedContext ? PartyMenuState.BattleForcedSelection : PartyMenuState.Selection;

            partyMenuView.ShowSelectionPrompt();
            partyMenuView.ClearSwapLock(); // Safety: makes sure no "Swap ghost" icons are left
        }

        private void OnSlotRequested(PartyMenuSlot slot)
        {
            player.Party.SetSelection(slot.Index);

            switch (currentState)
            {
                case PartyMenuState.BattleForcedSelection:
                    ShowOptions();
                    break;

                case PartyMenuState.Selection:
                    ShowOptions();
                    break;

                case PartyMenuState.Swap:
                    HandleSwap(slot);
                    break;

                case PartyMenuState.Item:
                    ItemTargetRequested?.Invoke(player.Party.SelectedMonster);
                    break;
            }
        }

        private void OnBackRequested()
        {
            switch (currentState)
            {
                case PartyMenuState.BattleForcedSelection:
                    // Do nothing. The player MUST pick a monster.
                    break;

                case PartyMenuState.Swap:
                    CancelSwap();
                    break;

                case PartyMenuState.Options:
                    ReturnToBaseSelection();
                    break;

                case PartyMenuState.Item:
                    ReturnToBaseSelection();
                    ViewManager.Instance.Close<PartyMenuView>();
                    break;

                case PartyMenuState.Selection:
                    if (CanClose)
                    {
                        ViewManager.Instance.Close<PartyMenuView>();
                    }
                    break;
            }
        }

        private void OnPartyChanged() => partyMenuView.Refresh();

        private void ShowOptions()
        {
            currentState = PartyMenuState.Options;
            ViewManager.Instance.Show<PartyMenuOptionsView>();
        }
    }
}