using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.UI;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Displays the primary action menu (Fight, Bag, Monsters, Flee) and handles player selection.
    /// </summary>
    internal sealed class PlayerActionMenuState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private BattleActionView actionPanel;

        private BattleView Battle => machine.BattleView;

        internal PlayerActionMenuState(BattleStateMachine machine)
        {
            this.machine = machine;
        }

        public void Enter()
        {
            Battle.StartCoroutine(SetupUIAndAwaitInput());
        }

        public void Update() { }

        public void Exit()
        {
            if (actionPanel == null) return;

            actionPanel.MoveSelectionRequested -= OnMoveSelectionRequested;
            actionPanel.PartyRequested -= OnPartyRequested;
            actionPanel.InventoryRequested -= OnInventoryRequested;
            actionPanel.EscapeRequested -= OnEscapeRequested;

            ViewManager.Instance.Close<BattleActionView>();
        }

        private IEnumerator SetupUIAndAwaitInput()
        {
            // Wait for UI transitions
            yield return new WaitUntil(() => !ViewManager.Instance.IsTransitioning);

            actionPanel = ViewManager.Instance.Show<BattleActionView>();

            var chooseActionMessage = BattleMessages.ChooseAction(Battle.PlayerActiveMonster.Definition.DisplayName);
            Battle.DialogueBox.DisplayInstant(chooseActionMessage);

            actionPanel.MoveSelectionRequested += OnMoveSelectionRequested;
            actionPanel.PartyRequested += OnPartyRequested;
            actionPanel.InventoryRequested += OnInventoryRequested;
            actionPanel.EscapeRequested += OnEscapeRequested;
        }

        private IEnumerator ShowEscapeFailDialogue()
        {
            ViewManager.Instance.Close<BattleActionView>();
            yield return Battle.DialogueBox.DisplayAndWaitTyping(BattleMessages.EscapeTrainer);

            machine.SetState(new PlayerActionMenuState(machine));
        }

        private void OnEscapeRequested()
        {
            // Determine if the battle involves a Trainer or a wild monster
            if (!Battle.Opponent)
            {
                // Wild monster battle
                machine.SetState(new PlayerEscapeState(machine));
            }
            else
            {
                // Trainer battle
                Battle.StartCoroutine(ShowEscapeFailDialogue());
            }
        }

        private void OnMoveSelectionRequested() => machine.SetState(new PlayerMoveSelectState(machine));
        private void OnPartyRequested() => machine.SetState(new PlayerPartySelectState(machine));
        private void OnInventoryRequested() => machine.SetState(new PlayerInventoryState(machine));
    }
}
