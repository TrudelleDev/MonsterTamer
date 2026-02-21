using System.Collections;
using MonsterTamer.Battle.Models;
using MonsterTamer.Battle.States.Core;
using MonsterTamer.Battle.States.Player;
using MonsterTamer.Moves;
using MonsterTamer.Moves.Models;
using MonsterTamer.Views;
using UnityEngine;

namespace MonsterTamer.Battle.States.Opponent
{
    /// <summary>
    /// Execute the opponent's turn by executing their move and resolving the outcome.
    /// </summary>
    internal sealed class OpponentTurnState : IBattleState
    {
        private readonly BattleStateMachine machine;
        private readonly Move opponentMove;
        private readonly Move playerMove;
        private readonly bool isActingFirst;

        private BattleView Battle => machine.BattleView;

        internal OpponentTurnState(BattleStateMachine machine, Move opponentMove, Move playerMove, bool isActingFirst)
        {
            this.machine = machine;
            this.opponentMove = opponentMove;
            this.playerMove = playerMove;
            this.isActingFirst = isActingFirst;
        }

        public void Enter()
        {
            Battle.StartCoroutine(PlaySequence());
        }

        public void Update() { }
        public void Exit() { }

        private IEnumerator PlaySequence()
        {
            // Wait for UI transitions
            yield return new WaitUntil(() => !ViewManager.Instance.IsTransitioning);

            var opponent = Battle.OpponentActiveMonster;
            var player = Battle.PlayerActiveMonster;
            var context = new MoveContext(Battle, opponent, player, opponentMove);
            var useMoveMessage = BattleMessages.UseMove(opponent.Definition.DisplayName, opponentMove.Definition.DisplayName);

            yield return Battle.DialogueBox.DisplayAndWaitTyping(useMoveMessage);
            yield return opponentMove.Definition.MoveEffect.PerformMoveSequence(context);
            yield return Battle.TurnPauseYield;

            if (player.IsFainted)
            {
                machine.SetState(new PlayerFaintedState(machine, player));
                yield break;
            }

            if (opponent.IsFainted)
            {
                machine.SetState(new OpponentFaintedState(machine, opponent));
                yield break;
            }

            if (isActingFirst)
            {
                // Opponent was first, now player gets their turn
                machine.SetState(new PlayerTurnState(machine, playerMove, opponentMove, false));
            }
            else
            {
                // Opponent was second, round is over
                machine.SetState(new PlayerActionMenuState(machine));
            }
        }
    }
}
