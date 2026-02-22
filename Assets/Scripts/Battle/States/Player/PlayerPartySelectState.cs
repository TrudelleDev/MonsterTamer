using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Dialogue;
using MonsterTamer.Party.Enums;
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

            partyPresenter.IsBattleSwap = this.isForced;

            partyPresenter.StartBattleSelection(isForced);

            if (!isForced)
            {
                partyView.BackRequested += OnBackRequested;
            }

            optionsView.SwapRequested += OnSwapRequested;

            partyView.Refresh();
        }

        public void Exit()
        {
            if (partyView != null)
                partyView.BackRequested -= OnBackRequested;

            if (optionsView != null)
                optionsView.SwapRequested -= OnSwapRequested;

            ViewManager.Instance.Close<PartyMenuOptionsView>();
            ViewManager.Instance.Close<PartyMenuView>();
        }

        public void Update() { }

        private IEnumerator PlayTransitionSequence()
        {
            var dialogue = DialogueBoxOverworld.Instance.Dialogue;
            var monster = Battle.Player.Party.SelectedMonster;

            // Active Monster must have remaining HP
            if (monster.IsFainted)
            {
                ViewManager.Instance.Close<PartyMenuOptionsView>();
                var noEnergyMessage = BattleMessages.MonsterHasNoEnergy(monster.Definition.DisplayName);

                yield return dialogue.DisplayAndWaitTyping(noEnergyMessage);
                yield break;
            }

            // Cannot swap to self
            if (monster == Battle.PlayerActiveMonster)
            {
                ViewManager.Instance.Close<PartyMenuOptionsView>();
                yield return dialogue.DisplayAndWaitTyping(BattleMessages.MonsterAlreadyInBattle);
                yield break;
            }

            // Clean up and Transition
            Exit();

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

        private void OnSwapRequested() => Battle.StartCoroutine(PlayTransitionSequence());
        private void OnBackRequested() => machine.SetState(new PlayerActionMenuState(machine));
    }
}
