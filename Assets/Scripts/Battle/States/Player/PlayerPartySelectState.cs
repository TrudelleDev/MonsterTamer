using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Monsters;
using MonsterTamer.Party.UI;
using MonsterTamer.Party.UI.PartyOptions;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Handles party selection. Transitions to SendOut if forced (fainted) or Swap if voluntary.
    /// </summary>
    internal sealed class PlayerPartySelectState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly bool isForced;

        private PartyMenuView partyView;
        private PartyMenuPresenter partyPresenter;
        private PartyMenuOptionsView optionsView;

        private BattleView Battle => machine.BattleView;

        internal PlayerPartySelectState(BattleStateMachine machine, bool isForced = false)
        {
            this.machine = machine;
            this.isForced = isForced;
        }

        public void Enter()
        {
            partyView = ViewManager.Instance.Show<PartyMenuView>();
            partyPresenter = partyView.GetComponent<PartyMenuPresenter>();
            optionsView = ViewManager.Instance.Get<PartyMenuOptionsView>();

            partyPresenter.StartBattleSelection(isForced);

            if (!isForced)
            {
                partyView.BackRequested += OnBackRequested;
            }

            optionsView.SwapRequested += OnSwapRequested;

            partyView.Refresh();
        }

        public void Exit() => PerformCleanup();

        public void Update() { }

        private IEnumerator PlayTransitionSequence()
        {
            var dialogue = Battle.DialogueBox;
            var monster = Battle.Player.Party.SelectedMonster;

            ViewManager.Instance.Close<PartyMenuOptionsView>();

            // Block invalid selections (fainted or already active) and reset the menu state
            if (monster.IsFainted || monster == Battle.PlayerActiveMonster)
            {
                // Close the options popup immediately to prevent double-clicks
                ViewManager.Instance.Close<PartyMenuOptionsView>();

                yield return dialogue.ShowBattleSequence(GetMessage(monster));

                // Return to either Selection or BattleForcedSelection based on the context
                partyPresenter.ReturnToBaseSelection();
                yield break;
            }

            PerformCleanup();
            yield return new WaitUntil(() => !ViewManager.Instance.IsTransitioning);

            if (isForced)
            {
                machine.SetState(new PlayerForcedSendOutState(machine, monster));
            }
            else
            {
                // Voluntary swap consumes the turn; AI selects a move
                var opponentMove = Battle.OpponentActiveMonster.GetRandomMove();
                machine.SetState(new PlayerSwapMonsterState(machine, monster, opponentMove));
            }
        }

        private string GetMessage(Monster monster)
        {
            if (monster.IsFainted) return BattleMessages.MonsterHasNoEnergy(monster.Definition.DisplayName);
            if (monster == Battle.PlayerActiveMonster) return BattleMessages.MonsterAlreadyInBattle;

            return string.Empty;
        }

        private void PerformCleanup()
        {
            if (partyView != null) partyView.BackRequested -= OnBackRequested;
            if (optionsView != null) optionsView.SwapRequested -= OnSwapRequested;

            ViewManager.Instance.Close<PartyMenuOptionsView>();
            ViewManager.Instance.Close<PartyMenuView>();
        }

        private void OnSwapRequested() => Battle.StartCoroutine(PlayTransitionSequence());
        private void OnBackRequested() => machine.SetState(new PlayerActionMenuState(machine));
    }
}
