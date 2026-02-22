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

        internal event Action<Monster> ItemTargetRequested;

        private bool CanClose => currentState == PartyMenuState.Selection && !IsBattleSwap;
        internal bool IsBattleSwap { get; set; }

        private void OnEnable()
        {
            partyMenuView.SlotRequested += OnSlotRequested;
            partyMenuView.BackRequested += OnBackRequested;
            player.Party.PartyChanged += OnPartyChanged;

            ResetToSelection();
        }

        private void OnDisable()
        {
            partyMenuView.SlotRequested -= OnSlotRequested;
            partyMenuView.BackRequested -= OnBackRequested;
            player.Party.PartyChanged -= OnPartyChanged;
        }

        internal void SetState(PartyMenuState state) => currentState = state;

        internal void ResetToSelection()
        {
            currentState = PartyMenuState.Selection;
            partyMenuView.ShowSelectionPrompt();
        }

        internal void StartSwap()
        {
            currentState = PartyMenuState.Swap;
            swapIndexA = player.Party.SelectedIndex;

            if (!IsBattleSwap)
            {
                partyMenuView.ShowSwapPrompt(partyMenuView.CurrentSlotButton);
            }
        }
        private void HandleSwap(PartyMenuSlot slot)
        {
            if (swapIndexA != slot.Index)
            {
                player.Party.Swap(swapIndexA, slot.Index);
            }

            currentState = PartyMenuState.Selection;
            partyMenuView.ClearSwapLock();
        }

        private void CancelSwap()
        {
            currentState = PartyMenuState.Selection;
            partyMenuView.ClearSwapLock();
        }

        internal void StartBattleSelection(bool forced)
        {
            currentState = PartyMenuState.Selection;
            IsBattleSwap = forced;
            ViewManager.Instance.Show<PartyMenuView>();
        }

        private void OnSlotRequested(PartyMenuSlot slot)
        {
            player.Party.SetSelection(slot.Index);

            switch (currentState)
            {
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
                case PartyMenuState.Swap:
                    CancelSwap();
                    break;

                case PartyMenuState.Options:
                    CancelOptions();
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

        internal void CancelOptions()
        {
            currentState = PartyMenuState.Selection;
            partyMenuView.ShowSelectionPrompt();
        }
    }
}