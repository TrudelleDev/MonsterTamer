using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Opponent;
using MonsterTamer.Moves;
using MonsterTamer.Moves.Models;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Battle.States.Player
{
    /// <summary>
    /// Executes the player's move and determines whether to pass the turn to the opponent or end the round.
    /// </summary>
    internal sealed class PlayerTurnState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly Move playerMove;
        private readonly Move opponentMove;
        private readonly bool isActingFirst;

        private BattleView Battle => machine.BattleView;

        internal PlayerTurnState(BattleStateMachine machine, Move playerMove, Move opponentMove, bool isFirst)
        {
            this.machine = machine;
            this.playerMove = playerMove;
            this.opponentMove = opponentMove;
            this.isActingFirst = isFirst;
        }

        public void Enter() => Battle.StartCoroutine(PlaySequence());

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            // Wait for UI Transitiom
            yield return new WaitUntil(() => !ViewManager.Instance.IsTransitioning);

            var player = Battle.PlayerActiveMonster;
            var opponent = Battle.OpponentActiveMonster;
            var context = new MoveContext(Battle, player, opponent, playerMove);
            var useMoveMessage = BattleMessages.UseMove(player.Definition.DisplayName, playerMove.Definition.DisplayName);

            yield return Battle.DialogueBox.DisplayAndWaitTyping(useMoveMessage);
            yield return playerMove.Definition.MoveEffect.PerformMoveSequence(context);
            yield return Battle.TurnPauseYield;

            if (opponent.IsFainted)
            {
                machine.SetState(new OpponentFaintedState(machine, opponent));
                yield break;
            }

            if (player.IsFainted)
            {
                machine.SetState(new PlayerFaintedState(machine, player));
                yield break;
            }

            if (isActingFirst)
            {
                // Player was first, now the opponent gets their turn
                machine.SetState(new OpponentTurnState(machine, opponentMove, playerMove, false));
            }
            else
            {
                // Player was second, round is over
                machine.SetState(new PlayerActionMenuState(machine));
            }
        }
    }
}
